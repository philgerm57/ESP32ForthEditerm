// ForthEditor — éditeur de code Forth et terminal série pour ESP32Forth
// Copyright (C) 2026 Germain PHILIPPE
// Licence : GNU General Public License v3.0 ou ultérieure — voir le fichier LICENSE.

using System.Reflection;

namespace ForthEditor;

/// <summary>Informations sur l'application. Le numéro de version vient de &lt;Version&gt; dans ForthEditor.csproj.</summary>
public static class AppInfo
{
    /// <summary>Version « 2.4 » (ou « 2.4.1 » si le 3e nombre n'est pas nul).</summary>
    public static string Version
    {
        get
        {
            var v = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0);
            return v.Build > 0 ? $"{v.Major}.{v.Minor}.{v.Build}" : $"{v.Major}.{v.Minor}";
        }
    }
}
