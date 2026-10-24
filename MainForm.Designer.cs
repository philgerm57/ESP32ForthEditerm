namespace ForthEditor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip = new MenuStrip();
            mnuFile = new ToolStripMenuItem();
            mnuNew = new ToolStripMenuItem();
            mnuOpen = new ToolStripMenuItem();
            mnuOpenFolder = new ToolStripMenuItem();
            mnuRecent = new ToolStripMenuItem();
            sepFile1 = new ToolStripSeparator();
            mnuSave = new ToolStripMenuItem();
            mnuSaveAs = new ToolStripMenuItem();
            sepFile2 = new ToolStripSeparator();
            mnuPrint = new ToolStripMenuItem();
            sepFile3 = new ToolStripSeparator();
            mnuClose = new ToolStripMenuItem();
            mnuDelete = new ToolStripMenuItem();
            sepFile4 = new ToolStripSeparator();
            mnuQuit = new ToolStripMenuItem();
            mnuEdit = new ToolStripMenuItem();
            mnuUndo = new ToolStripMenuItem();
            mnuRedo = new ToolStripMenuItem();
            sepEdit1 = new ToolStripSeparator();
            mnuCut = new ToolStripMenuItem();
            mnuCopy = new ToolStripMenuItem();
            mnuPaste = new ToolStripMenuItem();
            mnuSelectAll = new ToolStripMenuItem();
            sepEdit2 = new ToolStripSeparator();
            mnuFind = new ToolStripMenuItem();
            mnuFindNext = new ToolStripMenuItem();
            mnuFindPrev = new ToolStripMenuItem();
            mnuReplace = new ToolStripMenuItem();
            sepEdit3 = new ToolStripSeparator();
            mnuGoTo = new ToolStripMenuItem();
            mnuReindent = new ToolStripMenuItem();
            mnuView = new ToolStripMenuItem();
            mnuReloadSyntax = new ToolStripMenuItem();
            mnuEditSyntax = new ToolStripMenuItem();
            mnuOpenSyntaxFolder = new ToolStripMenuItem();
            sepView1 = new ToolStripSeparator();
            mnuTheme = new ToolStripMenuItem();
            mnuThemeDark = new ToolStripMenuItem();
            mnuThemeLight = new ToolStripMenuItem();
            mnuZoomIn = new ToolStripMenuItem();
            mnuZoomOut = new ToolStripMenuItem();
            mnuZoomReset = new ToolStripMenuItem();
            sepView2 = new ToolStripSeparator();
            mnuShowProject = new ToolStripMenuItem();
            mnuShowOutline = new ToolStripMenuItem();
            mnuShowOutput = new ToolStripMenuItem();
            mnuFold = new ToolStripMenuItem();
            mnuFoldToggle = new ToolStripMenuItem();
            mnuFoldAll = new ToolStripMenuItem();
            mnuUnfoldAll = new ToolStripMenuItem();
            sepView3 = new ToolStripSeparator();
            mnuRestoreSession = new ToolStripMenuItem();
            mnuRunMenu = new ToolStripMenuItem();
            mnuRun = new ToolStripMenuItem();
            mnuStop = new ToolStripMenuItem();
            mnuClearOutput = new ToolStripMenuItem();
            mnuToolsMenu = new ToolStripMenuItem();
            mnuTerminal = new ToolStripMenuItem();
            mnuSettingsMenu = new ToolStripMenuItem();
            mnuForthSettings = new ToolStripMenuItem();
            mnuAutoComplete = new ToolStripMenuItem();
            aideToolStripMenuItem = new ToolStripMenuItem();
            afficherAideToolStripMenuItem = new ToolStripMenuItem();
            mnuPdfOpen = new ToolStripMenuItem();
            mnuPdfFolder = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            aProposToolStripMenuItem = new ToolStripMenuItem();
            statusStrip = new StatusStrip();
            lblSpring = new ToolStripStatusLabel();
            lblPos = new ToolStripStatusLabel();
            lblSel = new ToolStripStatusLabel();
            lblLines = new ToolStripStatusLabel();
            lblEol = new ToolStripStatusLabel();
            ctxEditor = new ContextMenuStrip(components);
            ctxCut = new ToolStripMenuItem();
            ctxCopy = new ToolStripMenuItem();
            ctxPaste = new ToolStripMenuItem();
            sepCtx = new ToolStripSeparator();
            ctxSelectAll = new ToolStripMenuItem();
            sepCtx2 = new ToolStripSeparator();
            ctxReindent = new ToolStripMenuItem();
            splitOuter = new SplitContainer();
            splitInner = new SplitContainer();
            tabs = new TabControl();
            pnlOutline = new Panel();
            lstOutline = new ListBox();
            txtOutlineFilter = new TextBox();
            chkOutlineDefsOnly = new CheckBox();
            lblOutline = new Label();
            pnlOutput = new Panel();
            rtbOutput = new RichTextBox();
            txtStdin = new TextBox();
            pnlOutputBar = new Panel();
            btnHideOutput = new Button();
            lblOutputTitle = new Label();
            btnClearOutput = new Button();
            btnStop = new Button();
            btnRun = new Button();
            splitLeft = new SplitContainer();
            pnlProject = new Panel();
            tvProject = new TreeView();
            ctxProject = new ContextMenuStrip(components);
            ctxProjOpen = new ToolStripMenuItem();
            ctxProjNewFile = new ToolStripMenuItem();
            sepProj = new ToolStripSeparator();
            ctxProjExplorer = new ToolStripMenuItem();
            ctxProjRefresh = new ToolStripMenuItem();
            txtProjectFilter = new TextBox();
            cboProjectFilter = new ComboBox();
            pnlProjectBar = new Panel();
            cboProjectFolder = new ComboBox();
            btnProjectBrowse = new Button();
            lblProject = new Label();
            menuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            ctxEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitOuter).BeginInit();
            splitOuter.Panel1.SuspendLayout();
            splitOuter.Panel2.SuspendLayout();
            splitOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitInner).BeginInit();
            splitInner.Panel1.SuspendLayout();
            splitInner.Panel2.SuspendLayout();
            splitInner.SuspendLayout();
            pnlOutline.SuspendLayout();
            pnlOutput.SuspendLayout();
            pnlOutputBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitLeft).BeginInit();
            splitLeft.Panel1.SuspendLayout();
            splitLeft.Panel2.SuspendLayout();
            splitLeft.SuspendLayout();
            pnlProject.SuspendLayout();
            ctxProject.SuspendLayout();
            pnlProjectBar.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { mnuFile, mnuEdit, mnuView, mnuRunMenu, mnuToolsMenu, mnuSettingsMenu, aideToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(7, 3, 0, 3);
            menuStrip.Size = new Size(1257, 30);
            menuStrip.TabIndex = 0;
            // 
            // mnuFile
            // 
            mnuFile.DropDownItems.AddRange(new ToolStripItem[] { mnuNew, mnuOpen, mnuOpenFolder, mnuRecent, sepFile1, mnuSave, mnuSaveAs, sepFile2, mnuPrint, sepFile3, mnuClose, mnuDelete, sepFile4, mnuQuit });
            mnuFile.Name = "mnuFile";
            mnuFile.Size = new Size(66, 24);
            mnuFile.Text = "&Fichier";
            // 
            // mnuNew
            // 
            mnuNew.Name = "mnuNew";
            mnuNew.ShortcutKeys = Keys.Control | Keys.N;
            mnuNew.Size = new Size(397, 26);
            mnuNew.Text = "&Nouveau";
            mnuNew.Click += mnuNew_Click;
            // 
            // mnuOpen
            // 
            mnuOpen.Name = "mnuOpen";
            mnuOpen.ShortcutKeys = Keys.Control | Keys.O;
            mnuOpen.Size = new Size(397, 26);
            mnuOpen.Text = "&Ouvrir dans un nouvel onglet…";
            mnuOpen.Click += mnuOpen_Click;
            // 
            // mnuOpenFolder
            // 
            mnuOpenFolder.Name = "mnuOpenFolder";
            mnuOpenFolder.ShortcutKeys = Keys.Control | Keys.Shift | Keys.O;
            mnuOpenFolder.Size = new Size(397, 26);
            mnuOpenFolder.Text = "Ouvrir un &dossier (projet)…";
            mnuOpenFolder.Click += mnuOpenFolder_Click;
            // 
            // mnuRecent
            // 
            mnuRecent.Name = "mnuRecent";
            mnuRecent.Size = new Size(397, 26);
            mnuRecent.Text = "Fichiers &récents";
            // 
            // sepFile1
            // 
            sepFile1.Name = "sepFile1";
            sepFile1.Size = new Size(394, 6);
            // 
            // mnuSave
            // 
            mnuSave.Name = "mnuSave";
            mnuSave.ShortcutKeys = Keys.Control | Keys.S;
            mnuSave.Size = new Size(397, 26);
            mnuSave.Text = "&Enregistrer";
            mnuSave.Click += mnuSave_Click;
            // 
            // mnuSaveAs
            // 
            mnuSaveAs.Name = "mnuSaveAs";
            mnuSaveAs.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            mnuSaveAs.Size = new Size(397, 26);
            mnuSaveAs.Text = "Enregistrer &sous…";
            mnuSaveAs.Click += mnuSaveAs_Click;
            // 
            // sepFile2
            // 
            sepFile2.Name = "sepFile2";
            sepFile2.Size = new Size(394, 6);
            // 
            // mnuPrint
            // 
            mnuPrint.Name = "mnuPrint";
            mnuPrint.ShortcutKeys = Keys.Control | Keys.P;
            mnuPrint.Size = new Size(397, 26);
            mnuPrint.Text = "&Imprimer…";
            mnuPrint.Click += mnuPrint_Click;
            // 
            // sepFile3
            // 
            sepFile3.Name = "sepFile3";
            sepFile3.Size = new Size(394, 6);
            // 
            // mnuClose
            // 
            mnuClose.Name = "mnuClose";
            mnuClose.ShortcutKeys = Keys.Control | Keys.W;
            mnuClose.Size = new Size(397, 26);
            mnuClose.Text = "&Fermer l'onglet";
            mnuClose.Click += mnuClose_Click;
            // 
            // mnuDelete
            // 
            mnuDelete.Name = "mnuDelete";
            mnuDelete.Size = new Size(397, 26);
            mnuDelete.Text = "&Détruire le fichier de la page courante";
            mnuDelete.Click += mnuDelete_Click;
            // 
            // sepFile4
            // 
            sepFile4.Name = "sepFile4";
            sepFile4.Size = new Size(394, 6);
            // 
            // mnuQuit
            // 
            mnuQuit.Name = "mnuQuit";
            mnuQuit.ShortcutKeyDisplayString = "Alt+F4";
            mnuQuit.Size = new Size(397, 26);
            mnuQuit.Text = "&Quitter";
            mnuQuit.Click += mnuQuit_Click;
            // 
            // mnuEdit
            // 
            mnuEdit.DropDownItems.AddRange(new ToolStripItem[] { mnuUndo, mnuRedo, sepEdit1, mnuCut, mnuCopy, mnuPaste, mnuSelectAll, sepEdit2, mnuFind, mnuFindNext, mnuFindPrev, mnuReplace, sepEdit3, mnuGoTo, mnuReindent });
            mnuEdit.Name = "mnuEdit";
            mnuEdit.Size = new Size(70, 24);
            mnuEdit.Text = "&Édition";
            // 
            // mnuUndo
            // 
            mnuUndo.Name = "mnuUndo";
            mnuUndo.Size = new Size(377, 26);
            mnuUndo.Text = "&Annuler";
            mnuUndo.Click += Undo_Click;
            // 
            // mnuRedo
            // 
            mnuRedo.Name = "mnuRedo";
            mnuRedo.Size = new Size(377, 26);
            mnuRedo.Text = "&Rétablir";
            mnuRedo.Click += Redo_Click;
            // 
            // sepEdit1
            // 
            sepEdit1.Name = "sepEdit1";
            sepEdit1.Size = new Size(374, 6);
            // 
            // mnuCut
            // 
            mnuCut.Name = "mnuCut";
            mnuCut.Size = new Size(377, 26);
            mnuCut.Text = "Co&uper";
            mnuCut.Click += Cut_Click;
            // 
            // mnuCopy
            // 
            mnuCopy.Name = "mnuCopy";
            mnuCopy.Size = new Size(377, 26);
            mnuCopy.Text = "&Copier";
            mnuCopy.Click += Copy_Click;
            // 
            // mnuPaste
            // 
            mnuPaste.Name = "mnuPaste";
            mnuPaste.Size = new Size(377, 26);
            mnuPaste.Text = "&Coller";
            mnuPaste.Click += Paste_Click;
            // 
            // mnuSelectAll
            // 
            mnuSelectAll.Name = "mnuSelectAll";
            mnuSelectAll.Size = new Size(377, 26);
            mnuSelectAll.Text = "Tout &sélectionner";
            mnuSelectAll.Click += SelectAll_Click;
            // 
            // sepEdit2
            // 
            sepEdit2.Name = "sepEdit2";
            sepEdit2.Size = new Size(374, 6);
            // 
            // mnuFind
            // 
            mnuFind.Name = "mnuFind";
            mnuFind.ShortcutKeys = Keys.Control | Keys.F;
            mnuFind.Size = new Size(377, 26);
            mnuFind.Text = "&Rechercher…";
            mnuFind.Click += mnuFind_Click;
            // 
            // mnuFindNext
            // 
            mnuFindNext.Name = "mnuFindNext";
            mnuFindNext.ShortcutKeys = Keys.F3;
            mnuFindNext.Size = new Size(377, 26);
            mnuFindNext.Text = "Occurrence &suivante";
            mnuFindNext.Click += mnuFindNext_Click;
            // 
            // mnuFindPrev
            // 
            mnuFindPrev.Name = "mnuFindPrev";
            mnuFindPrev.ShortcutKeys = Keys.Shift | Keys.F3;
            mnuFindPrev.Size = new Size(377, 26);
            mnuFindPrev.Text = "Occurrence &précédente";
            mnuFindPrev.Click += mnuFindPrev_Click;
            // 
            // mnuReplace
            // 
            mnuReplace.Name = "mnuReplace";
            mnuReplace.ShortcutKeys = Keys.Control | Keys.H;
            mnuReplace.Size = new Size(377, 26);
            mnuReplace.Text = "Re&mplacer…";
            mnuReplace.Click += mnuReplace_Click;
            // 
            // sepEdit3
            // 
            sepEdit3.Name = "sepEdit3";
            sepEdit3.Size = new Size(374, 6);
            // 
            // mnuGoTo
            // 
            mnuGoTo.Name = "mnuGoTo";
            mnuGoTo.ShortcutKeys = Keys.Control | Keys.G;
            mnuGoTo.Size = new Size(377, 26);
            mnuGoTo.Text = "Aller à la &ligne…";
            mnuGoTo.Click += mnuGoTo_Click;
            // 
            // mnuReindent
            // 
            mnuReindent.Name = "mnuReindent";
            mnuReindent.ShortcutKeys = Keys.Control | Keys.Shift | Keys.I;
            mnuReindent.Size = new Size(377, 26);
            mnuReindent.Text = "&Réindenter le code Forth";
            mnuReindent.Click += mnuReindent_Click;
            // 
            // mnuView
            // 
            mnuView.DropDownItems.AddRange(new ToolStripItem[] { mnuReloadSyntax, mnuEditSyntax, mnuOpenSyntaxFolder, sepView1, mnuTheme, mnuZoomIn, mnuZoomOut, mnuZoomReset, sepView2, mnuShowProject, mnuShowOutline, mnuShowOutput, mnuFold, sepView3, mnuRestoreSession });
            mnuView.Name = "mnuView";
            mnuView.Size = new Size(87, 24);
            mnuView.Text = "&Affichage";
            // 
            // mnuReloadSyntax
            // 
            mnuReloadSyntax.Name = "mnuReloadSyntax";
            mnuReloadSyntax.ShortcutKeys = Keys.Control | Keys.F5;
            mnuReloadSyntax.Size = new Size(389, 26);
            mnuReloadSyntax.Text = "&Recharger la coloration (syntax.json)";
            mnuReloadSyntax.Click += mnuReloadSyntax_Click;
            // 
            // mnuEditSyntax
            // 
            mnuEditSyntax.Name = "mnuEditSyntax";
            mnuEditSyntax.Size = new Size(389, 26);
            mnuEditSyntax.Text = "&Modifier syntax.json";
            mnuEditSyntax.Click += mnuEditSyntax_Click;
            // 
            // mnuOpenSyntaxFolder
            // 
            mnuOpenSyntaxFolder.Name = "mnuOpenSyntaxFolder";
            mnuOpenSyntaxFolder.Size = new Size(389, 26);
            mnuOpenSyntaxFolder.Text = "Ouvrir le dossier des &syntaxes";
            mnuOpenSyntaxFolder.Click += mnuOpenSyntaxFolder_Click;
            // 
            // sepView1
            // 
            sepView1.Name = "sepView1";
            sepView1.Size = new Size(386, 6);
            // 
            // mnuTheme
            // 
            mnuTheme.DropDownItems.AddRange(new ToolStripItem[] { mnuThemeDark, mnuThemeLight });
            mnuTheme.Name = "mnuTheme";
            mnuTheme.Size = new Size(389, 26);
            mnuTheme.Text = "&Thème";
            // 
            // mnuThemeDark
            // 
            mnuThemeDark.Name = "mnuThemeDark";
            mnuThemeDark.Size = new Size(144, 26);
            mnuThemeDark.Text = "&Sombre";
            mnuThemeDark.Click += mnuThemeDark_Click;
            // 
            // mnuThemeLight
            // 
            mnuThemeLight.Name = "mnuThemeLight";
            mnuThemeLight.Size = new Size(144, 26);
            mnuThemeLight.Text = "C&lair";
            mnuThemeLight.Click += mnuThemeLight_Click;
            // 
            // mnuZoomIn
            // 
            mnuZoomIn.Name = "mnuZoomIn";
            mnuZoomIn.ShortcutKeyDisplayString = "Ctrl++";
            mnuZoomIn.ShortcutKeys = Keys.Control | Keys.Oemplus;
            mnuZoomIn.Size = new Size(389, 26);
            mnuZoomIn.Text = "&Agrandir la police";
            mnuZoomIn.Click += mnuZoomIn_Click;
            // 
            // mnuZoomOut
            // 
            mnuZoomOut.Name = "mnuZoomOut";
            mnuZoomOut.ShortcutKeyDisplayString = "Ctrl+-";
            mnuZoomOut.ShortcutKeys = Keys.Control | Keys.OemMinus;
            mnuZoomOut.Size = new Size(389, 26);
            mnuZoomOut.Text = "&Diminuer la police";
            mnuZoomOut.Click += mnuZoomOut_Click;
            // 
            // mnuZoomReset
            // 
            mnuZoomReset.Name = "mnuZoomReset";
            mnuZoomReset.ShortcutKeys = Keys.Control | Keys.D0;
            mnuZoomReset.Size = new Size(389, 26);
            mnuZoomReset.Text = "Taille &normale";
            mnuZoomReset.Click += mnuZoomReset_Click;
            // 
            // sepView2
            // 
            sepView2.Name = "sepView2";
            sepView2.Size = new Size(386, 6);
            // 
            // mnuShowProject
            // 
            mnuShowProject.Name = "mnuShowProject";
            mnuShowProject.ShortcutKeys = Keys.F8;
            mnuShowProject.Size = new Size(389, 26);
            mnuShowProject.Text = "&Projet (dossier)";
            mnuShowProject.Click += mnuShowProject_Click;
            // 
            // mnuShowOutline
            // 
            mnuShowOutline.Name = "mnuShowOutline";
            mnuShowOutline.ShortcutKeys = Keys.F7;
            mnuShowOutline.Size = new Size(389, 26);
            mnuShowOutline.Text = "Plan des &définitions";
            mnuShowOutline.Click += mnuShowOutline_Click;
            // 
            // mnuShowOutput
            // 
            mnuShowOutput.Name = "mnuShowOutput";
            mnuShowOutput.ShortcutKeys = Keys.F6;
            mnuShowOutput.Size = new Size(389, 26);
            mnuShowOutput.Text = "Panneau de &sortie";
            mnuShowOutput.Click += mnuShowOutput_Click;
            // 
            // mnuFold
            // 
            mnuFold.DropDownItems.AddRange(new ToolStripItem[] { mnuFoldToggle, mnuFoldAll, mnuUnfoldAll });
            mnuFold.Name = "mnuFold";
            mnuFold.Size = new Size(389, 26);
            mnuFold.Text = "&Repli du code";
            // 
            // mnuFoldToggle
            // 
            mnuFoldToggle.Name = "mnuFoldToggle";
            mnuFoldToggle.ShortcutKeys = Keys.Control | Keys.Shift | Keys.F;
            mnuFoldToggle.Size = new Size(428, 26);
            mnuFoldToggle.Text = "&Replier / déplier le bloc courant";
            mnuFoldToggle.Click += mnuFoldToggle_Click;
            // 
            // mnuFoldAll
            // 
            mnuFoldAll.Name = "mnuFoldAll";
            mnuFoldAll.Size = new Size(428, 26);
            mnuFoldAll.Text = "Tout &replier";
            mnuFoldAll.Click += mnuFoldAll_Click;
            // 
            // mnuUnfoldAll
            // 
            mnuUnfoldAll.Name = "mnuUnfoldAll";
            mnuUnfoldAll.Size = new Size(428, 26);
            mnuUnfoldAll.Text = "Tout &déplier";
            mnuUnfoldAll.Click += mnuUnfoldAll_Click;
            // 
            // sepView3
            // 
            sepView3.Name = "sepView3";
            sepView3.Size = new Size(386, 6);
            // 
            // mnuRestoreSession
            // 
            mnuRestoreSession.CheckOnClick = true;
            mnuRestoreSession.Name = "mnuRestoreSession";
            mnuRestoreSession.Size = new Size(389, 26);
            mnuRestoreSession.Text = "Restaurer la &session au démarrage";
            mnuRestoreSession.Click += mnuRestoreSession_Click;
            // 
            // mnuRunMenu
            // 
            mnuRunMenu.DropDownItems.AddRange(new ToolStripItem[] { mnuRun, mnuStop, mnuClearOutput });
            mnuRunMenu.Name = "mnuRunMenu";
            mnuRunMenu.Size = new Size(87, 24);
            mnuRunMenu.Text = "&Exécution";
            // 
            // mnuRun
            // 
            mnuRun.Name = "mnuRun";
            mnuRun.ShortcutKeys = Keys.F5;
            mnuRun.Size = new Size(238, 26);
            mnuRun.Text = "&Exécuter le fichier";
            mnuRun.Click += mnuRun_Click;
            // 
            // mnuStop
            // 
            mnuStop.Name = "mnuStop";
            mnuStop.ShortcutKeys = Keys.Shift | Keys.F5;
            mnuStop.Size = new Size(238, 26);
            mnuStop.Text = "&Arrêter";
            mnuStop.Click += mnuStop_Click;
            // 
            // mnuClearOutput
            // 
            mnuClearOutput.Name = "mnuClearOutput";
            mnuClearOutput.Size = new Size(238, 26);
            mnuClearOutput.Text = "E&ffacer la sortie";
            mnuClearOutput.Click += mnuClearOutput_Click;
            // 
            // mnuToolsMenu
            // 
            mnuToolsMenu.DropDownItems.AddRange(new ToolStripItem[] { mnuTerminal });
            mnuToolsMenu.Name = "mnuToolsMenu";
            mnuToolsMenu.Size = new Size(61, 24);
            mnuToolsMenu.Text = "&Outils";
            // 
            // mnuTerminal
            // 
            mnuTerminal.Name = "mnuTerminal";
            mnuTerminal.ShortcutKeys = Keys.Control | Keys.Shift | Keys.T;
            mnuTerminal.Size = new Size(321, 26);
            mnuTerminal.Text = "Terminal &série…";
            mnuTerminal.Click += mnuTerminal_Click;
            // 
            // mnuSettingsMenu
            // 
            mnuSettingsMenu.DropDownItems.AddRange(new ToolStripItem[] { mnuForthSettings, mnuAutoComplete });
            mnuSettingsMenu.Name = "mnuSettingsMenu";
            mnuSettingsMenu.Size = new Size(96, 24);
            mnuSettingsMenu.Text = "&Paramètres";
            // 
            // mnuForthSettings
            // 
            mnuForthSettings.Name = "mnuForthSettings";
            mnuForthSettings.Size = new Size(259, 26);
            mnuForthSettings.Text = "Interpréteur &Forth…";
            mnuForthSettings.Click += mnuForthSettings_Click;
            // 
            // mnuAutoComplete
            // 
            mnuAutoComplete.Checked = true;
            mnuAutoComplete.CheckOnClick = true;
            mnuAutoComplete.CheckState = CheckState.Checked;
            mnuAutoComplete.Name = "mnuAutoComplete";
            mnuAutoComplete.Size = new Size(259, 26);
            mnuAutoComplete.Text = "&Complétion automatique";
            mnuAutoComplete.Click += mnuAutoComplete_Click;
            // 
            // aideToolStripMenuItem
            // 
            aideToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { afficherAideToolStripMenuItem, mnuPdfOpen, mnuPdfFolder, toolStripMenuItem1, aProposToolStripMenuItem });
            aideToolStripMenuItem.Name = "aideToolStripMenuItem";
            aideToolStripMenuItem.Size = new Size(54, 24);
            aideToolStripMenuItem.Text = "Aide";
            // 
            // afficherAideToolStripMenuItem
            // 
            afficherAideToolStripMenuItem.Name = "afficherAideToolStripMenuItem";
            afficherAideToolStripMenuItem.ShortcutKeys = Keys.F1;
            afficherAideToolStripMenuItem.Size = new Size(249, 26);
            afficherAideToolStripMenuItem.Text = "Documentation PDF";
            afficherAideToolStripMenuItem.Click += mnuAfficherAide_Click;
            // 
            // mnuPdfOpen
            // 
            mnuPdfOpen.Name = "mnuPdfOpen";
            mnuPdfOpen.Size = new Size(249, 26);
            mnuPdfOpen.Text = "Ouvrir un PDF…";
            mnuPdfOpen.Click += mnuPdfOpen_Click;
            // 
            // mnuPdfFolder
            // 
            mnuPdfFolder.Name = "mnuPdfFolder";
            mnuPdfFolder.Size = new Size(249, 26);
            mnuPdfFolder.Text = "Dossier des PDF…";
            mnuPdfFolder.Click += mnuPdfFolder_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(246, 6);
            // 
            // aProposToolStripMenuItem
            // 
            aProposToolStripMenuItem.Name = "aProposToolStripMenuItem";
            aProposToolStripMenuItem.Size = new Size(249, 26);
            aProposToolStripMenuItem.Text = "A Propos";
            aProposToolStripMenuItem.Click += mnuApropos_Click;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { lblSpring, lblPos, lblSel, lblLines, lblEol });
            statusStrip.Location = new Point(0, 938);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 16, 0);
            statusStrip.Size = new Size(1257, 22);
            statusStrip.TabIndex = 2;
            // 
            // lblSpring
            // 
            lblSpring.Name = "lblSpring";
            lblSpring.Size = new Size(810, 16);
            lblSpring.Spring = true;
            // 
            // lblPos
            // 
            lblPos.AutoSize = false;
            lblPos.Name = "lblPos";
            lblPos.Size = new Size(130, 16);
            lblPos.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSel
            // 
            lblSel.AutoSize = false;
            lblSel.Name = "lblSel";
            lblSel.Size = new Size(90, 16);
            lblSel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblLines
            // 
            lblLines.AutoSize = false;
            lblLines.Name = "lblLines";
            lblLines.Size = new Size(100, 16);
            lblLines.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEol
            // 
            lblEol.AutoSize = false;
            lblEol.Name = "lblEol";
            lblEol.Size = new Size(110, 16);
            lblEol.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ctxEditor
            // 
            ctxEditor.ImageScalingSize = new Size(20, 20);
            ctxEditor.Items.AddRange(new ToolStripItem[] { ctxCut, ctxCopy, ctxPaste, sepCtx, ctxSelectAll, sepCtx2, ctxReindent });
            ctxEditor.Name = "ctxEditor";
            ctxEditor.Size = new Size(204, 136);
            ctxEditor.Opening += ctxEditor_Opening;
            // 
            // ctxCut
            // 
            ctxCut.Name = "ctxCut";
            ctxCut.Size = new Size(203, 24);
            ctxCut.Text = "Couper";
            ctxCut.Click += Cut_Click;
            // 
            // ctxCopy
            // 
            ctxCopy.Name = "ctxCopy";
            ctxCopy.Size = new Size(203, 24);
            ctxCopy.Text = "Copier";
            ctxCopy.Click += Copy_Click;
            // 
            // ctxPaste
            // 
            ctxPaste.Name = "ctxPaste";
            ctxPaste.Size = new Size(203, 24);
            ctxPaste.Text = "Coller";
            ctxPaste.Click += Paste_Click;
            // 
            // sepCtx
            // 
            sepCtx.Name = "sepCtx";
            sepCtx.Size = new Size(200, 6);
            // 
            // ctxSelectAll
            // 
            ctxSelectAll.Name = "ctxSelectAll";
            ctxSelectAll.Size = new Size(203, 24);
            ctxSelectAll.Text = "Tout sélectionner";
            ctxSelectAll.Click += SelectAll_Click;
            // 
            // sepCtx2
            // 
            sepCtx2.Name = "sepCtx2";
            sepCtx2.Size = new Size(200, 6);
            // 
            // ctxReindent
            // 
            ctxReindent.Name = "ctxReindent";
            ctxReindent.Size = new Size(203, 24);
            ctxReindent.Text = "Réindenter le code";
            ctxReindent.Click += ctxReindent_Click;
            // 
            // splitOuter
            // 
            splitOuter.Dock = DockStyle.Fill;
            splitOuter.FixedPanel = FixedPanel.Panel2;
            splitOuter.Location = new Point(0, 0);
            splitOuter.Margin = new Padding(3, 4, 3, 4);
            splitOuter.Name = "splitOuter";
            splitOuter.Orientation = Orientation.Horizontal;
            // 
            // splitOuter.Panel1
            // 
            splitOuter.Panel1.Controls.Add(splitInner);
            // 
            // splitOuter.Panel2
            // 
            splitOuter.Panel2.Controls.Add(pnlOutput);
            splitOuter.Size = new Size(1001, 908);
            splitOuter.SplitterDistance = 624;
            splitOuter.SplitterWidth = 5;
            splitOuter.TabIndex = 0;
            // 
            // splitInner
            // 
            splitInner.Dock = DockStyle.Fill;
            splitInner.FixedPanel = FixedPanel.Panel2;
            splitInner.Location = new Point(0, 0);
            splitInner.Margin = new Padding(3, 4, 3, 4);
            splitInner.Name = "splitInner";
            // 
            // splitInner.Panel1
            // 
            splitInner.Panel1.Controls.Add(tabs);
            // 
            // splitInner.Panel2
            // 
            splitInner.Panel2.Controls.Add(pnlOutline);
            splitInner.Panel2MinSize = 120;
            splitInner.Size = new Size(1001, 624);
            splitInner.SplitterDistance = 700;
            splitInner.SplitterWidth = 5;
            splitInner.TabIndex = 0;
            // 
            // tabs
            // 
            tabs.AllowDrop = true;
            tabs.Dock = DockStyle.Fill;
            tabs.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabs.ItemSize = new Size(180, 26);
            tabs.Location = new Point(0, 0);
            tabs.Margin = new Padding(3, 4, 3, 4);
            tabs.Name = "tabs";
            tabs.Padding = new Point(12, 4);
            tabs.SelectedIndex = 0;
            tabs.ShowToolTips = true;
            tabs.Size = new Size(700, 624);
            tabs.SizeMode = TabSizeMode.Fixed;
            tabs.TabIndex = 0;
            tabs.DrawItem += tabs_DrawItem;
            tabs.SelectedIndexChanged += tabs_SelectedIndexChanged;
            tabs.DragDrop += MainForm_DragDrop;
            tabs.DragEnter += MainForm_DragEnter;
            tabs.MouseDoubleClick += tabs_MouseDoubleClick;
            tabs.MouseDown += tabs_MouseDown;
            // 
            // pnlOutline
            // 
            pnlOutline.Controls.Add(lstOutline);
            pnlOutline.Controls.Add(txtOutlineFilter);
            pnlOutline.Controls.Add(chkOutlineDefsOnly);
            pnlOutline.Controls.Add(lblOutline);
            pnlOutline.Dock = DockStyle.Fill;
            pnlOutline.Location = new Point(0, 0);
            pnlOutline.Margin = new Padding(3, 4, 3, 4);
            pnlOutline.Name = "pnlOutline";
            pnlOutline.Size = new Size(296, 624);
            pnlOutline.TabIndex = 0;
            // 
            // lstOutline
            // 
            lstOutline.Dock = DockStyle.Fill;
            lstOutline.FormattingEnabled = true;
            lstOutline.HorizontalScrollbar = true;
            lstOutline.IntegralHeight = false;
            lstOutline.Location = new Point(0, 85);
            lstOutline.Margin = new Padding(3, 4, 3, 4);
            lstOutline.Name = "lstOutline";
            lstOutline.Size = new Size(296, 539);
            lstOutline.TabIndex = 2;
            lstOutline.MouseClick += lstOutline_MouseClick;
            lstOutline.KeyDown += lstOutline_KeyDown;
            // 
            // txtOutlineFilter
            // 
            txtOutlineFilter.Dock = DockStyle.Top;
            txtOutlineFilter.Location = new Point(0, 58);
            txtOutlineFilter.Margin = new Padding(3, 4, 3, 4);
            txtOutlineFilter.Name = "txtOutlineFilter";
            txtOutlineFilter.PlaceholderText = "Filtrer…";
            txtOutlineFilter.Size = new Size(296, 27);
            txtOutlineFilter.TabIndex = 1;
            txtOutlineFilter.TextChanged += txtOutlineFilter_TextChanged;
            // 
            // chkOutlineDefsOnly
            // 
            chkOutlineDefsOnly.Dock = DockStyle.Top;
            chkOutlineDefsOnly.Location = new Point(0, 29);
            chkOutlineDefsOnly.Margin = new Padding(3, 4, 3, 4);
            chkOutlineDefsOnly.Name = "chkOutlineDefsOnly";
            chkOutlineDefsOnly.Padding = new Padding(7, 0, 0, 0);
            chkOutlineDefsOnly.Size = new Size(296, 29);
            chkOutlineDefsOnly.TabIndex = 3;
            chkOutlineDefsOnly.Text = "Seulement les « : »";
            chkOutlineDefsOnly.UseVisualStyleBackColor = true;
            chkOutlineDefsOnly.CheckedChanged += chkOutlineDefsOnly_CheckedChanged;
            // 
            // lblOutline
            // 
            lblOutline.Dock = DockStyle.Top;
            lblOutline.Location = new Point(0, 0);
            lblOutline.Name = "lblOutline";
            lblOutline.Size = new Size(296, 29);
            lblOutline.TabIndex = 0;
            lblOutline.Text = " Définitions";
            lblOutline.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlOutput
            // 
            pnlOutput.Controls.Add(rtbOutput);
            pnlOutput.Controls.Add(txtStdin);
            pnlOutput.Controls.Add(pnlOutputBar);
            pnlOutput.Dock = DockStyle.Fill;
            pnlOutput.Location = new Point(0, 0);
            pnlOutput.Margin = new Padding(3, 4, 3, 4);
            pnlOutput.Name = "pnlOutput";
            pnlOutput.Size = new Size(1001, 279);
            pnlOutput.TabIndex = 0;
            // 
            // rtbOutput
            // 
            rtbOutput.DetectUrls = false;
            rtbOutput.Dock = DockStyle.Fill;
            rtbOutput.Font = new Font("Consolas", 10F);
            rtbOutput.HideSelection = false;
            rtbOutput.Location = new Point(0, 40);
            rtbOutput.Margin = new Padding(3, 4, 3, 4);
            rtbOutput.Name = "rtbOutput";
            rtbOutput.ReadOnly = true;
            rtbOutput.Size = new Size(1001, 212);
            rtbOutput.TabIndex = 1;
            rtbOutput.Text = "";
            rtbOutput.WordWrap = false;
            rtbOutput.MouseDoubleClick += rtbOutput_MouseDoubleClick;
            // 
            // txtStdin
            // 
            txtStdin.Dock = DockStyle.Bottom;
            txtStdin.Font = new Font("Consolas", 10F);
            txtStdin.Location = new Point(0, 252);
            txtStdin.Margin = new Padding(3, 4, 3, 4);
            txtStdin.Name = "txtStdin";
            txtStdin.PlaceholderText = "Entrée standard du programme — Entrée pour envoyer une ligne";
            txtStdin.Size = new Size(1001, 27);
            txtStdin.TabIndex = 2;
            txtStdin.KeyDown += txtStdin_KeyDown;
            // 
            // pnlOutputBar
            // 
            pnlOutputBar.Controls.Add(btnHideOutput);
            pnlOutputBar.Controls.Add(lblOutputTitle);
            pnlOutputBar.Controls.Add(btnClearOutput);
            pnlOutputBar.Controls.Add(btnStop);
            pnlOutputBar.Controls.Add(btnRun);
            pnlOutputBar.Dock = DockStyle.Top;
            pnlOutputBar.Location = new Point(0, 0);
            pnlOutputBar.Margin = new Padding(3, 4, 3, 4);
            pnlOutputBar.Name = "pnlOutputBar";
            pnlOutputBar.Size = new Size(1001, 40);
            pnlOutputBar.TabIndex = 0;
            // 
            // btnHideOutput
            // 
            btnHideOutput.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnHideOutput.Location = new Point(965, 4);
            btnHideOutput.Margin = new Padding(3, 4, 3, 4);
            btnHideOutput.Name = "btnHideOutput";
            btnHideOutput.Size = new Size(32, 32);
            btnHideOutput.TabIndex = 0;
            btnHideOutput.Text = "✕";
            btnHideOutput.Click += btnHideOutput_Click;
            // 
            // lblOutputTitle
            // 
            lblOutputTitle.AutoSize = true;
            lblOutputTitle.Location = new Point(343, 9);
            lblOutputTitle.Name = "lblOutputTitle";
            lblOutputTitle.Size = new Size(414, 20);
            lblOutputTitle.TabIndex = 1;
            lblOutputTitle.Text = "Sortie du programme (double-clic sur une erreur pour y aller)";
            // 
            // btnClearOutput
            // 
            btnClearOutput.Location = new Point(231, 4);
            btnClearOutput.Margin = new Padding(3, 4, 3, 4);
            btnClearOutput.Name = "btnClearOutput";
            btnClearOutput.Size = new Size(91, 32);
            btnClearOutput.TabIndex = 2;
            btnClearOutput.Text = "Effacer";
            btnClearOutput.Click += mnuClearOutput_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(123, 4);
            btnStop.Margin = new Padding(3, 4, 3, 4);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(103, 32);
            btnStop.TabIndex = 3;
            btnStop.Text = "■ Arrêter";
            btnStop.Click += mnuStop_Click;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(5, 4);
            btnRun.Margin = new Padding(3, 4, 3, 4);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(114, 32);
            btnRun.TabIndex = 4;
            btnRun.Text = "▶ Exécuter";
            btnRun.Click += mnuRun_Click;
            // 
            // splitLeft
            // 
            splitLeft.Dock = DockStyle.Fill;
            splitLeft.FixedPanel = FixedPanel.Panel1;
            splitLeft.Location = new Point(0, 30);
            splitLeft.Margin = new Padding(3, 4, 3, 4);
            splitLeft.Name = "splitLeft";
            // 
            // splitLeft.Panel1
            // 
            splitLeft.Panel1.Controls.Add(pnlProject);
            splitLeft.Panel1MinSize = 120;
            // 
            // splitLeft.Panel2
            // 
            splitLeft.Panel2.Controls.Add(splitOuter);
            splitLeft.Size = new Size(1257, 908);
            splitLeft.SplitterDistance = 251;
            splitLeft.SplitterWidth = 5;
            splitLeft.TabIndex = 1;
            // 
            // pnlProject
            // 
            pnlProject.Controls.Add(tvProject);
            pnlProject.Controls.Add(txtProjectFilter);
            pnlProject.Controls.Add(cboProjectFilter);
            pnlProject.Controls.Add(pnlProjectBar);
            pnlProject.Controls.Add(lblProject);
            pnlProject.Dock = DockStyle.Fill;
            pnlProject.Location = new Point(0, 0);
            pnlProject.Margin = new Padding(3, 4, 3, 4);
            pnlProject.Name = "pnlProject";
            pnlProject.Size = new Size(251, 908);
            pnlProject.TabIndex = 0;
            // 
            // tvProject
            // 
            tvProject.BorderStyle = BorderStyle.None;
            tvProject.ContextMenuStrip = ctxProject;
            tvProject.Dock = DockStyle.Fill;
            tvProject.HideSelection = false;
            tvProject.Location = new Point(0, 120);
            tvProject.Margin = new Padding(3, 4, 3, 4);
            tvProject.Name = "tvProject";
            tvProject.Size = new Size(251, 788);
            tvProject.TabIndex = 4;
            tvProject.BeforeExpand += tvProject_BeforeExpand;
            tvProject.NodeMouseDoubleClick += tvProject_NodeMouseDoubleClick;
            tvProject.KeyDown += tvProject_KeyDown;
            tvProject.MouseDown += tvProject_MouseDown;
            // 
            // ctxProject
            // 
            ctxProject.ImageScalingSize = new Size(20, 20);
            ctxProject.Items.AddRange(new ToolStripItem[] { ctxProjOpen, ctxProjNewFile, sepProj, ctxProjExplorer, ctxProjRefresh });
            ctxProject.Name = "ctxProject";
            ctxProject.Size = new Size(253, 106);
            ctxProject.Opening += ctxProject_Opening;
            // 
            // ctxProjOpen
            // 
            ctxProjOpen.Name = "ctxProjOpen";
            ctxProjOpen.Size = new Size(252, 24);
            ctxProjOpen.Text = "Ouvrir";
            ctxProjOpen.Click += ctxProjOpen_Click;
            // 
            // ctxProjNewFile
            // 
            ctxProjNewFile.Name = "ctxProjNewFile";
            ctxProjNewFile.Size = new Size(252, 24);
            ctxProjNewFile.Text = "Nouveau fichier…";
            ctxProjNewFile.Click += ctxProjNewFile_Click;
            // 
            // sepProj
            // 
            sepProj.Name = "sepProj";
            sepProj.Size = new Size(249, 6);
            // 
            // ctxProjExplorer
            // 
            ctxProjExplorer.Name = "ctxProjExplorer";
            ctxProjExplorer.Size = new Size(252, 24);
            ctxProjExplorer.Text = "Afficher dans l'Explorateur";
            ctxProjExplorer.Click += ctxProjExplorer_Click;
            // 
            // ctxProjRefresh
            // 
            ctxProjRefresh.Name = "ctxProjRefresh";
            ctxProjRefresh.Size = new Size(252, 24);
            ctxProjRefresh.Text = "Actualiser";
            ctxProjRefresh.Click += ctxProjRefresh_Click;
            // 
            // txtProjectFilter
            // 
            txtProjectFilter.Dock = DockStyle.Top;
            txtProjectFilter.Location = new Point(0, 93);
            txtProjectFilter.Margin = new Padding(3, 4, 3, 4);
            txtProjectFilter.Name = "txtProjectFilter";
            txtProjectFilter.PlaceholderText = "Filtrer par nom…";
            txtProjectFilter.Size = new Size(251, 27);
            txtProjectFilter.TabIndex = 3;
            txtProjectFilter.TextChanged += txtProjectFilter_TextChanged;
            // 
            // cboProjectFilter
            // 
            cboProjectFilter.Dock = DockStyle.Top;
            cboProjectFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProjectFilter.FormattingEnabled = true;
            cboProjectFilter.Location = new Point(0, 65);
            cboProjectFilter.Margin = new Padding(3, 4, 3, 4);
            cboProjectFilter.Name = "cboProjectFilter";
            cboProjectFilter.Size = new Size(251, 28);
            cboProjectFilter.TabIndex = 2;
            cboProjectFilter.SelectionChangeCommitted += cboProjectFilter_SelectionChangeCommitted;
            // 
            // pnlProjectBar
            // 
            pnlProjectBar.Controls.Add(cboProjectFolder);
            pnlProjectBar.Controls.Add(btnProjectBrowse);
            pnlProjectBar.Dock = DockStyle.Top;
            pnlProjectBar.Location = new Point(0, 29);
            pnlProjectBar.Margin = new Padding(3, 4, 3, 4);
            pnlProjectBar.Name = "pnlProjectBar";
            pnlProjectBar.Size = new Size(251, 36);
            pnlProjectBar.TabIndex = 1;
            // 
            // cboProjectFolder
            // 
            cboProjectFolder.Dock = DockStyle.Fill;
            cboProjectFolder.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProjectFolder.FormattingEnabled = true;
            cboProjectFolder.Location = new Point(0, 0);
            cboProjectFolder.Margin = new Padding(3, 4, 3, 4);
            cboProjectFolder.Name = "cboProjectFolder";
            cboProjectFolder.Size = new Size(212, 28);
            cboProjectFolder.TabIndex = 0;
            cboProjectFolder.SelectionChangeCommitted += cboProjectFolder_SelectionChangeCommitted;
            // 
            // btnProjectBrowse
            // 
            btnProjectBrowse.Dock = DockStyle.Right;
            btnProjectBrowse.Location = new Point(212, 0);
            btnProjectBrowse.Margin = new Padding(3, 4, 3, 4);
            btnProjectBrowse.Name = "btnProjectBrowse";
            btnProjectBrowse.Size = new Size(39, 36);
            btnProjectBrowse.TabIndex = 1;
            btnProjectBrowse.Text = "…";
            btnProjectBrowse.UseVisualStyleBackColor = true;
            btnProjectBrowse.Click += btnProjectBrowse_Click;
            // 
            // lblProject
            // 
            lblProject.Dock = DockStyle.Top;
            lblProject.Location = new Point(0, 0);
            lblProject.Name = "lblProject";
            lblProject.Size = new Size(251, 29);
            lblProject.TabIndex = 0;
            lblProject.Text = " Projet (dossier)";
            lblProject.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1257, 960);
            Controls.Add(splitLeft);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Éditeur Forth";
            WindowState = FormWindowState.Maximized;
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            DragDrop += MainForm_DragDrop;
            DragEnter += MainForm_DragEnter;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ctxEditor.ResumeLayout(false);
            splitOuter.Panel1.ResumeLayout(false);
            splitOuter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitOuter).EndInit();
            splitOuter.ResumeLayout(false);
            splitInner.Panel1.ResumeLayout(false);
            splitInner.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitInner).EndInit();
            splitInner.ResumeLayout(false);
            pnlOutline.ResumeLayout(false);
            pnlOutline.PerformLayout();
            pnlOutput.ResumeLayout(false);
            pnlOutput.PerformLayout();
            pnlOutputBar.ResumeLayout(false);
            pnlOutputBar.PerformLayout();
            splitLeft.Panel1.ResumeLayout(false);
            splitLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitLeft).EndInit();
            splitLeft.ResumeLayout(false);
            pnlProject.ResumeLayout(false);
            pnlProject.PerformLayout();
            ctxProject.ResumeLayout(false);
            pnlProjectBar.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ContextMenuStrip ctxEditor;
        private System.Windows.Forms.SplitContainer splitOuter;
        private System.Windows.Forms.SplitContainer splitInner;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.Panel pnlOutline;
        private System.Windows.Forms.Label lblOutline;
        private System.Windows.Forms.TextBox txtOutlineFilter;
        private System.Windows.Forms.ListBox lstOutline;
        private System.Windows.Forms.Panel pnlOutput;
        private System.Windows.Forms.Panel pnlOutputBar;
        private System.Windows.Forms.Label lblOutputTitle;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnClearOutput;
        private System.Windows.Forms.Button btnHideOutput;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.TextBox txtStdin;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuView;
        private System.Windows.Forms.ToolStripMenuItem mnuRunMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuSettingsMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuTheme;
        private System.Windows.Forms.ToolStripMenuItem mnuFold;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuRecent;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
        private System.Windows.Forms.ToolStripMenuItem mnuPrint;
        private System.Windows.Forms.ToolStripMenuItem mnuClose;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuQuit;
        private System.Windows.Forms.ToolStripMenuItem mnuUndo;
        private System.Windows.Forms.ToolStripMenuItem mnuRedo;
        private System.Windows.Forms.ToolStripMenuItem mnuCut;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripMenuItem mnuPaste;
        private System.Windows.Forms.ToolStripMenuItem mnuSelectAll;
        private System.Windows.Forms.ToolStripMenuItem mnuFind;
        private System.Windows.Forms.ToolStripMenuItem mnuFindNext;
        private System.Windows.Forms.ToolStripMenuItem mnuFindPrev;
        private System.Windows.Forms.ToolStripMenuItem mnuReplace;
        private System.Windows.Forms.ToolStripMenuItem mnuGoTo;
        private System.Windows.Forms.ToolStripMenuItem mnuReloadSyntax;
        private System.Windows.Forms.ToolStripMenuItem mnuEditSyntax;
        private System.Windows.Forms.ToolStripMenuItem mnuThemeDark;
        private System.Windows.Forms.ToolStripMenuItem mnuThemeLight;
        private System.Windows.Forms.ToolStripMenuItem mnuZoomIn;
        private System.Windows.Forms.ToolStripMenuItem mnuZoomOut;
        private System.Windows.Forms.ToolStripMenuItem mnuZoomReset;
        private System.Windows.Forms.ToolStripMenuItem mnuShowOutline;
        private System.Windows.Forms.ToolStripMenuItem mnuShowOutput;
        private System.Windows.Forms.ToolStripMenuItem mnuFoldToggle;
        private System.Windows.Forms.ToolStripMenuItem mnuFoldAll;
        private System.Windows.Forms.ToolStripMenuItem mnuUnfoldAll;
        private System.Windows.Forms.ToolStripMenuItem mnuRestoreSession;
        private System.Windows.Forms.ToolStripMenuItem mnuRun;
        private System.Windows.Forms.ToolStripMenuItem mnuStop;
        private System.Windows.Forms.ToolStripMenuItem mnuClearOutput;
        private System.Windows.Forms.ToolStripMenuItem mnuForthSettings;
        private System.Windows.Forms.ToolStripMenuItem mnuAutoComplete;
        private System.Windows.Forms.ToolStripMenuItem ctxCut;
        private System.Windows.Forms.ToolStripMenuItem ctxCopy;
        private System.Windows.Forms.ToolStripMenuItem ctxPaste;
        private System.Windows.Forms.ToolStripMenuItem ctxSelectAll;
        private System.Windows.Forms.ToolStripSeparator sepFile1;
        private System.Windows.Forms.ToolStripSeparator sepFile2;
        private System.Windows.Forms.ToolStripSeparator sepFile3;
        private System.Windows.Forms.ToolStripSeparator sepFile4;
        private System.Windows.Forms.ToolStripSeparator sepEdit1;
        private System.Windows.Forms.ToolStripSeparator sepEdit2;
        private System.Windows.Forms.ToolStripSeparator sepEdit3;
        private System.Windows.Forms.ToolStripSeparator sepView1;
        private System.Windows.Forms.ToolStripSeparator sepView2;
        private System.Windows.Forms.ToolStripSeparator sepView3;
        private System.Windows.Forms.ToolStripSeparator sepCtx;
        private System.Windows.Forms.ToolStripStatusLabel lblSpring;
        private System.Windows.Forms.ToolStripStatusLabel lblPos;
        private System.Windows.Forms.ToolStripStatusLabel lblSel;
        private System.Windows.Forms.ToolStripStatusLabel lblLines;
        private System.Windows.Forms.ToolStripStatusLabel lblEol;
        private System.Windows.Forms.ToolStripMenuItem mnuToolsMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuTerminal;
        private System.Windows.Forms.ToolStripMenuItem mnuReindent;
        private System.Windows.Forms.ToolStripMenuItem ctxReindent;
        private System.Windows.Forms.ToolStripSeparator sepCtx2;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenSyntaxFolder;
        private System.Windows.Forms.SplitContainer splitLeft;
        private System.Windows.Forms.Panel pnlProject;
        private System.Windows.Forms.Label lblProject;
        private System.Windows.Forms.Panel pnlProjectBar;
        private System.Windows.Forms.ComboBox cboProjectFolder;
        private System.Windows.Forms.Button btnProjectBrowse;
        private System.Windows.Forms.ComboBox cboProjectFilter;
        private System.Windows.Forms.TextBox txtProjectFilter;
        private System.Windows.Forms.TreeView tvProject;
        private System.Windows.Forms.ContextMenuStrip ctxProject;
        private System.Windows.Forms.ToolStripMenuItem ctxProjOpen;
        private System.Windows.Forms.ToolStripMenuItem ctxProjNewFile;
        private System.Windows.Forms.ToolStripSeparator sepProj;
        private System.Windows.Forms.ToolStripMenuItem ctxProjExplorer;
        private System.Windows.Forms.ToolStripMenuItem ctxProjRefresh;
        private System.Windows.Forms.CheckBox chkOutlineDefsOnly;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenFolder;
        private System.Windows.Forms.ToolStripMenuItem mnuShowProject;
        private ToolStripMenuItem aideToolStripMenuItem;
        private ToolStripMenuItem afficherAideToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem aProposToolStripMenuItem;
        private ToolStripMenuItem mnuPdfOpen;
        private ToolStripMenuItem mnuPdfFolder;
    }
}
