// ForthEditor — éditeur de code Forth et terminal série pour ESP32Forth
// Copyright (C) 2026 Germain PHILIPPE
// Licence : GNU General Public License v3.0 ou ultérieure — voir le fichier LICENSE.

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ForthEditor;

/// <summary>
/// Icônes PNG (64 px) embarquées dans l'exécutable : dossier « icons » du projet, ressources « icons.nom.png ».
/// Elles sont redimensionnées à la taille exacte voulue (menus 20 px, barre d'outils 32 px, ajustées au DPI).
/// Les sources sont générées par tools/make_icons.py.
/// </summary>
static class AppIcons
{
    static readonly Dictionary<string, Bitmap> _source = new();
    static readonly Dictionary<string, Bitmap> _cache = new();

    static Bitmap Source(string name)
    {
        if (_source.TryGetValue(name, out var b)) return b;
        using var st = typeof(AppIcons).Assembly.GetManifestResourceStream("icons." + name + ".png");
        if (st == null) return null;                       // icône absente : pas d'image, pas d'erreur
        using var tmp = new Bitmap(st);
        b = new Bitmap(tmp);                               // copie détachée du flux
        _source[name] = b;
        return b;
    }

    public static Image Get(string name, int size)
    {
        string key = name + "@" + size;
        if (_cache.TryGetValue(key, out var c)) return c;
        var src = Source(name);
        if (src == null) return null;
        var bmp = new Bitmap(size, size, PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(bmp))
        {
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawImage(src, new Rectangle(0, 0, size, size));
        }
        _cache[key] = bmp;
        return bmp;
    }
}

// Partie de MainForm : icônes des menus et barre d'outils.
public partial class MainForm
{
    ToolStrip _toolBar;

    /// <summary>Appelée au chargement de la fenêtre (après la mise à l'échelle automatique).</summary>
    void InitIcons()
    {
        float k = DeviceDpi / 96f;
        int small = (int)Math.Round(20 * k);     // menus
        int big = (int)Math.Round(32 * k);       // barre d'outils

        void Set(ToolStripItem it, string name)
        {
            var im = AppIcons.Get(name, small);
            if (it == null || im == null) return;
            it.ImageScaling = ToolStripItemImageScaling.None;
            it.Image = im;
        }

        // --- Fichier
        Set(mnuNew, "new"); Set(mnuOpen, "open"); Set(mnuOpenFolder, "openfolder"); Set(mnuRecent, "recent");
        Set(mnuSave, "save"); Set(mnuSaveAs, "saveas"); Set(mnuPrint, "print"); Set(mnuClose, "close");
        Set(mnuDelete, "delete"); Set(mnuQuit, "quit");
        // --- Édition
        Set(mnuUndo, "undo"); Set(mnuRedo, "redo"); Set(mnuCut, "cut"); Set(mnuCopy, "copy"); Set(mnuPaste, "paste");
        Set(mnuSelectAll, "selectall"); Set(mnuFind, "find"); Set(mnuFindNext, "findnext"); Set(mnuFindPrev, "findprev");
        Set(mnuReplace, "replace"); Set(mnuGoTo, "goto"); Set(mnuReindent, "reindent");
        // --- Affichage
        Set(mnuReloadSyntax, "syntax_reload"); Set(mnuEditSyntax, "syntax_edit"); Set(mnuOpenSyntaxFolder, "folder");
        Set(mnuTheme, "theme"); Set(mnuThemeDark, "theme_dark"); Set(mnuThemeLight, "theme_light");
        Set(mnuZoomIn, "zoomin"); Set(mnuZoomOut, "zoomout"); Set(mnuZoomReset, "zoomreset");
        Set(mnuShowProject, "pane_project"); Set(mnuShowOutline, "pane_outline"); Set(mnuShowOutput, "pane_output");
        Set(mnuFold, "fold"); Set(mnuFoldToggle, "foldtoggle"); Set(mnuFoldAll, "foldall"); Set(mnuUnfoldAll, "unfoldall");
        Set(mnuRestoreSession, "session");
        // --- Exécution, outils, paramètres, aide
        Set(mnuRun, "run"); Set(mnuStop, "stop"); Set(mnuClearOutput, "clear");
        Set(mnuTerminal, "terminal");
        Set(mnuForthSettings, "settings"); Set(mnuAutoComplete, "autocomplete");
        Set(afficherAideToolStripMenuItem, "help"); Set(mnuPdfOpen, "pdfopen"); Set(mnuPdfFolder, "pdffolder");
        Set(aProposToolStripMenuItem, "about");
        // --- Menus contextuels
        Set(ctxCut, "cut"); Set(ctxCopy, "copy"); Set(ctxPaste, "paste"); Set(ctxSelectAll, "selectall");
        Set(ctxReindent, "reindent");
        Set(ctxProjOpen, "open"); Set(ctxProjNewFile, "new"); Set(ctxProjExplorer, "folder"); Set(ctxProjRefresh, "refresh");

        BuildToolBar(big);
    }

    /// <summary>Barre d'outils : chaque bouton déclenche l'élément de menu correspondant et en reprend l'état.</summary>
    void BuildToolBar(int size)
    {
        _toolBar = new ToolStrip
        {
            Name = "toolBar",
            Dock = DockStyle.Top,
            GripStyle = ToolStripGripStyle.Hidden,
            ImageScalingSize = new Size(size, size),
            ShowItemToolTips = true,
            CanOverflow = true,
            Padding = new Padding(6, 3, 6, 3),
        };

        void Btn(ToolStripMenuItem menu, string icon)
        {
            var img = AppIcons.Get(icon, size);
            if (menu == null || img == null) return;
            string label = menu.Text.Replace("&", "").TrimEnd('…');
            string tip = label;
            if (menu.ShortcutKeys != Keys.None)
                tip += " (" + new KeysConverter().ConvertToString(menu.ShortcutKeys) + ")";
            var b = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                ImageScaling = ToolStripItemImageScaling.None,
                Image = img,
                Text = label,
                ToolTipText = tip,
                AutoToolTip = false,
                Padding = new Padding(5),
                Margin = new Padding(1, 1, 1, 2),
                Enabled = menu.Enabled,
                Checked = menu.Checked,
            };
            b.Click += (s, e) => menu.PerformClick();
            menu.EnabledChanged += (s, e) => b.Enabled = menu.Enabled;
            menu.CheckedChanged += (s, e) => b.Checked = menu.Checked;
            _toolBar.Items.Add(b);
        }
        void Sep() => _toolBar.Items.Add(new ToolStripSeparator { Margin = new Padding(6, 0, 6, 0) });

        Btn(mnuNew, "new"); Btn(mnuOpen, "open"); Btn(mnuOpenFolder, "openfolder"); Btn(mnuSave, "save"); Btn(mnuPrint, "print");
        Sep();
        Btn(mnuUndo, "undo"); Btn(mnuRedo, "redo");
        Sep();
        Btn(mnuCut, "cut"); Btn(mnuCopy, "copy"); Btn(mnuPaste, "paste");
        Sep();
        Btn(mnuFind, "find"); Btn(mnuReplace, "replace"); Btn(mnuReindent, "reindent");
        Sep();
        Btn(mnuRun, "run"); Btn(mnuStop, "stop"); Btn(mnuClearOutput, "clear");
        Sep();
        Btn(mnuTerminal, "terminal"); Btn(mnuForthSettings, "settings");
        Sep();
        Btn(mnuShowProject, "pane_project"); Btn(mnuShowOutline, "pane_outline"); Btn(mnuShowOutput, "pane_output");
        Sep();
        Btn(afficherAideToolStripMenuItem, "help");

        SuspendLayout();
        Controls.Add(_toolBar);
        // même rang que la barre de menus, juste en dessous d'elle : le menu reste tout en haut
        Controls.SetChildIndex(_toolBar, Controls.GetChildIndex(menuStrip));
        ResumeLayout(true);
    }
}
