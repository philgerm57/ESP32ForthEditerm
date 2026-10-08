namespace ForthEditor;

// Partie de MainForm : ouverture du terminal série (menu Outils).
public partial class MainForm
{
    TerminalForm _terminal;

    void mnuTerminal_Click(object sender, EventArgs e)
    {
        if (_terminal == null || _terminal.IsDisposed)
        {
            _terminal = new TerminalForm { GetEditorText = GetTextToSend };
            _terminal.Show(this);              // non modale : l'éditeur reste utilisable
        }
        else
        {
            if (_terminal.WindowState == FormWindowState.Minimized) _terminal.WindowState = FormWindowState.Normal;
            _terminal.Activate();
        }
    }

    /// <summary>Texte envoyé au port série : la sélection de l'onglet courant, sinon tout le fichier.</summary>
    string GetTextToSend()
    {
        var t = Current;
        if (t == null) return "";
        var box = t.Editor.Box;
        return box.SelectionLength > 0 ? box.SelectedText : t.Editor.GetTextForFile();
    }
}
