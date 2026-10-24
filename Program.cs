// ForthEditor — éditeur de code Forth et terminal série pour ESP32Forth
// Copyright (C) 2026 Germain PHILIPPE
// Licence : GNU General Public License v3.0 ou ultérieure — voir le fichier LICENSE.

namespace ForthEditor;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm(args));
    }
}
