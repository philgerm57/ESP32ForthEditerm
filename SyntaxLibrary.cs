namespace ForthEditor;

/// <summary>
/// Ensemble des fichiers de coloration : « syntax.json » (par défaut, Forth) et tous les « *.json » du dossier « syntaxes ».
/// Chaque fichier déclare les extensions de fichiers auxquelles il s'applique (« extensions »). Un fichier dont l'extension
/// n'est déclarée nulle part (ou pas encore enregistré) utilise la syntaxe par défaut.
/// </summary>
public class SyntaxLibrary
{
    readonly List<SyntaxConfig> _others = new();
    readonly Dictionary<string, SyntaxConfig> _byExtension = new(StringComparer.OrdinalIgnoreCase);

    public SyntaxConfig Default { get; private set; }
    public IReadOnlyList<SyntaxConfig> Others => _others;
    /// <summary>Fichiers de syntaxe illisibles (JSON invalide…), à signaler à l'utilisateur.</summary>
    public List<string> Errors { get; } = new();

    public static string FolderPath(string baseDirectory) => Path.Combine(baseDirectory, "syntaxes");

    public static SyntaxLibrary Load(string defaultPath, string folder)
    {
        var lib = new SyntaxLibrary { Default = SyntaxConfig.Load(defaultPath) };
        try
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                SyntaxConfig.WriteExamples(folder);        // JSON et texte brut, comme modèles à copier
            }
            foreach (var file in Directory.EnumerateFiles(folder, "*.json").OrderBy(f => f, StringComparer.OrdinalIgnoreCase))
            {
                var cfg = SyntaxConfig.TryLoad(file, out string error);
                if (cfg == null) { lib.Errors.Add(error); continue; }
                lib._others.Add(cfg);
                foreach (var e in cfg.Extensions)
                {
                    string ext = NormalizeExtension(e);
                    if (ext.Length > 0) lib._byExtension[ext] = cfg;       // en cas de doublon, le dernier fichier (ordre alphabétique) l'emporte
                }
            }
        }
        catch (Exception ex) { lib.Errors.Add("Dossier des syntaxes : " + ex.Message); }
        return lib;
    }

    static string NormalizeExtension(string e)
    {
        e = (e ?? "").Trim();
        if (e.Length == 0) return "";
        return e.StartsWith('.') ? e : "." + e;
    }

    /// <summary>Syntaxe à utiliser pour ce fichier, d'après son extension.</summary>
    public SyntaxConfig For(string filePath)
    {
        if (!string.IsNullOrEmpty(filePath) && _byExtension.TryGetValue(Path.GetExtension(filePath), out var cfg)) return cfg;
        return Default;
    }
}
