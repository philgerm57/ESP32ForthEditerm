using System.ComponentModel;

namespace ForthEditor;

/// <summary>Un onglet = un document (créé dynamiquement, donc pas de concepteur).</summary>
[DesignerCategory("Code")]
public class EditorTab : TabPage
{
    public ForthEditorControl Editor { get; } = new() { Dock = DockStyle.Fill };
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string FilePath { get; set; }
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Title { get; set; } = "";
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Modified { get; set; }

    /// <summary>Syntaxe (coloration) actuellement appliquée, choisie d'après l'extension du fichier.</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SyntaxConfig Syntax { get; set; }

    public EditorTab() { Controls.Add(Editor); }
    public void RefreshTitle() { Text = Title + (Modified ? " *" : ""); ToolTipText = FilePath ?? "(non enregistré)"; }
}
