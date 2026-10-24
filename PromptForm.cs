// ForthEditor — éditeur de code Forth et terminal série pour ESP32Forth
// Copyright (C) 2026 Germain PHILIPPE
// Licence : GNU General Public License v3.0 ou ultérieure — voir le fichier LICENSE.

namespace ForthEditor;

/// <summary>Petite boîte de saisie (utilisée pour renommer un fichier).</summary>
public partial class PromptForm : Form
{
    public PromptForm() { InitializeComponent(); }

    public PromptForm(string title, string label, string value) : this()
    {
        Text = title; lblPrompt.Text = label; txtValue.Text = value;
    }

    public string Value => txtValue.Text;
}
