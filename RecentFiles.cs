// ForthEditor — éditeur de code Forth et terminal série pour ESP32Forth
// Copyright (C) 2026 Germain PHILIPPE
// Licence : GNU General Public License v3.0 ou ultérieure — voir le fichier LICENSE.

using System.Text.Json;

namespace ForthEditor;

/// <summary>Liste des derniers fichiers ouverts, stockée dans %AppData%\ForthEditor\recent.json</summary>
static class RecentFiles
{
    static string FilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ForthEditor", "recent.json");

    public static List<string> Load()
    {
        try { return JsonSerializer.Deserialize<List<string>>(File.ReadAllText(FilePath)) ?? new(); }
        catch { return new(); }
    }

    public static void Save(List<string> list)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(list));
        }
        catch { /* non bloquant */ }
    }
}
