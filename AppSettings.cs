// ForthEditor — éditeur de code Forth et terminal série pour ESP32Forth
// Copyright (C) 2026 Germain PHILIPPE
// Licence : GNU General Public License v3.0 ou ultérieure — voir le fichier LICENSE.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace ForthEditor;

/// <summary>Une configuration d'interpréteur Forth (nom libre, chemin, arguments).</summary>
public class ForthProfile
{
    public string Name { get; set; } = "";
    /// <summary>Chemin complet de l'exécutable, ou nom seul si présent dans le PATH.</summary>
    public string Exe { get; set; } = "";
    /// <summary>Variables : {file} fichier, {dir} son dossier, {name} son nom.</summary>
    public string Args { get; set; } = "\"{file}\"";
    /// <summary>Arguments utilisés par le bouton « Tester » (vide = lancer sans argument). Ex. --version pour gforth.</summary>
    public string TestArgs { get; set; } = "";
}

/// <summary>Un PDF récemment ouvert dans la visionneuse et la page où l'on s'était arrêté.</summary>
public class PdfRecent
{
    public string Path { get; set; } = "";
    public int Page { get; set; } = 1;
    public string Zoom { get; set; }                       // zoom PDF.js : auto, page-width, page-fit ou facteur (1.25)
}

/// <summary>Préférences utilisateur : %AppData%\ForthEditor\settings.json</summary>
public class AppSettings
{
    public bool RestoreSession { get; set; } = true;
    public string Theme { get; set; } = "dark";      // "dark" | "light" | nom d'un thème défini dans syntax.json
    public float Zoom { get; set; }                  // écart de taille de police par rapport à syntax.json
    public List<string> Session { get; set; } = new();
    public string ActiveFile { get; set; }

    // --- Interpréteurs Forth externes ---
    public List<ForthProfile> Profiles { get; set; } = new();
    public string ActiveProfile { get; set; }
    public bool SaveBeforeRun { get; set; } = true;
    public bool ClearOutputBeforeRun { get; set; } = true;

    // Anciens réglages (un seul interpréteur) : lus uniquement pour migrer vers les configurations nommées
    public string ForthExe { get; set; } = "gforth";
    public string ForthArgs { get; set; } = "\"{file}\" -e bye";

    // --- Panneaux et options ---
    public bool ShowOutline { get; set; } = true;
    public bool ShowOutput { get; set; }
    public bool AutoComplete { get; set; } = true;
    public bool OutlineDefsOnly { get; set; }                 // plan : seulement les définitions « : »

    // --- Aide PDF ---
    public string PdfFolder { get; set; }                     // null = sous-dossier « doc » de l'application
    public int PdfSplit { get; set; }                         // largeur de la liste (0 = défaut)
    public double PdfZoom { get; set; } = 1.0;                // zoom de la visionneuse
    public int[] PdfBounds { get; set; }                      // x, y, largeur, hauteur de la fenêtre
    public bool PdfMaximized { get; set; }
    public int PdfRecentMax { get; set; } = 10;               // nombre de PDF mémorisés (le plus ancien est éliminé)
    public List<PdfRecent> PdfRecents { get; set; } = new();  // du plus récent au plus ancien

    // --- Projet (dossier) ---
    public string ProjectFolder { get; set; }
    public List<string> ProjectFolders { get; set; } = new(); // derniers dossiers ouverts
    public bool ShowProject { get; set; } = true;
    public bool ProjectShowAll { get; set; }                  // false = fichiers Forth seulement

    [JsonIgnore]
    public ForthProfile CurrentProfile =>
        Profiles.FirstOrDefault(p => string.Equals(p.Name, ActiveProfile, StringComparison.OrdinalIgnoreCase)) ?? Profiles.FirstOrDefault();

    /// <summary>Garantit au moins une configuration (migration de l'ancien réglage + exemples).</summary>
    void EnsureProfiles()
    {
        Profiles ??= new();
        if (Profiles.Count == 0)
        {
            Profiles.Add(new ForthProfile { Name = "gforth — exécute puis quitte", Exe = "gforth", Args = "\"{file}\" -e bye", TestArgs = "--version" });
            Profiles.Add(new ForthProfile { Name = "gforth — reste interactif", Exe = "gforth", Args = "\"{file}\"", TestArgs = "--version" });
            Profiles.Add(new ForthProfile { Name = "pForth — reste interactif", Exe = "pforth", Args = "\"{file}\"" });

            bool legacyIsDefault = ForthExe == "gforth" && ForthArgs == "\"{file}\" -e bye";
            if (!string.IsNullOrWhiteSpace(ForthExe) && !legacyIsDefault)
            {
                Profiles.Insert(0, new ForthProfile { Name = "Mon interpréteur", Exe = ForthExe, Args = ForthArgs });
                ActiveProfile = "Mon interpréteur";
            }
        }
        if (CurrentProfile == null || !Profiles.Any(p => string.Equals(p.Name, ActiveProfile, StringComparison.OrdinalIgnoreCase)))
            ActiveProfile = Profiles[0].Name;
    }

    static string FilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ForthEditor", "settings.json");

    public static AppSettings Load()
    {
        AppSettings s;
        try { s = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(FilePath)) ?? new(); }
        catch { s = new(); }
        s.EnsureProfiles();
        return s;
    }

    public void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch { /* non bloquant */ }
    }
}
