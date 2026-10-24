// ForthEditor — éditeur de code Forth et terminal série pour ESP32Forth
// Copyright (C) 2026 Germain PHILIPPE
// Licence : GNU General Public License v3.0 ou ultérieure — voir le fichier LICENSE.

namespace ForthEditor;

public sealed class ForthDefinition
{
    public string Kind = ":";          // « : », VARIABLE, CONSTANT, CREATE…
    public string Name = "";
    public int Index;                  // position du mot de définition
    public int NameEnd;                // position juste après le nom défini
    public bool AtLineStart;
}

public sealed class ForthScan
{
    public List<ForthDefinition> Definitions = new();
    public List<int> Semicolons = new();    // positions des « ; » qui terminent réellement une définition
}

/// <summary>
/// Analyse lexicale minimale du Forth pour le plan des définitions et le repliement.
/// Elle découpe le texte en mots (séparés par des espaces) et ignore ce qui n'est pas du code :
/// commentaires « \ … » et « ( … ) », chaînes « ." … " », « S" … " », « ABORT" … " », ainsi que le mot
/// qui suit « ' », « [CHAR] », « POSTPONE »… (un nom cité n'est pas une définition).
/// </summary>
public static class ForthScanner
{
    static readonly HashSet<string> Definers = new(StringComparer.OrdinalIgnoreCase)
    {
        ":", "VARIABLE", "2VARIABLE", "FVARIABLE", "CONSTANT", "2CONSTANT", "FCONSTANT",
        "CREATE", "VALUE", "2VALUE", "FVALUE", "DEFER", "BUFFER:", "MARKER"
    };

    // Mots qui lisent un texte jusqu'au guillemet fermant
    static readonly HashSet<string> StringWords = new(StringComparer.OrdinalIgnoreCase)
    {
        ".\"", "S\"", "C\"", "ABORT\"", "S\\\"", ".\\\""
    };

    // Mots dont le mot suivant est une simple référence ou un littéral, pas une définition
    static readonly HashSet<string> SkipNext = new(StringComparer.OrdinalIgnoreCase)
    {
        "'", "[']", "POSTPONE", "[COMPILE]", "CHAR", "[CHAR]", "TO", "IS", "SEE", "FORGET", "ACTION-OF"
    };

    public static ForthScan Scan(string text)
    {
        var res = new ForthScan();
        int n = text.Length, i = 0;
        ForthDefinition pending = null;       // mot de définition en attente de son nom
        bool skipNext = false;
        bool inColon = false;                 // à l'intérieur d'une définition « : … ; »

        while (true)
        {
            while (i < n && char.IsWhiteSpace(text[i])) i++;
            if (i >= n) break;
            int start = i;
            while (i < n && !char.IsWhiteSpace(text[i])) i++;
            string tok = text.Substring(start, i - start);

            if (skipNext) { skipNext = false; continue; }

            if (tok == "\\")                                         // commentaire jusqu'à la fin de la ligne
            {
                while (i < n && text[i] != '\n') i++;
                continue;
            }
            if (tok == "(" || tok == ".(")                           // commentaire ( … ) ou affichage .( … )
            {
                int e = text.IndexOf(')', i);
                i = e < 0 ? n : e + 1;
                continue;
            }
            if (StringWords.Contains(tok))                           // chaîne : tout jusqu'au guillemet fermant est ignoré
            {
                i = SkipString(text, i, tok.Contains('\\'));
                pending = null;
                continue;
            }
            if (pending != null)                                     // le mot qui suit un définisseur est le nom défini
            {
                if (tok != ";" && !tok.Contains('"'))
                {
                    pending.Name = tok;
                    pending.NameEnd = i;
                    res.Definitions.Add(pending);
                    if (pending.Kind == ":") inColon = true;
                }
                pending = null;
                continue;
            }
            if (tok == ";") { res.Semicolons.Add(start); inColon = false; continue; }
            if (tok.Equals(":NONAME", StringComparison.OrdinalIgnoreCase)) { inColon = true; continue; }
            if (Definers.Contains(tok))
            {
                // CREATE, VARIABLE, … écrits DANS une définition « : … ; » s'exécutent plus tard : ce n'est pas une définition de mot
                if (inColon && tok != ":") continue;
                pending = new ForthDefinition { Kind = tok.ToUpperInvariant(), Index = start, AtLineStart = IsLineStart(text, start) };
                continue;
            }
            if (SkipNext.Contains(tok)) skipNext = true;
        }
        return res;
    }

    static bool IsLineStart(string text, int index)
    {
        for (int k = index - 1; k >= 0; k--)
        {
            if (text[k] == '\n') return true;
            if (text[k] != ' ' && text[k] != '\t') return false;
        }
        return true;
    }

    /// <summary>Retourne la position qui suit le guillemet fermant (avec ou sans échappement \").</summary>
    static int SkipString(string text, int i, bool escaped)
    {
        int n = text.Length;
        if (!escaped)
        {
            int e = text.IndexOf('"', i);
            return e < 0 ? n : e + 1;
        }
        while (i < n)
        {
            if (text[i] == '\\') { i += 2; continue; }
            if (text[i] == '"') return i + 1;
            i++;
        }
        return n;
    }
}
