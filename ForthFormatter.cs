namespace ForthEditor;

/// <summary>
/// Réindentation du code Forth selon les structures de contrôle :
///   : … ;   IF … ELSE … THEN   BEGIN … UNTIL / AGAIN   BEGIN … WHILE … REPEAT   DO / ?DO … LOOP / +LOOP
///   FOR … NEXT   FOR … AFT … THEN … NEXT   CASE … OF … ENDOF … ENDCASE   [IF] … [ELSE] … [THEN]   DOES>
/// Les commentaires (\ … et ( … )) et les chaînes (." … ") sont ignorés, y compris sur plusieurs lignes.
/// Les lignes qui commencent à l'intérieur d'un commentaire ou d'une chaîne ne sont jamais modifiées
/// (changer leurs espaces changerait le texte de la chaîne).
/// </summary>
public static class ForthFormatter
{
    public sealed class Options
    {
        public int IndentSize = 4;
        public bool UseTabs;
        public bool TrimTrailing = true;
    }

    sealed class Entry
    {
        public readonly string Kind;
        public readonly bool Indents;      // false : marqueur invisible (AFT), qui ne décale pas le texte
        public Entry(string kind, bool indents) { Kind = kind; Indents = indents; }
    }

    sealed class State
    {
        public readonly List<Entry> Stack = new();
        public bool InParen, InString, SkipNext;
        public int Depth { get { int d = 0; foreach (var e in Stack) if (e.Indents) d++; return d; } }
    }

    static readonly StringComparer Cmp = StringComparer.OrdinalIgnoreCase;

    static readonly HashSet<string> StringWords = new(Cmp) { ".\"", "S\"", "C\"", "ABORT\"", "S\\\"", ".\\\"" };
    static readonly HashSet<string> SkipNextWords = new(Cmp)
    {
        "'", "[']", "POSTPONE", "[COMPILE]", "CHAR", "[CHAR]", "TO", "IS", "SEE", "FORGET", "ACTION-OF"
    };

    static readonly Dictionary<string, string> Openers = new(Cmp)
    {
        ["IF"] = "IF", ["BEGIN"] = "BEGIN", ["FOR"] = "FOR", ["CASE"] = "CASE", ["OF"] = "OF", ["[IF]"] = "[IF]",
        ["DO"] = "DO", ["?DO"] = "DO", ["+DO"] = "DO", ["-DO"] = "DO", ["U+DO"] = "DO", ["U-DO"] = "DO",
    };

    static readonly Dictionary<string, string[]> Closers = new(Cmp)
    {
        ["THEN"] = new[] { "IF", "AFT" }, ["ENDIF"] = new[] { "IF", "AFT" },
        ["UNTIL"] = new[] { "BEGIN" }, ["AGAIN"] = new[] { "BEGIN" }, ["REPEAT"] = new[] { "BEGIN" },
        ["LOOP"] = new[] { "DO" }, ["+LOOP"] = new[] { "DO" }, ["-LOOP"] = new[] { "DO" },
        ["NEXT"] = new[] { "FOR" }, ["ENDCASE"] = new[] { "CASE" }, ["ENDOF"] = new[] { "OF" },
        ["[THEN]"] = new[] { "[IF]" }, ["[ENDIF]"] = new[] { "[IF]" },
    };

    // Mots « intermédiaires » : ils s'alignent sur leur ouvreur mais le texte qui suit reste décalé
    static readonly Dictionary<string, string> Middles = new(Cmp)
    {
        ["ELSE"] = "IF", ["[ELSE]"] = "[IF]", ["WHILE"] = "BEGIN", ["DOES>"] = "COLON", ["AFT"] = "FOR",
    };

    /// <summary>
    /// Retourne les nouvelles lignes pour [firstLine, lastLine] (indices 0, bornes incluses). Toutes les lignes précédentes
    /// sont analysées pour connaître le contexte (on peut donc réindenter une simple sélection au milieu d'une définition).
    /// </summary>
    public static string[] ReindentRange(string[] lines, int firstLine, int lastLine, Options o)
    {
        var st = new State();
        var result = new List<string>();
        lastLine = Math.Min(lastLine, lines.Length - 1);
        for (int ln = 0; ln <= lastLine; ln++)
        {
            string line = lines[ln];
            int indent = ProcessLine(st, line, out bool continuation);
            if (ln < firstLine) continue;
            if (continuation) { result.Add(line); continue; }        // début de ligne dans un commentaire / une chaîne : intacte

            string content = o.TrimTrailing ? line.Trim() : line.TrimStart();
            if (content.Trim().Length == 0) { result.Add(o.TrimTrailing ? "" : line); continue; }
            string pad = o.UseTabs ? new string('\t', indent) : new string(' ', indent * o.IndentSize);
            result.Add(pad + content);
        }
        return result.ToArray();
    }

    static int FindFromTop(State st, string[] kinds)
    {
        for (int i = st.Stack.Count - 1; i >= 0; i--)
            if (Array.IndexOf(kinds, st.Stack[i].Kind) >= 0) return i;
        return -1;
    }

    /// <summary>Retourne la position qui suit le guillemet fermant, ou -1 s'il n'est pas sur cette ligne.</summary>
    static int StringEnd(string line, int i, bool escaped)
    {
        if (!escaped)
        {
            int e = line.IndexOf('"', i);
            return e < 0 ? -1 : e + 1;
        }
        while (i < line.Length)
        {
            if (line[i] == '\\') { i += 2; continue; }
            if (line[i] == '"') return i + 1;
            i++;
        }
        return -1;
    }

    /// <summary>Met à jour la pile des structures avec les mots de la ligne et retourne le niveau d'indentation de cette ligne.</summary>
    static int ProcessLine(State st, string line, out bool continuation)
    {
        continuation = st.InParen || st.InString;
        int n = line.Length, i = 0;
        int indent = st.Depth;
        bool lead = true;      // seuls des mots fermants / intermédiaires ont été vus jusqu'ici
        bool first = true;

        if (st.InParen)
        {
            int e = line.IndexOf(')');
            if (e < 0) return indent;
            i = e + 1; st.InParen = false;
        }
        else if (st.InString)
        {
            int e = line.IndexOf('"');
            if (e < 0) return indent;
            i = e + 1; st.InString = false;
        }

        while (true)
        {
            while (i < n && char.IsWhiteSpace(line[i])) i++;
            if (i >= n) break;
            int start = i;
            while (i < n && !char.IsWhiteSpace(line[i])) i++;
            string tok = line.Substring(start, i - start);
            bool wasFirst = first;
            first = false;

            if (st.SkipNext) { st.SkipNext = false; lead = false; continue; }
            if (tok == "\\") break;                                          // commentaire jusqu'à la fin de la ligne
            if (tok == "(" || tok == ".(")                                   // commentaire ( … )
            {
                int e = line.IndexOf(')', i);
                if (e < 0) { st.InParen = true; break; }
                i = e + 1;
                continue;
            }
            if (StringWords.Contains(tok))                                   // chaîne ." … "
            {
                int e = StringEnd(line, i, tok.Contains('\\'));
                if (e < 0) { st.InString = true; break; }
                i = e; lead = false;
                continue;
            }

            if (tok == ":" || tok.Equals(":NONAME", StringComparison.OrdinalIgnoreCase))
            {
                st.Stack.Clear();                                            // une définition ne s'imbrique pas : on repart de zéro
                if (lead) indent = 0;
                st.Stack.Add(new Entry("COLON", true));
                lead = false;
            }
            else if (Openers.TryGetValue(tok, out var openKind))
            {
                st.Stack.Add(new Entry(openKind, true));
                lead = false;
            }
            else if (tok == ";")
            {
                int idx = -1;
                for (int k = st.Stack.Count - 1; k >= 0; k--)
                    if (st.Stack[k].Kind == "COLON") { idx = k; break; }
                st.Stack.RemoveRange(idx >= 0 ? idx : 0, st.Stack.Count - (idx >= 0 ? idx : 0));
                if (lead && wasFirst) indent = Math.Min(indent, st.Depth);   // « THEN ; » reste au niveau de THEN
            }
            else if (Closers.TryGetValue(tok, out var closeKinds))
            {
                int idx = FindFromTop(st, closeKinds);
                if (idx < 0) { lead = false; continue; }                     // fermeture sans ouverture : ignorée
                bool marker = !st.Stack[idx].Indents;                        // THEN qui ferme un AFT : mot intermédiaire
                if (marker)
                {
                    if (lead) indent = Math.Min(indent, st.Depth - 1);
                    st.Stack.RemoveRange(idx, st.Stack.Count - idx);
                }
                else
                {
                    st.Stack.RemoveRange(idx, st.Stack.Count - idx);
                    if (lead) indent = Math.Min(indent, st.Depth);
                }
            }
            else if (Middles.TryGetValue(tok, out var midKind))
            {
                int idx = FindFromTop(st, new[] { midKind });
                if (idx < 0) { lead = false; continue; }
                st.Stack.RemoveRange(idx + 1, st.Stack.Count - idx - 1);     // structures internes restées ouvertes
                if (lead) indent = Math.Min(indent, st.Depth - 1);
                if (tok.Equals("AFT", StringComparison.OrdinalIgnoreCase)) st.Stack.Add(new Entry("AFT", false));
            }
            else
            {
                lead = false;
                if (SkipNextWords.Contains(tok)) st.SkipNext = true;
            }
        }
        return Math.Max(0, indent);
    }
}
