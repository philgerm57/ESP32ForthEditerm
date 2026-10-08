using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;

namespace ForthEditor;

/// <summary>Logique de la fenêtre principale. L'interface (menus, onglets, barre d'état) est dans MainForm.Designer.cs.</summary>
public partial class MainForm : Form
{
    const int CloseSize = 16, MaxRecent = 10;
    const string FileFilter = "Fichiers Forth|*.fs;*.fth;*.4th;*.forth;*.f;*.fb|Tous les fichiers|*.*";
    static string SyntaxPath => Path.Combine(AppContext.BaseDirectory, "syntax.json");

    readonly Font _closeFont = new("Segoe UI Symbol", 8f);
    readonly string[] _args;
    readonly AppSettings _settings = AppSettings.Load();
    FindReplaceForm _findForm;
    SyntaxLibrary _library;
    SyntaxTheme _theme;
    List<string> _recent = new();
    int _untitled;
    bool _exiting;

    EditorTab Current => tabs.SelectedTab as EditorTab;

    public MainForm() : this(Array.Empty<string>()) { }

    public MainForm(string[] args)
    {
        InitializeComponent();
        _args = args;
        _recent = RecentFiles.Load();
        _library = SyntaxLibrary.Load(SyntaxPath, SyntaxFolder);
        ApplyThemeToAll(true);
        mnuRestoreSession.Checked = _settings.RestoreSession;
        UpdateThemeChecks();
        InitPanels();
        InitProject();
        RebuildRecentMenu();
    }

    // ================= Fenêtre =================
    void MainForm_Load(object sender, EventArgs e)
    {
        if (_settings.RestoreSession)
        {
            foreach (var p in _settings.Session) if (File.Exists(p)) OpenFile(p, false);
            var active = tabs.TabPages.Cast<EditorTab>().FirstOrDefault(t =>
                string.Equals(t.FilePath, _settings.ActiveFile, StringComparison.OrdinalIgnoreCase));
            if (active != null) tabs.SelectedTab = active;
        }
        foreach (var a in _args) if (File.Exists(a)) OpenFile(a);
        if (tabs.TabCount == 0) NewDoc();
    }

    void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        StopProcess();
        // La session est capturée avant la fermeture des onglets
        var session = tabs.TabPages.Cast<EditorTab>().Where(t => t.FilePath != null).Select(t => t.FilePath).ToList();
        string active = Current?.FilePath;
        _exiting = true;
        foreach (var t in tabs.TabPages.Cast<EditorTab>().ToList())
        {
            tabs.SelectedTab = t;
            if (!CloseTab(t)) { e.Cancel = true; _exiting = false; return; }
        }
        _settings.Session = session; _settings.ActiveFile = active; _settings.Save();
        DisposeProject();
    }

    // ================= Menu Fichier =================
    void mnuNew_Click(object sender, EventArgs e) => NewDoc();
    void mnuOpen_Click(object sender, EventArgs e) => OpenDialog();
    void mnuSave_Click(object sender, EventArgs e) { if (Current != null) Save(Current, false); }
    void mnuSaveAs_Click(object sender, EventArgs e) { if (Current != null) Save(Current, true); }
    void mnuPrint_Click(object sender, EventArgs e) { if (Current != null) Print(Current); }
    void mnuClose_Click(object sender, EventArgs e) { if (Current != null) CloseTab(Current); }
    void mnuDelete_Click(object sender, EventArgs e) { if (Current != null) DeleteFile(Current); }
    void mnuQuit_Click(object sender, EventArgs e) => Close();

    // ================= Menu Édition (partagé avec le menu contextuel) =================
    void Undo_Click(object sender, EventArgs e) => Current?.Editor.Box.Undo();
    void Redo_Click(object sender, EventArgs e) => Current?.Editor.Box.Redo();
    void Cut_Click(object sender, EventArgs e) => Current?.Editor.Box.Cut();
    void Copy_Click(object sender, EventArgs e) => Current?.Editor.Box.Copy();
    void Paste_Click(object sender, EventArgs e) => Current?.Editor.Box.PasteText();
    void SelectAll_Click(object sender, EventArgs e) => Current?.Editor.Box.SelectAll();
    void mnuFind_Click(object sender, EventArgs e) => ShowFind(false);
    void mnuFindNext_Click(object sender, EventArgs e) => FindNext(true);
    void mnuFindPrev_Click(object sender, EventArgs e) => FindNext(false);
    void mnuReplace_Click(object sender, EventArgs e) => ShowFind(true);

    // ================= Menu Affichage =================
    void mnuReloadSyntax_Click(object sender, EventArgs e) => ReloadSyntax();
    void mnuEditSyntax_Click(object sender, EventArgs e) => OpenFile(SyntaxPath);

    void mnuOpenSyntaxFolder_Click(object sender, EventArgs e)
    {
        try
        {
            Directory.CreateDirectory(SyntaxFolder);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("explorer.exe", $"\"{SyntaxFolder}\"") { UseShellExecute = true });
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Dossier des syntaxes", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
    void mnuThemeDark_Click(object sender, EventArgs e) => SetTheme("dark");
    void mnuThemeLight_Click(object sender, EventArgs e) => SetTheme("light");
    void mnuZoomIn_Click(object sender, EventArgs e) => Zoom(1);
    void mnuZoomOut_Click(object sender, EventArgs e) => Zoom(-1);
    void mnuZoomReset_Click(object sender, EventArgs e) => Zoom(0);
    void mnuRestoreSession_Click(object sender, EventArgs e) { _settings.RestoreSession = mnuRestoreSession.Checked; _settings.Save(); }
    void mnuGoTo_Click(object sender, EventArgs e) => GoToLine();
    //================== Menu aide =======================

    void mnuAfficherAide_Click(object sender, EventArgs e) => AfficherAide();
    void mnuApropos_Click(object sender, EventArgs e) => APropos();

    // ================= Glisser-déposer =================
    void MainForm_DragEnter(object sender, DragEventArgs e) =>
        e.Effect = e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;

    void MainForm_DragDrop(object sender, DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is string[] files) OpenFiles(files);
    }

    void OpenFiles(string[] files)
    {
        foreach (var f in files) if (File.Exists(f)) OpenFile(f);
    }

    void ctxEditor_Opening(object sender, CancelEventArgs e)
    {
        bool sel = Current?.Editor.Box.SelectionLength > 0;
        ctxCut.Enabled = ctxCopy.Enabled = sel;
        ctxPaste.Enabled = Clipboard.ContainsText();
    }

    // ================= Thème / syntaxe / zoom =================
    static string SyntaxFolder => SyntaxLibrary.FolderPath(AppContext.BaseDirectory);

    readonly Dictionary<SyntaxConfig, SyntaxTheme> _themeCache = new();

    /// <summary>Thème (couleurs et règles) d'une syntaxe, selon le thème clair/sombre et le zoom courants.</summary>
    SyntaxTheme GetTheme(SyntaxConfig cfg)
    {
        if (!_themeCache.TryGetValue(cfg, out var th))
        {
            th = cfg.Build(_settings.Theme, _settings.Zoom);
            _themeCache[cfg] = th;
        }
        return th;
    }

    /// <summary>Applique à un onglet la syntaxe correspondant à l'extension de son fichier.</summary>
    void ApplySyntaxToTab(EditorTab t, bool force = false)
    {
        var cfg = _library.For(t.FilePath);
        if (!force && ReferenceEquals(cfg, t.Syntax)) return;
        t.Syntax = cfg;
        t.Editor.ApplyTheme(GetTheme(cfg));
    }

    /// <summary>Recompile les thèmes (fichiers déjà chargés dans _library) et les applique à tous les onglets.</summary>
    void ApplyThemeToAll(bool showErrors)
    {
        _themeCache.Clear();
        _theme = GetTheme(_library.Default);          // thème de référence : sortie, panneaux, interface
        if (showErrors)
        {
            var errors = new List<string>(_library.Errors);
            foreach (var cfg in new[] { _library.Default }.Concat(_library.Others))
                errors.AddRange(GetTheme(cfg).Errors.Select(er => $"[{cfg.Name}] {er}"));
            if (errors.Count > 0)
                MessageBox.Show(string.Join("\n", errors), "Erreurs dans les fichiers de syntaxe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        foreach (EditorTab t in tabs.TabPages) ApplySyntaxToTab(t, true);
        ApplyOutputTheme();
    }

    void ReloadSyntax()
    {
        _library = SyntaxLibrary.Load(SyntaxPath, SyntaxFolder);
        ApplyThemeToAll(true);
    }

    static bool IsSyntaxFile(string path) =>
        string.Equals(path, SyntaxPath, StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith(SyntaxFolder + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);

    void SetTheme(string name)
    {
        _settings.Theme = name; _settings.Save();
        UpdateThemeChecks();
        ApplyThemeToAll(false);
    }

    void UpdateThemeChecks()
    {
        mnuThemeDark.Checked = _settings.Theme != "light";
        mnuThemeLight.Checked = _settings.Theme == "light";
    }

    void Zoom(int steps)
    {
        _settings.Zoom = steps == 0 ? 0 : Math.Clamp(_settings.Zoom + steps, -5f, 40f);
        _settings.Save();
        ApplyThemeToAll(false);
    }

    // ================= Fichiers récents =================
    void AddRecent(string path)
    {
        _recent.RemoveAll(p => string.Equals(p, path, StringComparison.OrdinalIgnoreCase));
        _recent.Insert(0, path);
        if (_recent.Count > MaxRecent) _recent.RemoveRange(MaxRecent, _recent.Count - MaxRecent);
        RecentFiles.Save(_recent); RebuildRecentMenu();
    }

    void RemoveRecent(string path)
    {
        _recent.RemoveAll(p => string.Equals(p, path, StringComparison.OrdinalIgnoreCase));
        RecentFiles.Save(_recent); RebuildRecentMenu();
    }

    void RebuildRecentMenu()
    {
        mnuRecent.DropDownItems.Clear();
        if (_recent.Count == 0) { mnuRecent.DropDownItems.Add(new ToolStripMenuItem("(vide)") { Enabled = false }); return; }
        for (int i = 0; i < _recent.Count; i++)
        {
            string p = _recent[i];
            mnuRecent.DropDownItems.Add(new ToolStripMenuItem($"&{i + 1}  {p}", null, (s, e) =>
            {
                if (File.Exists(p)) OpenFile(p);
                else { MessageBox.Show("Fichier introuvable :\n" + p); RemoveRecent(p); }
            }));
        }
        mnuRecent.DropDownItems.Add(new ToolStripSeparator());
        mnuRecent.DropDownItems.Add(new ToolStripMenuItem("Effacer la liste", null, (s, e) =>
        { _recent.Clear(); RecentFiles.Save(_recent); RebuildRecentMenu(); }));
    }

    // ================= Onglets / fichiers =================
    EditorTab AddTab(string title, string path, string content, bool crlf)
    {
        var t = new EditorTab { Title = title, FilePath = path };
        t.Editor.UseCrLf = crlf;
        t.Editor.AutoCompleteEnabled = _settings.AutoComplete;
        t.Editor.Box.ContextMenuStrip = ctxEditor;
        ApplySyntaxToTab(t, true);
        t.Editor.LoadText(content);
        t.Editor.ContentChanged += (s, e) =>
        {
            if (!t.Modified) { t.Modified = true; t.RefreshTitle(); tabs.Invalidate(); }
        };
        t.RefreshTitle();
        tabs.TabPages.Add(t); tabs.SelectedTab = t;
        t.Editor.Box.SelectionChanged += (s, e) => { if (t == Current) UpdateStatus(); };
        t.Editor.ZoomRequested += Zoom;
        t.Editor.FilesDropped += OpenFiles;
        t.Editor.StructureChanged += () => { if (t == Current) RefreshOutline(); };
        t.Editor.Box.Focus();
        UpdateStatus();
        RefreshOutline();
        return t;
    }

    void NewDoc() => AddTab($"sans titre {++_untitled}.fs", null, "", true);

    void OpenDialog()
    {
        using var dlg = new OpenFileDialog { Filter = FileFilter, Multiselect = true };
        if (dlg.ShowDialog(this) == DialogResult.OK) foreach (var f in dlg.FileNames) OpenFile(f);
    }

    void OpenFile(string path, bool addToRecent = true)
    {
        path = Path.GetFullPath(path);
        var existing = tabs.TabPages.Cast<EditorTab>()
            .FirstOrDefault(t => string.Equals(t.FilePath, path, StringComparison.OrdinalIgnoreCase));
        if (existing != null) { tabs.SelectedTab = existing; return; }
        try
        {
            string text = File.ReadAllText(path);
            AddTab(Path.GetFileName(path), path, text.Replace("\r\n", "\n"), text.Contains("\r\n") || !text.Contains('\n'));
            if (addToRecent) AddRecent(path);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Ouverture impossible", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    bool Save(EditorTab t, bool saveAs)
    {
        string path = t.FilePath;
        if (path == null || saveAs)
        {
            using var dlg = new SaveFileDialog
            {
                Filter = FileFilter, DefaultExt = "fs", FileName = t.Title,
                InitialDirectory = path != null ? Path.GetDirectoryName(path) : ""
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return false;
            path = dlg.FileName;
        }
        try { File.WriteAllText(path, t.Editor.GetTextForFile(), new System.Text.UTF8Encoding(false)); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Échec de l'enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }

        t.FilePath = path; t.Title = Path.GetFileName(path); t.Modified = false;
        t.RefreshTitle(); tabs.Invalidate();
        AddRecent(path);
        if (IsSyntaxFile(path)) ReloadSyntax(); else ApplySyntaxToTab(t);
        return true;
    }

    /// <summary>Ferme l'onglet (propose d'enregistrer). Retourne false si l'utilisateur annule.</summary>
    bool CloseTab(EditorTab t, bool force = false)
    {
        if (!force && t.Modified)
        {
            tabs.SelectedTab = t;
            var r = MessageBox.Show($"Enregistrer les modifications de « {t.Title} » ?", "Fermer",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (r == DialogResult.Cancel) return false;
            if (r == DialogResult.Yes && !Save(t, false)) return false;
        }
        tabs.TabPages.Remove(t); t.Dispose();
        if (tabs.TabCount == 0 && !_exiting) NewDoc();
        return true;
    }

    void DeleteFile(EditorTab t)
    {
        string msg = t.FilePath != null
            ? $"Détruire DÉFINITIVEMENT le fichier :\n{t.FilePath}\n\net fermer cet onglet ?"
            : "Ce document n'a jamais été enregistré. Fermer l'onglet sans enregistrer ?";
        if (MessageBox.Show(msg, "Détruire", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        if (t.FilePath != null)
        {
            try { File.Delete(t.FilePath); RemoveRecent(t.FilePath); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Suppression impossible", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
        }
        CloseTab(t, force: true);
    }

    void Rename(EditorTab t)
    {
        using var dlg = new PromptForm("Renommer", "Nouveau titre du fichier :", t.Title);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        string n = dlg.Value.Trim();
        if (string.IsNullOrEmpty(n) || n == t.Title) return;
        if (n.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) { MessageBox.Show("Nom invalide."); return; }
        if (t.FilePath != null)
        {
            string np = Path.Combine(Path.GetDirectoryName(t.FilePath)!, n);
            if (File.Exists(np)) { MessageBox.Show("Un fichier porte déjà ce nom."); return; }
            try { File.Move(t.FilePath, np); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Renommage impossible", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            RemoveRecent(t.FilePath); t.FilePath = np; AddRecent(np);
        }
        t.Title = n; t.RefreshTitle(); tabs.Invalidate();
        ApplySyntaxToTab(t);
    }

    // ================= Dessin des onglets (titre + croix) =================
    static Rectangle CloseRect(Rectangle r) =>
        new(r.Right - CloseSize - 6, r.Top + (r.Height - CloseSize) / 2, CloseSize, CloseSize);

    void tabs_DrawItem(object sender, DrawItemEventArgs e)
    {
        var page = tabs.TabPages[e.Index];
        var r = tabs.GetTabRect(e.Index);
        bool sel = e.Index == tabs.SelectedIndex;
        using (var b = new SolidBrush(sel ? SystemColors.Window : SystemColors.ControlLight))
            e.Graphics.FillRectangle(b, r);
        TextRenderer.DrawText(e.Graphics, page.Text, Font,
            new Rectangle(r.X + 6, r.Y, r.Width - CloseSize - 14, r.Height), SystemColors.ControlText,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        TextRenderer.DrawText(e.Graphics, "✕", _closeFont, CloseRect(r), Color.Gray,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    void tabs_MouseDown(object sender, MouseEventArgs e)
    {
        for (int i = 0; i < tabs.TabCount; i++)
        {
            var r = tabs.GetTabRect(i);
            if (!r.Contains(e.Location)) continue;
            if ((e.Button == MouseButtons.Left && CloseRect(r).Contains(e.Location)) || e.Button == MouseButtons.Middle)
                CloseTab((EditorTab)tabs.TabPages[i]);
            return;
        }
    }

    void tabs_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        for (int i = 0; i < tabs.TabCount; i++)
        {
            var r = tabs.GetTabRect(i);
            if (r.Contains(e.Location) && !CloseRect(r).Contains(e.Location)) { Rename((EditorTab)tabs.TabPages[i]); return; }
        }
    }

    void tabs_SelectedIndexChanged(object sender, EventArgs e) { UpdateStatus(); RefreshOutline(); }

    // ================= Barre d'état =================
    void UpdateStatus()
    {
        var t = Current;
        if (t == null) { lblPos.Text = lblSel.Text = lblLines.Text = lblEol.Text = ""; return; }
        var box = t.Editor.Box;
        int idx = box.SelectionStart;
        int line = t.Editor.LineFromIndex(idx);
        int col = idx - t.Editor.LineStart(line) + 1;
        lblPos.Text = $"Ln {line + 1}, Col {col}";
        lblSel.Text = box.SelectionLength > 0 ? $"Sél. : {box.SelectionLength}" : "";
        lblLines.Text = $"{t.Editor.LineCount} lignes";
        lblEol.Text = (t.Editor.UseCrLf ? "CRLF" : "LF") + "  UTF-8";
    }

    // ================= Recherche / remplacement =================
    FindReplaceForm EnsureFind() => _findForm ??= new FindReplaceForm(() => Current?.Editor.Box);

    void ShowFind(bool replace) { if (Current != null) EnsureFind().ShowFind(this, replace); }

    void FindNext(bool forward)
    {
        if (Current == null) return;
        var f = EnsureFind();
        if (!f.HasSearchText) f.ShowFind(this, false); else f.FindNext(forward);
    }

    //==================== Menu Aide =====================

    void AfficherAide()
    {

    }

    void APropos()
    {
        AproposForm Ap = new AproposForm();
        Ap.ShowDialog();

    }

    // ================= Aller à la ligne =================
    void GoToLine()
    {
        var t = Current;
        if (t == null) return;
        var box = t.Editor.Box;
        int total = t.Editor.LineCount;
        int cur = t.Editor.LineFromIndex(box.SelectionStart) + 1;
        using var dlg = new PromptForm("Aller à la ligne", $"Numéro de ligne (1 – {total}) :", cur.ToString());
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        if (!int.TryParse(dlg.Value.Trim(), out int n)) { MessageBox.Show("Numéro de ligne invalide."); return; }
        t.Editor.GoToLine(Math.Clamp(n, 1, total) - 1);
    }

    // ================= Impression =================
    void Print(EditorTab t)
    {
        string text = t.Editor.Box.Text.Replace("\t", "    ");
        int pos = 0;
        using var font = new Font("Consolas", 10f);
        using var doc = new PrintDocument { DocumentName = t.Title };
        doc.PrintPage += (s, e) =>
        {
            var rect = e.MarginBounds;
            rect.Height -= rect.Height % font.Height; // pas de ligne coupée en bas de page
            using var fmt = new StringFormat(StringFormatFlags.LineLimit);
            e.Graphics.MeasureString(text.Substring(pos), font, rect.Size, fmt, out int chars, out _);
            e.Graphics.DrawString(text.Substring(pos, chars), font, Brushes.Black, rect, fmt);
            pos += chars;
            e.HasMorePages = pos < text.Length;
        };
        using var dlg = new PrintDialog { Document = doc, UseEXDialog = true };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try { doc.Print(); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Impression", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
}
