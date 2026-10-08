using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace ForthEditor;

// Partie de MainForm : panneau « Projet » (dossier + fichiers), à gauche de l'éditeur.
public partial class MainForm
{
    sealed record ProjNode(string Path, bool IsDir);

    sealed class ProjectChoice
    {
        public string Path { get; }
        public ProjectChoice(string path) { Path = path; }
        public override string ToString()
        {
            string name = System.IO.Path.GetFileName(Path.TrimEnd('\\', '/'));
            return name.Length > 0 ? name : Path;
        }
    }

    static readonly string[] ForthExtensions = { ".fs", ".fth", ".4th", ".forth", ".f", ".fb", ".fr", ".frt", ".seq" };
    static readonly HashSet<string> IgnoredFolders = new(StringComparer.OrdinalIgnoreCase) { ".git", ".vs", "bin", "obj", "node_modules" };
    const int MaxProjects = 10;

    ImageList _projIcons;
    FileSystemWatcher _projWatcher;
    readonly System.Windows.Forms.Timer _projTimer = new() { Interval = 600 };
    readonly ToolTip _projTip = new();

    // ================= Initialisation =================
    void InitProject()
    {
        _projIcons = new ImageList { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(16, 16) };
        _projIcons.Images.Add("folder", MakeIcon(true));
        _projIcons.Images.Add("file", MakeIcon(false));
        tvProject.ImageList = _projIcons;
        tvProject.ShowNodeToolTips = true;

        cboProjectFilter.Items.AddRange(new object[] { "Fichiers Forth", "Tous les fichiers" });
        cboProjectFilter.SelectedIndex = _settings.ProjectShowAll ? 1 : 0;

        splitLeft.Panel1Collapsed = !_settings.ShowProject;
        mnuShowProject.Checked = _settings.ShowProject;

        _projTimer.Tick += (s, e) => { _projTimer.Stop(); BuildProjectTree(); };
        FillProjectCombo();
        BuildProjectTree();
        StartProjectWatcher();
    }

    void DisposeProject()
    {
        _projTimer.Stop();
        _projWatcher?.Dispose();
        _projWatcher = null;
    }

    static Bitmap MakeIcon(bool folder)
    {
        var bmp = new Bitmap(16, 16);
        using var g = Graphics.FromImage(bmp);
        if (folder)
        {
            using var fill = new SolidBrush(Color.FromArgb(240, 200, 80));
            using var edge = new Pen(Color.FromArgb(190, 150, 40));
            g.FillRectangle(fill, 1, 2, 6, 3);
            g.FillRectangle(fill, 1, 4, 14, 10);
            g.DrawRectangle(edge, 1, 4, 13, 9);
        }
        else
        {
            using var edge = new Pen(Color.FromArgb(120, 120, 120));
            using var line = new Pen(Color.FromArgb(170, 170, 170));
            g.FillRectangle(Brushes.White, 3, 1, 10, 14);
            g.DrawRectangle(edge, 3, 1, 9, 13);
            for (int y = 5; y <= 11; y += 3) g.DrawLine(line, 5, y, 10, y);
        }
        return bmp;
    }

    // ================= Choix du dossier =================
    void FillProjectCombo()
    {
        cboProjectFolder.Items.Clear();
        foreach (var f in _settings.ProjectFolders) cboProjectFolder.Items.Add(new ProjectChoice(f));
        int idx = _settings.ProjectFolders.FindIndex(f => string.Equals(f, _settings.ProjectFolder, StringComparison.OrdinalIgnoreCase));
        cboProjectFolder.SelectedIndex = idx;
        _projTip.SetToolTip(cboProjectFolder, _settings.ProjectFolder ?? "Aucun dossier sélectionné");
    }

    void BrowseProjectFolder()
    {
        using var dlg = new FolderBrowserDialog
        {
            Description = "Choisissez le dossier du projet (ses fichiers Forth s'afficheront dans le panneau de gauche)",
            UseDescriptionForTitle = true,
            ShowNewFolderButton = true,
            SelectedPath = Directory.Exists(_settings.ProjectFolder ?? "") ? _settings.ProjectFolder : ""
        };
        if (dlg.ShowDialog(this) == DialogResult.OK) { ShowProject(true); SetProjectFolder(dlg.SelectedPath); }
    }

    void SetProjectFolder(string path)
    {
        try { path = Path.GetFullPath(path); } catch { return; }
        if (!Directory.Exists(path))
        {
            MessageBox.Show(this, "Ce dossier n'existe plus :\n" + path, "Projet", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _settings.ProjectFolders.RemoveAll(f => string.Equals(f, path, StringComparison.OrdinalIgnoreCase));
            if (string.Equals(_settings.ProjectFolder, path, StringComparison.OrdinalIgnoreCase)) _settings.ProjectFolder = null;
            _settings.Save();
            FillProjectCombo(); BuildProjectTree(); StartProjectWatcher();
            return;
        }
        _settings.ProjectFolder = path;
        _settings.ProjectFolders.RemoveAll(f => string.Equals(f, path, StringComparison.OrdinalIgnoreCase));
        _settings.ProjectFolders.Insert(0, path);
        if (_settings.ProjectFolders.Count > MaxProjects)
            _settings.ProjectFolders.RemoveRange(MaxProjects, _settings.ProjectFolders.Count - MaxProjects);
        _settings.Save();
        FillProjectCombo(); BuildProjectTree(); StartProjectWatcher();
    }

    void ShowProject(bool show)
    {
        splitLeft.Panel1Collapsed = !show;
        mnuShowProject.Checked = show;
        _settings.ShowProject = show; _settings.Save();
    }

    void mnuOpenFolder_Click(object sender, EventArgs e) => BrowseProjectFolder();
    void btnProjectBrowse_Click(object sender, EventArgs e) => BrowseProjectFolder();
    void mnuShowProject_Click(object sender, EventArgs e) => ShowProject(splitLeft.Panel1Collapsed);

    void cboProjectFolder_SelectionChangeCommitted(object sender, EventArgs e)
    {
        if (cboProjectFolder.SelectedItem is ProjectChoice pc) SetProjectFolder(pc.Path);
    }

    void cboProjectFilter_SelectionChangeCommitted(object sender, EventArgs e)
    {
        _settings.ProjectShowAll = cboProjectFilter.SelectedIndex == 1;
        _settings.Save();
        BuildProjectTree();
    }

    void txtProjectFilter_TextChanged(object sender, EventArgs e) => BuildProjectTree();

    void chkOutlineDefsOnly_CheckedChanged(object sender, EventArgs e)
    {
        _settings.OutlineDefsOnly = chkOutlineDefsOnly.Checked;
        _settings.Save();
        _outlineKey = "";
        RefreshOutline();
    }

    // ================= Arbre des fichiers =================
    void BuildProjectTree()
    {
        var expanded = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        CollectExpanded(tvProject.Nodes, expanded);

        tvProject.BeginUpdate();
        try
        {
            tvProject.Nodes.Clear();
            string root = _settings.ProjectFolder;
            if (string.IsNullOrEmpty(root) || !Directory.Exists(root))
            {
                tvProject.Nodes.Add(new TreeNode("Aucun dossier — cliquez sur « … »") { ForeColor = Color.Gray, ImageKey = "file", SelectedImageKey = "file" });
                return;
            }
            string name = Path.GetFileName(root.TrimEnd('\\', '/'));
            var rootNode = new TreeNode(name.Length > 0 ? name : root)
            {
                Tag = new ProjNode(root, true), ImageKey = "folder", SelectedImageKey = "folder", ToolTipText = root
            };
            tvProject.Nodes.Add(rootNode);
            LoadChildren(rootNode);
            rootNode.Expand();
            RestoreExpanded(rootNode, expanded);
        }
        finally { tvProject.EndUpdate(); }
    }

    static void CollectExpanded(TreeNodeCollection nodes, HashSet<string> set)
    {
        foreach (TreeNode n in nodes)
        {
            if (n.IsExpanded && n.Tag is ProjNode { IsDir: true } p) set.Add(p.Path);
            CollectExpanded(n.Nodes, set);
        }
    }

    void RestoreExpanded(TreeNode node, HashSet<string> expanded)
    {
        foreach (TreeNode child in node.Nodes)
            if (child.Tag is ProjNode { IsDir: true } p && expanded.Contains(p.Path))
            {
                LoadChildren(child);
                child.Expand();
                RestoreExpanded(child, expanded);
            }
    }

    void LoadChildren(TreeNode node)
    {
        node.Nodes.Clear();
        if (node.Tag is not ProjNode { IsDir: true } pn) return;
        bool all = cboProjectFilter.SelectedIndex == 1;
        string filter = txtProjectFilter.Text.Trim();
        try
        {
            foreach (var dir in Directory.EnumerateDirectories(pn.Path).OrderBy(d => d, StringComparer.OrdinalIgnoreCase))
            {
                string name = Path.GetFileName(dir);
                if (IgnoredFolders.Contains(name)) continue;
                try { if ((new DirectoryInfo(dir).Attributes & (FileAttributes.Hidden | FileAttributes.System)) != 0) continue; } catch { }
                var dn = new TreeNode(name) { Tag = new ProjNode(dir, true), ImageKey = "folder", SelectedImageKey = "folder" };
                dn.Nodes.Add(new TreeNode("…"));              // marque « contenu non chargé » (Tag == null)
                node.Nodes.Add(dn);
            }
            foreach (var file in Directory.EnumerateFiles(pn.Path).OrderBy(f => f, StringComparer.OrdinalIgnoreCase))
            {
                string name = Path.GetFileName(file);
                if (!all && !ForthExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase)) continue;
                if (filter.Length > 0 && name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) < 0) continue;
                node.Nodes.Add(new TreeNode(name)
                {
                    Tag = new ProjNode(file, false), ImageKey = "file", SelectedImageKey = "file", ToolTipText = file
                });
            }
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException or IOException)
        {
            node.Nodes.Add(new TreeNode("(dossier inaccessible)") { ForeColor = Color.Gray, ImageKey = "file", SelectedImageKey = "file" });
        }
    }

    void tvProject_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
        var n = e.Node;
        if (n.Tag is ProjNode { IsDir: true } && n.Nodes.Count == 1 && n.Nodes[0].Tag == null) LoadChildren(n);
    }

    void tvProject_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) => OpenProjectNode(e.Node);

    void tvProject_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        if (tvProject.SelectedNode?.Tag is ProjNode { IsDir: false }) OpenProjectNode(tvProject.SelectedNode);
        else tvProject.SelectedNode?.Toggle();
        e.Handled = e.SuppressKeyPress = true;
    }

    void tvProject_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            var n = tvProject.GetNodeAt(e.X, e.Y);
            if (n != null) tvProject.SelectedNode = n;
        }
    }

    void OpenProjectNode(TreeNode n)
    {
        if (n?.Tag is ProjNode { IsDir: false } p) OpenFile(p.Path);
    }

    // ================= Menu contextuel =================
    ProjNode SelectedProjNode => tvProject.SelectedNode?.Tag as ProjNode;

    void ctxProject_Opening(object sender, CancelEventArgs e)
    {
        var n = SelectedProjNode;
        ctxProjOpen.Enabled = n is { IsDir: false };
        ctxProjNewFile.Enabled = !string.IsNullOrEmpty(_settings.ProjectFolder) && Directory.Exists(_settings.ProjectFolder);
        ctxProjExplorer.Enabled = n != null;
    }

    void ctxProjOpen_Click(object sender, EventArgs e) => OpenProjectNode(tvProject.SelectedNode);
    void ctxProjRefresh_Click(object sender, EventArgs e) => BuildProjectTree();

    void ctxProjExplorer_Click(object sender, EventArgs e)
    {
        var n = SelectedProjNode;
        if (n == null) return;
        try
        {
            Process.Start(new ProcessStartInfo("explorer.exe", n.IsDir ? $"\"{n.Path}\"" : $"/select,\"{n.Path}\"") { UseShellExecute = true });
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Explorateur", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    void ctxProjNewFile_Click(object sender, EventArgs e)
    {
        string root = _settings.ProjectFolder;
        if (string.IsNullOrEmpty(root) || !Directory.Exists(root)) return;
        var n = SelectedProjNode;
        string dir = n == null ? root : n.IsDir ? n.Path : Path.GetDirectoryName(n.Path) ?? root;

        using var dlg = new PromptForm("Nouveau fichier", "Nom du fichier :", "nouveau.fs");
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        string name = dlg.Value.Trim();
        if (name.Length == 0) return;
        if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) { MessageBox.Show(this, "Nom de fichier invalide."); return; }
        if (!Path.HasExtension(name)) name += ".fs";

        string full = Path.Combine(dir, name);
        try { if (!File.Exists(full)) File.WriteAllText(full, ""); }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Création impossible", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
        BuildProjectTree();
        OpenFile(full);
    }

    // ================= Surveillance du dossier =================
    void StartProjectWatcher()
    {
        _projWatcher?.Dispose();
        _projWatcher = null;
        string root = _settings.ProjectFolder;
        if (string.IsNullOrEmpty(root) || !Directory.Exists(root)) return;
        try
        {
            _projWatcher = new FileSystemWatcher(root)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName,
                EnableRaisingEvents = true
            };
            _projWatcher.Created += (s, e) => RequestProjectRefresh();
            _projWatcher.Deleted += (s, e) => RequestProjectRefresh();
            _projWatcher.Renamed += (s, e) => RequestProjectRefresh();
        }
        catch { /* dossier inaccessible : pas d'actualisation automatique */ }
    }

    void RequestProjectRefresh()
    {
        if (!IsHandleCreated || IsDisposed) return;
        BeginInvoke(new Action(() => { _projTimer.Stop(); _projTimer.Start(); }));   // regroupe les rafales d'événements
    }
}
