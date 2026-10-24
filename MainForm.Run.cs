// ForthEditor — éditeur de code Forth et terminal série pour ESP32Forth
// Copyright (C) 2026 Germain PHILIPPE
// Licence : GNU General Public License v3.0 ou ultérieure — voir le fichier LICENSE.

using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace ForthEditor;

// Partie de MainForm dédiée : plan des définitions, repliement, panneaux, exécution d'un interpréteur Forth externe.
public partial class MainForm
{
    Process _proc;
    EditorTab _runTab;
    string _runFile = "", _runDir = "";
    bool _runIsTemp;
    string _outlineKey = "";

    static readonly Regex AnsiRx = new(@"\x1B\[[0-9;?]*[ -/]*[@-~]", RegexOptions.Compiled);
    static readonly Regex ErrLocRx = new(@"(?<file>(?:[A-Za-z]:)?[^:\r\n""<>|?*]+?\.\w+):(?<line>\d+)", RegexOptions.Compiled);

    // ================= Initialisation des panneaux =================
    void InitPanels()
    {
        splitInner.Panel2Collapsed = !_settings.ShowOutline;
        splitOuter.Panel2Collapsed = !_settings.ShowOutput;
        mnuShowOutline.Checked = _settings.ShowOutline;
        mnuShowOutput.Checked = _settings.ShowOutput;
        mnuAutoComplete.Checked = _settings.AutoComplete;
        chkOutlineDefsOnly.Checked = _settings.OutlineDefsOnly;
        UpdateRunButtons();
    }

    void ApplyOutputTheme()
    {
        rtbOutput.BackColor = txtStdin.BackColor = _theme.Background;
        rtbOutput.ForeColor = txtStdin.ForeColor = _theme.Foreground;
        rtbOutput.Font = txtStdin.Font = _theme.Font;
        lstOutline.BackColor = _theme.NumberBackground;
        lstOutline.ForeColor = _theme.Foreground;
    }

    void ShowOutline(bool show)
    {
        splitInner.Panel2Collapsed = !show; mnuShowOutline.Checked = show;
        _settings.ShowOutline = show; _settings.Save();
    }

    void ShowOutput(bool show)
    {
        splitOuter.Panel2Collapsed = !show; mnuShowOutput.Checked = show;
        _settings.ShowOutput = show; _settings.Save();
    }

    void mnuShowOutline_Click(object sender, EventArgs e) => ShowOutline(splitInner.Panel2Collapsed);
    void mnuShowOutput_Click(object sender, EventArgs e) => ShowOutput(splitOuter.Panel2Collapsed);
    void btnHideOutput_Click(object sender, EventArgs e) => ShowOutput(false);

    void mnuAutoComplete_Click(object sender, EventArgs e)
    {
        _settings.AutoComplete = mnuAutoComplete.Checked; _settings.Save();
        foreach (EditorTab t in tabs.TabPages) t.Editor.AutoCompleteEnabled = _settings.AutoComplete;
    }

    // ================= Repliement =================
    void mnuFoldToggle_Click(object sender, EventArgs e) => Current?.Editor.ToggleFoldAtCaret();
    void mnuFoldAll_Click(object sender, EventArgs e) => Current?.Editor.FoldAll();
    void mnuUnfoldAll_Click(object sender, EventArgs e) => Current?.Editor.UnfoldAll();

    // ================= Plan des définitions =================
    void RefreshOutline()
    {
        var items = Current == null ? new List<OutlineItem>() : Current.Editor.Outline.ToList();
        if (_settings.OutlineDefsOnly) items = items.Where(i => i.Kind == ":").ToList();
        string f = txtOutlineFilter.Text.Trim();
        if (f.Length > 0) items = items.Where(i => i.Name.Contains(f, StringComparison.OrdinalIgnoreCase)).ToList();
        string key = string.Join("|", items.Select(i => i.Line + i.ToString()));
        if (key == _outlineKey) return;
        _outlineKey = key;
        lstOutline.BeginUpdate();
        lstOutline.Items.Clear();
        lstOutline.Items.AddRange(items.Cast<object>().ToArray());
        lstOutline.EndUpdate();
    }

    void txtOutlineFilter_TextChanged(object sender, EventArgs e) { _outlineKey = ""; RefreshOutline(); }

    void lstOutline_MouseClick(object sender, MouseEventArgs e)
    {
        int i = lstOutline.IndexFromPoint(e.Location);
        if (i >= 0) GoToOutlineItem(i);
    }

    void lstOutline_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter && lstOutline.SelectedIndex >= 0) { GoToOutlineItem(lstOutline.SelectedIndex); e.Handled = true; }
    }

    void GoToOutlineItem(int index)
    {
        if (lstOutline.Items[index] is OutlineItem it) Current?.Editor.GoToLine(it.Line);
    }

    // ================= Sortie =================
    Color ErrColor => _theme.Background.GetBrightness() > 0.5f ? Color.Firebrick : Color.FromArgb(244, 135, 113);
    static Color InfoColor => Color.FromArgb(86, 156, 214);

    void AppendOutput(string text, Color color)
    {
        text = AnsiRx.Replace(text, "").Replace("\r\n", "\n").Replace("\r", "");
        if (text.Length == 0) return;
        if (rtbOutput.TextLength > 400_000) { rtbOutput.Select(0, 100_000); rtbOutput.SelectedText = ""; }
        rtbOutput.SelectionStart = rtbOutput.TextLength;
        rtbOutput.SelectionLength = 0;
        rtbOutput.SelectionColor = color;
        rtbOutput.AppendText(text);
        rtbOutput.ScrollToCaret();
    }

    void mnuClearOutput_Click(object sender, EventArgs e) => rtbOutput.Clear();

    /// <summary>Double-clic sur une ligne « fichier:ligne: erreur » → ouvre le fichier à la ligne indiquée.</summary>
    void rtbOutput_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        int line = rtbOutput.GetLineFromCharIndex(rtbOutput.GetCharIndexFromPosition(e.Location));
        if (line < 0 || line >= rtbOutput.Lines.Length) return;
        var m = ErrLocRx.Match(rtbOutput.Lines[line]);
        if (!m.Success) return;
        string f = m.Groups["file"].Value.Trim();
        int ln = int.Parse(m.Groups["line"].Value);
        if (!Path.IsPathRooted(f)) f = Path.Combine(_runDir, f);

        EditorTab target = null;
        try
        {
            if (_runIsTemp && _runTab != null && tabs.TabPages.Contains(_runTab) &&
                string.Equals(Path.GetFullPath(f), Path.GetFullPath(_runFile), StringComparison.OrdinalIgnoreCase)) target = _runTab;
            else if (File.Exists(f)) { OpenFile(f); target = Current; }
        }
        catch { }
        if (target == null) return;
        tabs.SelectedTab = target;
        target.Editor.GoToLine(ln - 1);
    }

    // ================= Exécution =================
    void UpdateRunButtons()
    {
        bool running = _proc != null;
        btnRun.Enabled = mnuRun.Enabled = !running;
        btnStop.Enabled = mnuStop.Enabled = running;
        txtStdin.Enabled = running;
    }

    bool ShowForthSettings()
    {
        using var dlg = new SettingsForm(_settings.Profiles, _settings.ActiveProfile, _settings.SaveBeforeRun, _settings.ClearOutputBeforeRun);
        if (dlg.ShowDialog(this) != DialogResult.OK) return false;
        _settings.Profiles = dlg.Profiles.ToList();
        _settings.ActiveProfile = dlg.ActiveName;
        _settings.SaveBeforeRun = dlg.SaveBeforeRun; _settings.ClearOutputBeforeRun = dlg.ClearBeforeRun;
        _settings.Save();
        return !string.IsNullOrWhiteSpace(_settings.CurrentProfile?.Exe);
    }

    void mnuForthSettings_Click(object sender, EventArgs e) => ShowForthSettings();

    async void mnuRun_Click(object sender, EventArgs e)
    {
        try { await RunCurrentAsync(); }
        catch (Exception ex) { AppendOutput("Erreur : " + ex.Message + "\n", ErrColor); _proc = null; UpdateRunButtons(); }
    }

    void mnuStop_Click(object sender, EventArgs e) => StopProcess();

    async Task RunCurrentAsync()
    {
        if (_proc != null) { MessageBox.Show(this, "Un programme est déjà en cours. Arrêtez-le d'abord (Maj+F5).", "Exécution"); return; }
        var t = Current;
        if (t == null) return;
        var prof = _settings.CurrentProfile;
        if (prof == null || string.IsNullOrWhiteSpace(prof.Exe))
        {
            if (!ShowForthSettings()) return;
            prof = _settings.CurrentProfile;
        }

        // Fichier à exécuter : le fichier enregistré, ou un fichier temporaire
        string file;
        bool temp = false;
        if (t.FilePath == null || t.Modified)
        {
            if (_settings.SaveBeforeRun) { if (!Save(t, false)) return; file = t.FilePath; }
            else
            {
                file = Path.Combine(Path.GetTempPath(), $"forth_run_{Guid.NewGuid():N}.fs");
                File.WriteAllText(file, t.Editor.GetTextForFile(), new UTF8Encoding(false));
                temp = true;
            }
        }
        else file = t.FilePath;

        ShowOutput(true);
        if (_settings.ClearOutputBeforeRun) rtbOutput.Clear();
        _runTab = t; _runFile = file; _runDir = Path.GetDirectoryName(file) ?? ""; _runIsTemp = temp;

        string args = prof.Args.Replace("{file}", file).Replace("{dir}", _runDir).Replace("{name}", Path.GetFileName(file));
        var psi = new ProcessStartInfo(prof.Exe, args)
        {
            WorkingDirectory = _runDir,
            RedirectStandardOutput = true, RedirectStandardError = true, RedirectStandardInput = true,
            UseShellExecute = false, CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8, StandardErrorEncoding = Encoding.UTF8,
            StandardInputEncoding = new UTF8Encoding(false)
        };
        AppendOutput($"> [{prof.Name}] {prof.Exe} {args}\n", InfoColor);

        Process p;
        try { p = Process.Start(psi); }
        catch (Exception ex)
        {
            AppendOutput("Impossible de lancer l'interpréteur : " + ex.Message + "\n", ErrColor);
            AppendOutput("Vérifiez Paramètres → Interpréteur Forth.\n", InfoColor);
            if (temp) try { File.Delete(file); } catch { }
            return;
        }
        p.StandardInput.AutoFlush = true;
        _proc = p;
        UpdateRunButtons();

        var sw = Stopwatch.StartNew();
        int code = -1;
        try
        {
            await Task.WhenAll(Pump(p.StandardOutput, false), Pump(p.StandardError, true));
            await p.WaitForExitAsync();
            code = p.ExitCode;
        }
        catch (Exception ex) { AppendOutput("\nErreur de lecture : " + ex.Message + "\n", ErrColor); }
        sw.Stop();
        AppendOutput($"\n[Terminé — code {code} — {sw.Elapsed.TotalSeconds:0.00} s]\n", InfoColor);

        _proc = null;
        try { p.Dispose(); } catch { }
        if (temp) try { File.Delete(file); } catch { }
        UpdateRunButtons();
    }

    /// <summary>Lecture par blocs (et non par lignes) pour voir aussi les invites sans retour à la ligne.</summary>
    async Task Pump(StreamReader reader, bool isError)
    {
        var buf = new char[2048];
        int n;
        while ((n = await reader.ReadAsync(buf, 0, buf.Length)) > 0)
            AppendOutput(new string(buf, 0, n), isError ? ErrColor : _theme.Foreground);
    }

    void StopProcess()
    {
        var p = _proc;
        if (p == null) return;
        try
        {
            if (!p.HasExited) { p.Kill(true); AppendOutput("\n[Arrêté par l'utilisateur]\n", InfoColor); }
        }
        catch { }
    }

    /// <summary>Entrée standard du programme : Entrée envoie la ligne.</summary>
    void txtStdin_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.Handled = e.SuppressKeyPress = true;
        var p = _proc;
        if (p == null) return;
        string line = txtStdin.Text;
        try
        {
            p.StandardInput.WriteLine(line);
            AppendOutput(line + "\n", InfoColor);
        }
        catch { }
        txtStdin.Clear();
    }
}
