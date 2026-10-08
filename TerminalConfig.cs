using System.Text.Json;

namespace ForthEditor;

/// <summary>Paramètres du terminal série : %AppData%\ForthEditor\terminal.json</summary>
public class TerminalConfig
{
    public string PortName { get; set; } = "COM1";
    public int BaudRate { get; set; } = 9600;
    public int DataBits { get; set; } = 8;
    public string Parity { get; set; } = "None";
    public string StopBits { get; set; } = "One";
    public string BackColorHex { get; set; } = "#000000";
    public string ForeColorHex { get; set; } = "#32CD32";

    /// <summary>"ASCII" | "Latin1" | "UTF8"</summary>
    public string EncodingName { get; set; } = "ASCII";
    /// <summary>Fin de ligne émise : "CRLF" | "CR" | "LF"</summary>
    public string EolMode { get; set; } = "CRLF";

    /// <summary>Envoi de l'éditeur : "Delay" (pause fixe) | "WaitPrompt" (attend l'invite de la cible, ex. « ok »).</summary>
    public string SendMode { get; set; } = "Delay";
    public int LineDelayMs { get; set; } = 30;
    public string Prompt { get; set; } = "ok";
    public int PromptTimeoutMs { get; set; } = 3000;
    /// <summary>Ne pas envoyer les lignes vides ni les lignes de commentaire « \ ».</summary>
    public bool SkipComments { get; set; } = true;

    /// <summary>Masque le texte que la cible renvoie en écho après chaque ligne envoyée.</summary>
    public bool SuppressEcho { get; set; }

    /// <summary>Copie de travail (utilisée par la fenêtre de paramètres : annuler = ne rien modifier).</summary>
    public TerminalConfig Clone() => (TerminalConfig)MemberwiseClone();

    static string ConfigPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ForthEditor", "terminal.json");

    public static TerminalConfig Load()
    {
        try
        {
            if (File.Exists(ConfigPath))
                return JsonSerializer.Deserialize<TerminalConfig>(File.ReadAllText(ConfigPath)) ?? new TerminalConfig();
        }
        catch { /* fichier illisible : valeurs par défaut */ }
        return new TerminalConfig();
    }

    public void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
            File.WriteAllText(ConfigPath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch { /* écriture impossible : on ignore */ }
    }
}
