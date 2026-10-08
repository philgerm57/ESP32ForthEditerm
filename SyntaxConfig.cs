using System.Drawing;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ForthEditor;

/// <summary>Une règle de coloration : soit une liste de mots, soit une expression régulière.</summary>
public class SyntaxRule
{
    public string Name { get; set; } = "";
    /// <summary>Mots Forth séparés (si renseigné, prioritaire sur Pattern).</summary>
    public List<string> Words { get; set; } = new();
    /// <summary>Expression régulière .NET (Multiline activé).</summary>
    public string Pattern { get; set; } = "";
    public string Color { get; set; } = "#D4D4D4";
    public bool Bold { get; set; }
    public bool Italic { get; set; }
}

/// <summary>Configuration complète, chargée depuis syntax.json. Les règles sont appliquées dans l'ordre :
/// une règle placée plus bas écrase les précédentes (les commentaires sont donc en dernier).</summary>
public class SyntaxConfig
{
    public bool CaseSensitive { get; set; } = false;
    public string FontName { get; set; } = "Consolas";
    public float FontSize { get; set; } = 11f;
    public string Background { get; set; } = "#1E1E1E";
    public string Foreground { get; set; } = "#D4D4D4";
    public string LineNumberForeground { get; set; } = "#858585";
    public string LineNumberBackground { get; set; } = "#252526";
    public List<SyntaxRule> Rules { get; set; } = new();

    /// <summary>Indentation automatique à l'appui sur Entrée.</summary>
    public bool AutoIndent { get; set; } = true;
    public int IndentSize { get; set; } = 4;
    /// <summary>Indente d'un cran la ligne suivant une ligne « : nom ... » non terminée par « ; ».</summary>
    public bool IndentAfterColon { get; set; } = true;
    /// <summary>La touche Tab insère IndentSize espaces au lieu d'un caractère de tabulation.</summary>
    public bool TabUsesSpaces { get; set; }

    /// <summary>Nom affiché pour cette syntaxe (libre).</summary>
    public string Name { get; set; } = "";
    /// <summary>Extensions de fichiers colorées par ce fichier, ex. [".json", ".jsonc"].</summary>
    public List<string> Extensions { get; set; } = new();
    [JsonIgnore] public string SourcePath { get; set; }

    /// <summary>Thèmes alternatifs = surcharges de couleurs. Le thème "dark" correspond aux valeurs de base ci-dessus.</summary>
    public Dictionary<string, ThemePalette> Themes { get; set; } = new();

    static readonly JsonSerializerOptions JsonOpt = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static SyntaxConfig Load(string path)
    {
        if (File.Exists(path))
        {
            try { return Normalize(JsonSerializer.Deserialize<SyntaxConfig>(File.ReadAllText(path), JsonOpt)); }
            catch (Exception ex)
            {
                MessageBox.Show($"syntax.json invalide, configuration par défaut utilisée.\n\n{ex.Message}",
                    "Coloration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return Default();
            }
        }
        var d = Default();
        try { File.WriteAllText(path, JsonSerializer.Serialize(d, JsonOpt)); } catch { }
        return d;
    }

    // ---- Configuration par défaut (modifiable ici, ou plus simplement dans syntax.json) ----
    static SyntaxRule W(string name, string color, string words, bool bold = false, bool italic = false) => new()
    {
        Name = name, Color = color, Bold = bold, Italic = italic,
        Words = words.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList()
    };
    static SyntaxRule P(string name, string color, string pattern, bool bold = false, bool italic = false) => new()
    { Name = name, Color = color, Pattern = pattern, Bold = bold, Italic = italic };

    static SyntaxConfig Normalize(SyntaxConfig c)
    {
        c ??= Default();
        c.Themes ??= new();
        c.Rules ??= new();
        c.Extensions ??= new();
        if (!c.Themes.ContainsKey("light"))
        {
            // Palette Forth si le fichier utilise les noms de règles Forth, sinon couleurs assombries automatiquement
            var forth = LightPalette();
            c.Themes["light"] = c.Rules.Any(r => forth.RuleColors.ContainsKey(r.Name ?? "")) ? forth : AutoLightPalette(c);
        }
        return c;
    }

    static ThemePalette LightPalette() => new()
    {
        Background = "#FFFFFF", Foreground = "#1E1E1E",
        LineNumberForeground = "#6E7681", LineNumberBackground = "#F3F3F3",
        RuleColors = new()
        {
            ["opérateurs"] = "#795E26", ["pile"] = "#267F99", ["mémoire / E-S"] = "#001080",
            ["contrôle"] = "#0000FF", ["définition"] = "#AF00DB", ["nombres"] = "#098658",
            ["nom défini"] = "#0070C1", ["chaînes"] = "#A31515",
            ["commentaire ( )"] = "#008000", ["commentaire \\"] = "#008000",
        }
    };

    public static SyntaxConfig Default() => new()
    {
        Name = "Forth",
        Extensions = new() { ".fs", ".fth", ".4th", ".forth", ".f", ".fb", ".fr", ".frt", ".seq" },
        Themes = new() { ["light"] = LightPalette() },
        Rules = new List<SyntaxRule>
        {
            W("opérateurs", "#D7BA7D", "+ - * / MOD /MOD */ NEGATE ABS MIN MAX AND OR XOR INVERT LSHIFT RSHIFT = <> < > <= >= 0= 0< 0> U< 1+ 1- 2* 2/"),
            W("pile", "#4EC9B0", "DUP DROP SWAP OVER ROT -ROT NIP TUCK 2DUP 2DROP 2SWAP 2OVER ?DUP PICK ROLL DEPTH >R R> R@"),
            W("mémoire / E-S", "#DCDCAA", "@ ! C@ C! +! , C, ALLOT HERE CELLS CELL+ CHARS TYPE EMIT CR . .S U. KEY ACCEPT SPACE SPACES MOVE FILL WORDS"),
            W("contrôle", "#569CD6", "IF ELSE THEN DO ?DO LOOP +LOOP LEAVE UNLOOP BEGIN WHILE REPEAT UNTIL AGAIN CASE OF ENDOF ENDCASE EXIT I J K", bold: true),
            W("définition", "#C586C0", ": ; :NONAME CREATE DOES> VARIABLE CONSTANT 2VARIABLE 2CONSTANT VALUE TO DEFER IS IMMEDIATE POSTPONE [ ] ['] [CHAR] CHAR RECURSE", bold: true),
            P("nombres", "#B5CEA8", @"(?<!\S)[-+]?(?:\d+(?:\.\d*)?(?:e[-+]?\d+)?|\$[0-9a-f]+|0x[0-9a-f]+|#\d+|%[01]+)(?!\S)"),
            P("nom défini", "#4FC1FF", @"(?<=(?<!\S)(?::|VARIABLE|CONSTANT|2VARIABLE|2CONSTANT|CREATE|VALUE|DEFER)[ \t]+)\S+", bold: true),
            P("chaînes", "#CE9178", @"(?<!\S)(?:\.|S|C|ABORT|S\\)""\s[^""]*"""),
            P("commentaire ( )", "#6A9955", @"(?<!\S)\((?=\s)[^)]*\)", italic: true),
            P("commentaire \\", "#6A9955", @"(?<!\S)\\(?!\S).*$", italic: true),
        }
    };

    // ---- Fichiers de syntaxe supplémentaires (dossier « syntaxes ») ----
    /// <summary>Charge un fichier de syntaxe ; retourne null (et un message) s'il est illisible.</summary>
    public static SyntaxConfig TryLoad(string path, out string error)
    {
        error = null;
        try
        {
            var c = Normalize(JsonSerializer.Deserialize<SyntaxConfig>(File.ReadAllText(path), JsonOpt));
            if (string.IsNullOrWhiteSpace(c.Name)) c.Name = Path.GetFileNameWithoutExtension(path);
            c.SourcePath = path;
            return c;
        }
        catch (Exception ex) { error = $"{Path.GetFileName(path)} : {ex.Message}"; return null; }
    }

    static string Darken(string html)
    {
        try
        {
            var c = ColorTranslator.FromHtml(html);
            return ColorTranslator.ToHtml(Color.FromArgb((int)(c.R * 0.55), (int)(c.G * 0.55), (int)(c.B * 0.55)));
        }
        catch { return html; }
    }

    /// <summary>Thème clair déduit des couleurs sombres : mêmes règles, couleurs assombries, fond blanc.</summary>
    static ThemePalette AutoLightPalette(SyntaxConfig c) => new()
    {
        Background = "#FFFFFF", Foreground = "#1E1E1E", LineNumberForeground = "#6E7681", LineNumberBackground = "#F3F3F3",
        RuleColors = c.Rules.Where(r => !string.IsNullOrEmpty(r.Name)).GroupBy(r => r.Name).ToDictionary(g => g.Key, g => Darken(g.First().Color))
    };

    /// <summary>Crée deux modèles dans le dossier « syntaxes » : JSON et texte brut.</summary>
    public static void WriteExamples(string folder)
    {
        var json = new SyntaxConfig
        {
            Name = "JSON", Extensions = new() { ".json" }, IndentAfterColon = false,
            Rules = new List<SyntaxRule>
            {
                P("chaînes", "#CE9178", @"""(?:[^""\\\r\n]|\\.)*"""),
                P("clés", "#9CDCFE", @"""(?:[^""\\\r\n]|\\.)*""(?=\s*:)"),
                P("nombres", "#B5CEA8", @"(?<![\w.])-?\d+(?:\.\d+)?(?:[eE][+-]?\d+)?(?![\w.])"),
                P("constantes", "#569CD6", @"\b(?:true|false|null)\b", bold: true),
            }
        };
        var text = new SyntaxConfig
        {
            Name = "Texte brut", Extensions = new() { ".txt", ".md", ".log", ".ini", ".cfg" },
            IndentAfterColon = false, Rules = new List<SyntaxRule>()
        };
        File.WriteAllText(Path.Combine(folder, "json.json"), JsonSerializer.Serialize(json, JsonOpt));
        File.WriteAllText(Path.Combine(folder, "texte.json"), JsonSerializer.Serialize(text, JsonOpt));
    }

    // ---- Compilation en objets prêts à l'emploi ----
    static Color Col(string html, Color fallback)
    {
        try { return ColorTranslator.FromHtml(html); } catch { return fallback; }
    }

    static string RuleColor(ThemePalette pal, SyntaxRule r) =>
        pal?.RuleColors != null && pal.RuleColors.TryGetValue(r.Name ?? "", out var c) ? c : r.Color;

    /// <param name="themeName">"dark" (valeurs de base) ou nom d'un thème de la section "themes".</param>
    /// <param name="zoom">Points ajoutés à la taille de police.</param>
    public SyntaxTheme Build(string themeName = "dark", float zoom = 0)
    {
        ThemePalette pal = null;
        if (themeName != null) Themes?.TryGetValue(themeName, out pal);
        var theme = new SyntaxTheme
        {
            Font = new Font(FontName, Math.Clamp(FontSize + zoom, 6f, 72f)),
            Foreground = Col(pal?.Foreground ?? Foreground, Color.Gainsboro),
            Background = Col(pal?.Background ?? Background, Color.FromArgb(30, 30, 30)),
            NumberForeground = Col(pal?.LineNumberForeground ?? LineNumberForeground, Color.Gray),
            NumberBackground = Col(pal?.LineNumberBackground ?? LineNumberBackground, Color.FromArgb(37, 37, 38)),
            AutoIndent = AutoIndent, IndentSize = Math.Clamp(IndentSize, 1, 16), IndentAfterColon = IndentAfterColon, TabUsesSpaces = TabUsesSpaces
        };
        var opt = RegexOptions.Multiline | RegexOptions.CultureInvariant |
                  (CaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase);
        foreach (var r in Rules)
        {
            try
            {
                string pat = r.Pattern;
                if (r.Words is { Count: > 0 })
                    pat = @"(?<!\S)(?:" + string.Join("|", r.Words.OrderByDescending(w => w.Length).Select(Regex.Escape)) + @")(?!\S)";
                if (string.IsNullOrEmpty(pat)) continue;
                if (r.Words is { Count: > 0 }) theme.Keywords.AddRange(r.Words);
                var style = (r.Bold ? FontStyle.Bold : 0) | (r.Italic ? FontStyle.Italic : 0);
                theme.Rules.Add(new CompiledRule
                {
                    Regex = new Regex(pat, opt),
                    Color = ColorTranslator.FromHtml(RuleColor(pal, r)),
                    Font = style == 0 ? theme.Font : new Font(theme.Font, style)
                });
            }
            catch (Exception ex) { theme.Errors.Add($"Règle « {r.Name} » : {ex.Message}"); }
        }
        theme.Keywords = theme.Keywords.Where(w => w.Length >= 2).Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(w => w, StringComparer.OrdinalIgnoreCase).ToList();
        return theme;
    }
}

public class CompiledRule { public Regex Regex; public Color Color; public Font Font; }

public class SyntaxTheme
{
    public Font Font;
    public Color Foreground, Background, NumberForeground, NumberBackground;
    public List<CompiledRule> Rules = new();
    public List<string> Errors = new();
    public List<string> Keywords = new();   // mots Forth connus (pour la complétion)
    public bool AutoIndent = true, IndentAfterColon = true, TabUsesSpaces;
    public int IndentSize = 4;
}

/// <summary>Surcharge de couleurs pour un thème (les valeurs absentes reprennent celles de base).</summary>
public class ThemePalette
{
    public string Background { get; set; }
    public string Foreground { get; set; }
    public string LineNumberForeground { get; set; }
    public string LineNumberBackground { get; set; }
    /// <summary>Couleur par nom de règle, ex. "contrôle": "#0000FF"</summary>
    public Dictionary<string, string> RuleColors { get; set; } = new();
}
