// ForthEditor — éditeur de code Forth et terminal série pour ESP32Forth
// Copyright (C) 2026 Germain PHILIPPE
// Licence : GNU General Public License v3.0 ou ultérieure — voir le fichier LICENSE.

using Microsoft.Web.WebView2.Core;

namespace ForthEditor;

/// <summary>Visionneuse de documentation PDF (WebView2 / visionneuse PDF intégrée d'Edge).</summary>
public partial class PdfViewerForm : Form
{
    readonly AppSettings _settings;
    string _folder;
    string _pendingFile;
    bool _ready;
    string _current;                       // PDF affiché (dont on suit la page)
    DateTime _pollAfter = DateTime.MaxValue;
    bool _suppressEvents;
    bool _closing, _closeReady;
    /// <summary>Action à exécuter une fois la fenêtre réellement fermée (ex. fermeture de l'application).</summary>
    public Action AfterClose;

    public PdfViewerForm(AppSettings settings)
    {
        _settings = settings;
        InitializeComponent();
        Load += async (s, e) => { RestoreLayout(); await InitWebViewAsync(); };
        FormClosing += OnViewerClosing;
        FormClosed += (s, e) => { var a = AfterClose; if (a != null) { try { Owner?.BeginInvoke(a); } catch { } } };
        cboRecent.SelectedIndexChanged += (s, e) => { if (!_suppressEvents && cboRecent.SelectedItem is PdfItem it) ShowPdf(it.Path); };
        btnZoomOut.Click += (s, e) => SetZoom(web.ZoomFactor - 0.1);
        btnZoomIn.Click += (s, e) => SetZoom(web.ZoomFactor + 0.1);
        btnZoom100.Click += (s, e) => SetZoom(1.0);
        lstPdf.SelectedIndexChanged += (s, e) => { if (!_suppressEvents && lstPdf.SelectedItem is PdfItem it) ShowPdf(it.Path); };
        btnFolder.Click += (s, e) => ChooseFolder();
        btnOpenFile.Click += (s, e) => OpenPdfFile();
        btnExplorer.Click += (s, e) => OpenInExplorer();
        SetFolder(DefaultFolder, false);
        RefreshRecent();
    }

    sealed class PdfItem
    {
        public string Path, Label;
        public override string ToString() => Label;
    }

    void SetZoom(double z)
    {
        web.ZoomFactor = Math.Clamp(Math.Round(z, 2), 0.25, 5.0);
        UpdateZoomLabel();
    }

    void UpdateZoomLabel() => btnZoom100.Text = (int)Math.Round(web.ZoomFactor * 100) + " %";

    void RestoreLayout()
    {
        try
        {
            var b = _settings.PdfBounds;
            if (b != null && b.Length == 4 && b[2] > 300 && b[3] > 200)
            {
                var r = new Rectangle(b[0], b[1], b[2], b[3]);
                if (Screen.AllScreens.Any(sc => sc.WorkingArea.IntersectsWith(r)))
                {
                    StartPosition = FormStartPosition.Manual;
                    Bounds = r;
                }
            }
            if (_settings.PdfMaximized) WindowState = FormWindowState.Maximized;
            if (_settings.PdfSplit > 0) split.SplitterDistance = Math.Clamp(_settings.PdfSplit, split.Panel1MinSize, Math.Max(split.Panel1MinSize, split.Width - split.Panel2MinSize - split.SplitterWidth));
        }
        catch { }
    }

    void SaveLayout()
    {
        try
        {
            var r = WindowState == FormWindowState.Normal ? Bounds : RestoreBounds;
            _settings.PdfBounds = new[] { r.X, r.Y, r.Width, r.Height };
            _settings.PdfMaximized = WindowState == FormWindowState.Maximized;
            _settings.PdfSplit = split.SplitterDistance;
            if (_ready) _settings.PdfZoom = web.ZoomFactor;
            _settings.Save();
        }
        catch { }
    }

    /// <summary>Dossier « doc » installé à côté de l'application.</summary>
    public static string DefaultFolder => Path.Combine(AppContext.BaseDirectory, "doc");

    string CurrentFolder =>
        !string.IsNullOrWhiteSpace(_settings.PdfFolder) && Directory.Exists(_settings.PdfFolder) ? _settings.PdfFolder : DefaultFolder;

    async Task InitWebViewAsync()
    {
        try
        {
            string data = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ForthEditor", "WebView2");
            var env = await CoreWebView2Environment.CreateAsync(null, data);
            await web.EnsureCoreWebView2Async(env);
            web.CoreWebView2.NewWindowRequested += (s, e) =>
            {
                e.Handled = true;      // liens externes : navigateur par défaut
                try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri) { UseShellExecute = true }); } catch { }
            };
            _ready = true;
            web.ZoomFactor = Math.Clamp(_settings.PdfZoom <= 0 ? 1.0 : _settings.PdfZoom, 0.25, 5.0);
            UpdateZoomLabel();
            // Visionneuse PDF.js (dossier « pdfjs » de l'application) : https://pdfjs.local/
            web.CoreWebView2.SetVirtualHostNameToFolderMapping("pdfjs.local", Path.Combine(AppContext.BaseDirectory, "pdfjs"),
                CoreWebView2HostResourceAccessKind.Allow);
            var last = _settings.PdfRecents.FirstOrDefault(r => File.Exists(r.Path));
            if (_pendingFile != null) ShowPdf(_pendingFile);
            else if (last != null) ShowPdf(last.Path);                 // reprend le dernier PDF, à sa page
            else if (lstPdf.Items.Count > 0) lstPdf.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            lblInfo.Text = "WebView2 indisponible.";
            MessageBox.Show(this, "Impossible d'initialiser WebView2 (le runtime Microsoft Edge WebView2 est-il installé ?).\n\n" + ex.Message,
                "Aide PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>Change de dossier et relit la liste des PDF (sous-dossiers inclus).</summary>
    public void SetFolder(string folder, bool save)
    {
        _folder = folder;
        if (save) _settings.PdfFolder = string.Equals(Path.GetFullPath(folder), Path.GetFullPath(DefaultFolder), StringComparison.OrdinalIgnoreCase) ? null : folder;
        if (save) _settings.Save();
        txtFolder.Text = folder;
        lstPdf.Items.Clear();
        try
        {
            if (!Directory.Exists(folder) && string.Equals(folder, DefaultFolder, StringComparison.OrdinalIgnoreCase)) Directory.CreateDirectory(folder);
            if (Directory.Exists(folder))
                foreach (var f in Directory.EnumerateFiles(folder, "*.pdf", SearchOption.AllDirectories).OrderBy(x => x, StringComparer.OrdinalIgnoreCase))
                    lstPdf.Items.Add(new PdfItem { Path = f, Label = Path.GetRelativePath(folder, f) });
        }
        catch (Exception ex) { lblInfo.Text = ex.Message; }
        lblInfo.Text = lstPdf.Items.Count == 0 ? "Aucun PDF dans ce dossier." : lstPdf.Items.Count + " document(s)";
    }

    public void ChooseFolder()
    {
        using var dlg = new FolderBrowserDialog { Description = "Dossier contenant les fichiers PDF", SelectedPath = CurrentFolder, UseDescriptionForTitle = true };
        if (dlg.ShowDialog(this) == DialogResult.OK) SetFolder(dlg.SelectedPath, true);
    }

    public void OpenPdfFile()
    {
        using var dlg = new OpenFileDialog { Title = "Ouvrir un fichier PDF", Filter = "Fichiers PDF (*.pdf)|*.pdf|Tous les fichiers|*.*", InitialDirectory = CurrentFolder };
        if (dlg.ShowDialog(this) == DialogResult.OK) ShowPdf(dlg.FileName);
    }

    void OpenInExplorer()
    {
        try
        {
            string f = CurrentFolder;
            Directory.CreateDirectory(f);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(f) { UseShellExecute = true });
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Dossier", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    public async void ShowPdf(string path)
    {
        if (!File.Exists(path)) { lblInfo.Text = "Fichier introuvable : " + path; return; }
        try { await PollPageAsync(); } catch { }          // mémorise la page du PDF qu'on quitte
        _current = null;
        _pendingFile = path;
        Text = "Documentation — " + Path.GetFileName(path);
        var old = _settings.PdfRecents.FirstOrDefault(r => SamePath(r.Path, path));
        int page = old?.Page ?? 1;
        string zoom = old?.Zoom;
        TouchRecent(path, page, zoom);
        RefreshRecent();
        SelectInList(path);
        if (!_ready) return;                              // affiché dès que WebView2 est prêt

        // Le PDF est servi sous https://pdfdata.local/ (son dossier), la visionneuse sous https://pdfjs.local/
        string dir = Path.GetDirectoryName(Path.GetFullPath(path));
        web.CoreWebView2.ClearVirtualHostNameToFolderMapping("pdfdata.local");
        web.CoreWebView2.SetVirtualHostNameToFolderMapping("pdfdata.local", dir, CoreWebView2HostResourceAccessKind.Allow);
        string file = "https://pdfdata.local/" + Uri.EscapeDataString(Path.GetFileName(path));
        string hash = "#page=" + Math.Max(1, page);
        string z = ToHashZoom(zoom);
        if (z != null) hash += "&zoom=" + z;
        _pollAfter = DateTime.Now.AddSeconds(2);          // laisse le temps au PDF de se positionner
        _current = path;
        web.CoreWebView2.Navigate("https://pdfjs.local/web/viewer.html?file=" + Uri.EscapeDataString(file) + hash);
    }

    /// <summary>Convertit le zoom mémorisé (facteur 1.25 ou nom) en valeur du paramètre « zoom » de PDF.js (pourcentage ou nom).</summary>
    static string ToHashZoom(string z)
    {
        if (string.IsNullOrWhiteSpace(z)) return null;
        if (double.TryParse(z, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double f) && f > 0)
            return Math.Round(f * 100).ToString(System.Globalization.CultureInfo.InvariantCulture);
        return z;
    }

    // ---------- Derniers PDF + page courante ----------
    static bool SamePath(string a, string b) => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

    /// <summary>Place le PDF en tête de liste ; au-delà du maximum, le plus ancien disparaît (rotation).</summary>
    void TouchRecent(string path, int page, string zoom)
    {
        var list = _settings.PdfRecents ??= new();
        list.RemoveAll(r => SamePath(r.Path, path));
        list.Insert(0, new PdfRecent { Path = path, Page = Math.Max(1, page), Zoom = zoom });
        int max = Math.Clamp(_settings.PdfRecentMax <= 0 ? 10 : _settings.PdfRecentMax, 1, 50);
        while (list.Count > max) list.RemoveAt(list.Count - 1);
    }

    void RefreshRecent()
    {
        _suppressEvents = true;
        try
        {
            cboRecent.Items.Clear();
            foreach (var r in _settings.PdfRecents)
                cboRecent.Items.Add(new PdfItem { Path = r.Path, Label = Path.GetFileName(r.Path) + "  (p. " + r.Page + ")" });
            if (cboRecent.Items.Count > 0) cboRecent.SelectedIndex = 0;
        }
        finally { _suppressEvents = false; }
    }

    void SelectInList(string path)
    {
        _suppressEvents = true;
        try
        {
            lstPdf.SelectedIndex = -1;
            for (int i = 0; i < lstPdf.Items.Count; i++)
                if (lstPdf.Items[i] is PdfItem it && SamePath(it.Path, path)) { lstPdf.SelectedIndex = i; break; }
        }
        finally { _suppressEvents = false; }
    }

    // ---------- Lecture de la page et du zoom courants (PDF.js) ----------
    const string PageJs = "(function(){try{var a=window.PDFViewerApplication;return (a&&a.pdfViewer&&a.pagesCount)?(a.page+'|'+a.pdfViewer.currentScaleValue):null;}catch(e){return null;}})()";

    /// <summary>Relit la page (et le zoom) du PDF affiché et les mémorise.</summary>
    async Task PollPageAsync(bool log = false)
    {
        if (_current == null || !_ready || web.CoreWebView2 == null || DateTime.Now < _pollAfter) return;
        string cur = _current;
        string r = null;
        try
        {
            r = (await web.CoreWebView2.ExecuteScriptAsync(PageJs) ?? "").Trim('"');
            var parts = r.Split('|');
            var rec = _settings.PdfRecents.FirstOrDefault(x => SamePath(x.Path, cur));
            if (rec != null && parts.Length == 2 && int.TryParse(parts[0], out int n) && n > 0)
            {
                rec.Page = n;
                rec.Zoom = parts[1];
            }
            else if (log) WriteLog(cur, r);
        }
        catch (Exception ex) { if (log) WriteLog(cur, "erreur : " + ex.Message); }
    }

    static void WriteLog(string file, string result)
    {
        try
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ForthEditor");
            Directory.CreateDirectory(dir);
            File.AppendAllText(Path.Combine(dir, "pdfpage.log"), DateTime.Now + "  " + file + "  résultat=" + result + Environment.NewLine);
        }
        catch { }
    }

    // ---------- Fermeture : on lit la page avant de fermer ----------
    void OnViewerClosing(object sender, FormClosingEventArgs e)
    {
        if (_closeReady || !_ready || _current == null) { SaveLayout(); return; }
        e.Cancel = true;                       // on diffère la fermeture le temps de lire la page (asynchrone)
        if (_closing) return;
        _closing = true;
        _ = FinishCloseAsync();
    }

    async Task FinishCloseAsync()
    {
        try
        {
            _pollAfter = DateTime.MinValue;    // lecture immédiate, même si le PDF vient juste d'être ouvert
            await Task.WhenAny(PollPageAsync(true), Task.Delay(2500));
        }
        catch { }
        _closeReady = true;
        Close();
    }
}
