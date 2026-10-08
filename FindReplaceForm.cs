using System.Text.RegularExpressions;

namespace ForthEditor;

/// <summary>Recherche / remplacement non modal, appliqué à l'onglet courant. Interface : FindReplaceForm.Designer.cs</summary>
public partial class FindReplaceForm : Form
{
    Func<CodeBox> _getBox;

    public bool HasSearchText => txtFind.Text.Length > 0;

    public FindReplaceForm() { InitializeComponent(); }
    public FindReplaceForm(Func<CodeBox> getBox) : this() { _getBox = getBox; }

    // ---- événements (branchés dans le concepteur) ----
    void btnNext_Click(object sender, EventArgs e) => FindNext(true);
    void btnPrev_Click(object sender, EventArgs e) => FindNext(false);
    void btnReplace_Click(object sender, EventArgs e) => ReplaceOne();
    void btnReplaceAll_Click(object sender, EventArgs e) => ReplaceAll();
    void btnClose_Click(object sender, EventArgs e) => Hide();
    void FindReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing) { e.Cancel = true; Hide(); }
    }

    // ---- logique ----
    public void ShowFind(IWin32Window owner, bool focusReplace)
    {
        var box = _getBox?.Invoke();
        if (box != null && box.SelectionLength > 0 && !box.SelectedText.Contains('\n')) txtFind.Text = box.SelectedText;
        if (!Visible) Show(owner); else Activate();
        var target = focusReplace ? txtReplace : txtFind;
        target.Focus(); target.SelectAll();
    }

    Regex BuildRegex(bool rightToLeft)
    {
        string p = txtFind.Text;
        if (p.Length == 0) { lblInfo.Text = "Saisissez un texte à rechercher."; return null; }
        if (!chkRegex.Checked) p = Regex.Escape(p);
        if (chkWord.Checked) p = @"(?<!\S)(?:" + p + @")(?!\S)";
        var o = RegexOptions.Multiline | RegexOptions.CultureInvariant;
        if (!chkCase.Checked) o |= RegexOptions.IgnoreCase;
        if (rightToLeft) o |= RegexOptions.RightToLeft;
        try { return new Regex(p, o); }
        catch (ArgumentException ex) { lblInfo.Text = "Expression invalide : " + ex.Message; return null; }
    }

    public bool FindNext(bool forward)
    {
        var box = _getBox?.Invoke();
        var re = BuildRegex(!forward);
        if (box == null || re == null) return false;

        string text = box.Text;
        bool wrapped = false;
        Match m;
        if (forward)
        {
            m = re.Match(text, Math.Min(box.SelectionStart + box.SelectionLength, text.Length));
            if (!m.Success) { m = re.Match(text, 0); wrapped = true; }
        }
        else
        {
            m = re.Match(text, box.SelectionStart);
            if (!m.Success) { m = re.Match(text, text.Length); wrapped = true; }
        }
        if (!m.Success || m.Length == 0) { lblInfo.Text = "Aucune occurrence."; return false; }

        box.Select(m.Index, m.Length);
        box.ScrollToCaret();
        lblInfo.Text = wrapped ? (forward ? "Reprise depuis le début du fichier." : "Reprise depuis la fin du fichier.") : "";
        return true;
    }

    void ReplaceOne()
    {
        var box = _getBox?.Invoke();
        var re = BuildRegex(false);
        if (box == null || re == null) return;
        if (box.SelectionLength > 0)
        {
            var m = re.Match(box.Text, box.SelectionStart);
            if (m.Success && m.Index == box.SelectionStart && m.Length == box.SelectionLength)
                box.SelectedText = chkRegex.Checked ? m.Result(txtReplace.Text) : txtReplace.Text;
        }
        FindNext(true);
    }

    void ReplaceAll()
    {
        var box = _getBox?.Invoke();
        var re = BuildRegex(false);
        if (box == null || re == null) return;
        string text = box.Text;
        int n = re.Matches(text).Count;
        if (n == 0) { lblInfo.Text = "Aucune occurrence."; return; }
        string repl = chkRegex.Checked ? txtReplace.Text : txtReplace.Text.Replace("$", "$$");
        box.SelectAll();
        box.SelectedText = re.Replace(text, repl);   // une seule action d'annulation
        box.Select(0, 0); box.ScrollToCaret();
        lblInfo.Text = $"{n} remplacement(s) effectué(s).";
    }
}
