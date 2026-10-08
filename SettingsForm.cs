using System.Diagnostics;

namespace ForthEditor;

/// <summary>Configurations d'interpréteur Forth (nom libre, chemin, arguments). Interface : SettingsForm.Designer.cs</summary>
public partial class SettingsForm : Form
{
    readonly List<ForthProfile> _profiles = new();
    int _index;     // configuration actuellement affichée

    /// <summary>Chemin tel que saisi ou choisi (les guillemets collés autour sont retirés).</summary>
    string Exe => txtExe.Text.Trim().Trim('"');

    public IReadOnlyList<ForthProfile> Profiles { get { StoreFields(); return _profiles; } }
    public string ActiveName => _profiles[_index].Name;
    public bool SaveBeforeRun => chkSave.Checked;
    public bool ClearBeforeRun => chkClear.Checked;

    public SettingsForm() { InitializeComponent(); }

    public SettingsForm(IEnumerable<ForthProfile> profiles, string active, bool saveBeforeRun, bool clearBeforeRun) : this()
    {
        // Copie de travail : rien n'est modifié tant que l'on ne valide pas par OK
        _profiles.AddRange(profiles.Select(p => new ForthProfile { Name = p.Name, Exe = p.Exe, Args = p.Args, TestArgs = p.TestArgs }));
        if (_profiles.Count == 0) _profiles.Add(new ForthProfile { Name = "gforth", Exe = "gforth", Args = "\"{file}\" -e bye" });
        _index = Math.Max(0, _profiles.FindIndex(p => string.Equals(p.Name, active, StringComparison.OrdinalIgnoreCase)));
        chkSave.Checked = saveBeforeRun;
        chkClear.Checked = clearBeforeRun;
        RefreshProfileList();
        LoadFields();
    }

    // ---------- Configurations ----------
    void RefreshProfileList()
    {
        cboProfile.Items.Clear();
        foreach (var p in _profiles) cboProfile.Items.Add(p.Name);
        cboProfile.SelectedIndex = _index;
        btnDelete.Enabled = _profiles.Count > 1;
    }

    void LoadFields()
    {
        txtExe.Text = _profiles[_index].Exe;
        txtArgs.Text = _profiles[_index].Args;
        txtTestArgs.Text = _profiles[_index].TestArgs ?? "";
    }

    void StoreFields()
    {
        if (_index < 0 || _index >= _profiles.Count) return;
        _profiles[_index].Exe = Exe;
        _profiles[_index].Args = txtArgs.Text.Trim();
        _profiles[_index].TestArgs = txtTestArgs.Text.Trim();
    }

    bool NameTaken(string name, int exceptIndex) =>
        _profiles.Where((p, i) => i != exceptIndex).Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

    string AskName(string title, string initial, int exceptIndex)
    {
        using var dlg = new PromptForm(title, "Nom de la configuration :", initial);
        if (dlg.ShowDialog(this) != DialogResult.OK) return null;
        string name = dlg.Value.Trim();
        if (name.Length == 0) return null;
        if (NameTaken(name, exceptIndex)) { MessageBox.Show(this, "Une configuration porte déjà ce nom.", title); return null; }
        return name;
    }

    void cboProfile_SelectionChangeCommitted(object sender, EventArgs e)
    {
        StoreFields();                      // on garde ce qui a été saisi pour la configuration quittée
        _index = cboProfile.SelectedIndex;
        LoadFields();
    }

    void btnNew_Click(object sender, EventArgs e)
    {
        StoreFields();
        string name = AskName("Nouvelle configuration", "Nouvelle configuration", -1);
        if (name == null) return;
        // La nouvelle configuration part d'une copie de la configuration courante
        _profiles.Add(new ForthProfile { Name = name, Exe = _profiles[_index].Exe, Args = _profiles[_index].Args, TestArgs = _profiles[_index].TestArgs });
        _index = _profiles.Count - 1;
        RefreshProfileList();
        LoadFields();
        txtExe.Focus();
    }

    void btnRename_Click(object sender, EventArgs e)
    {
        string name = AskName("Renommer la configuration", _profiles[_index].Name, _index);
        if (name == null) return;
        StoreFields();
        _profiles[_index].Name = name;
        RefreshProfileList();
    }

    void btnDelete_Click(object sender, EventArgs e)
    {
        if (_profiles.Count <= 1) return;
        if (MessageBox.Show(this, $"Supprimer la configuration « {_profiles[_index].Name} » ?", "Supprimer",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        _profiles.RemoveAt(_index);
        _index = Math.Min(_index, _profiles.Count - 1);
        RefreshProfileList();
        LoadFields();
    }

    // ---------- Exécutable ----------
    void btnBrowse_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Choisir l'interpréteur Forth",
            Filter = "Exécutables (*.exe;*.bat;*.cmd)|*.exe;*.bat;*.cmd|Tous les fichiers|*.*"
        };
        // Repart du dossier du chemin actuel s'il est absolu
        try
        {
            if (Path.IsPathRooted(Exe))
            {
                dlg.InitialDirectory = Path.GetDirectoryName(Exe);
                dlg.FileName = Path.GetFileName(Exe);
            }
        }
        catch { }
        if (dlg.ShowDialog(this) == DialogResult.OK) txtExe.Text = dlg.FileName;      // chemin absolu conservé tel quel
    }

    /// <summary>
    /// Lance l'interpréteur avec les « arguments du test » (vides par défaut), entrée standard fermée pour qu'un interpréteur
    /// interactif se termine. Indique simplement s'il se lance ; sa sortie éventuelle est affichée à titre d'information.
    /// </summary>
    async void btnTest_Click(object sender, EventArgs e)
    {
        if (Exe.Length == 0) { MessageBox.Show(this, "Indiquez d'abord l'interpréteur.", "Test"); return; }
        btnTest.Enabled = false;
        try
        {
            var psi = new ProcessStartInfo(Exe, txtTestArgs.Text.Trim())
            {
                RedirectStandardOutput = true, RedirectStandardError = true, RedirectStandardInput = true,
                UseShellExecute = false, CreateNoWindow = true
            };
            using var p = Process.Start(psi);
            try { p.StandardInput.Close(); } catch { }
            var o = p.StandardOutput.ReadToEndAsync();
            var er = p.StandardError.ReadToEndAsync();

            bool exited = true;
            using (var cts = new CancellationTokenSource(4000))
            {
                try { await p.WaitForExitAsync(cts.Token); }
                catch (OperationCanceledException) { exited = false; try { p.Kill(true); } catch { } }
            }
            string output = ((await o) + "\n" + (await er)).Trim();
            if (output.Length > 700) output = output.Substring(0, 700) + "…";

            string head = exited
                ? $"L'interpréteur se lance correctement (code de sortie {p.ExitCode})."
                : "L'interpréteur se lance correctement (il est resté actif, arrêté après 4 s).";
            if (exited && p.ExitCode != 0)
                head += "\nUn code non nul est normal ici : il n'a reçu aucun fichier à exécuter.";
            MessageBox.Show(this, head + (output.Length > 0 ? "\n\nSortie de l'interpréteur :\n" + output : ""),
                "Test de l'interpréteur", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Impossible de lancer l'interpréteur :\n" + ex.Message, "Test de l'interpréteur",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { btnTest.Enabled = true; }
    }
}
