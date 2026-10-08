namespace ForthEditor
{
    partial class TerminalForm
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
            menuStrip = new MenuStrip();
            mnuSettings = new ToolStripMenuItem();
            mnuPortSettings = new ToolStripMenuItem();
            mnuTextSettings = new ToolStripMenuItem();
            mnuSendSettings = new ToolStripMenuItem();
            sepSettings = new ToolStripSeparator();
            mnuSuppressEcho = new ToolStripMenuItem();
            topPanel = new FlowLayoutPanel();
            btnOpen = new Button();
            btnClose = new Button();
            btnSendFile = new Button();
            btnClear = new Button();
            txtTerminal = new RichTextBox();
            ctxTerminal = new ContextMenuStrip(components);
            ctxTermCopy = new ToolStripMenuItem();
            ctxTermCopyAll = new ToolStripMenuItem();
            ctxTermSelectAll = new ToolStripMenuItem();
            sepTerm1 = new ToolStripSeparator();
            ctxTermPaste = new ToolStripMenuItem();
            sepTerm2 = new ToolStripSeparator();
            ctxTermClear = new ToolStripMenuItem();
            lblStatus = new Label();
            menuStrip.SuspendLayout();
            topPanel.SuspendLayout();
            ctxTerminal.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { mnuSettings });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(7, 3, 0, 3);
            menuStrip.Size = new Size(953, 30);
            menuStrip.TabIndex = 0;
            // 
            // mnuSettings
            // 
            mnuSettings.DropDownItems.AddRange(new ToolStripItem[] { mnuPortSettings, mnuTextSettings, mnuSendSettings, sepSettings, mnuSuppressEcho });
            mnuSettings.Name = "mnuSettings";
            mnuSettings.Size = new Size(96, 24);
            mnuSettings.Text = "&Paramètres";
            // 
            // mnuPortSettings
            // 
            mnuPortSettings.Name = "mnuPortSettings";
            mnuPortSettings.Size = new Size(376, 26);
            mnuPortSettings.Text = "&Port série (vitesse, bits, parité, stop)…";
            mnuPortSettings.Click += mnuPortSettings_Click;
            // 
            // mnuTextSettings
            // 
            mnuTextSettings.Name = "mnuTextSettings";
            mnuTextSettings.Size = new Size(376, 26);
            mnuTextSettings.Text = "&Texte et couleurs (encodage, fin de ligne)…";
            mnuTextSettings.Click += mnuTextSettings_Click;
            // 
            // mnuSendSettings
            // 
            mnuSendSettings.Name = "mnuSendSettings";
            mnuSendSettings.Size = new Size(376, 26);
            mnuSendSettings.Text = "&Envoi de l'éditeur (pause, invite)…";
            mnuSendSettings.Click += mnuSendSettings_Click;
            // 
            // sepSettings
            // 
            sepSettings.Name = "sepSettings";
            sepSettings.Size = new Size(373, 6);
            // 
            // mnuSuppressEcho
            // 
            mnuSuppressEcho.CheckOnClick = true;
            mnuSuppressEcho.Name = "mnuSuppressEcho";
            mnuSuppressEcho.Size = new Size(376, 26);
            mnuSuppressEcho.Text = "Supprimer l'é&cho de la cible";
            mnuSuppressEcho.ToolTipText = "Masque le texte que la cible renvoie après chaque ligne envoyée (écho distant)";
            mnuSuppressEcho.Click += mnuSuppressEcho_Click;
            // 
            // topPanel
            // 
            topPanel.AutoSize = true;
            topPanel.Controls.Add(btnOpen);
            topPanel.Controls.Add(btnClose);
            topPanel.Controls.Add(btnSendFile);
            topPanel.Controls.Add(btnClear);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 30);
            topPanel.Margin = new Padding(3, 4, 3, 4);
            topPanel.Name = "topPanel";
            topPanel.Padding = new Padding(9, 8, 9, 8);
            topPanel.Size = new Size(953, 64);
            topPanel.TabIndex = 1;
            // 
            // btnOpen
            // 
            btnOpen.AutoSize = true;
            btnOpen.Location = new Point(12, 12);
            btnOpen.Margin = new Padding(3, 4, 3, 4);
            btnOpen.MinimumSize = new Size(103, 40);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(103, 40);
            btnOpen.TabIndex = 0;
            btnOpen.Text = "Ouvrir com";
            btnOpen.UseVisualStyleBackColor = true;
            btnOpen.Click += BtnOpen_Click;
            // 
            // btnClose
            // 
            btnClose.AutoSize = true;
            btnClose.Location = new Point(121, 12);
            btnClose.Margin = new Padding(3, 4, 3, 4);
            btnClose.MinimumSize = new Size(103, 40);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(103, 40);
            btnClose.TabIndex = 1;
            btnClose.Text = "Fermer com";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;
            // 
            // btnSendFile
            // 
            btnSendFile.AutoSize = true;
            btnSendFile.Location = new Point(254, 12);
            btnSendFile.Margin = new Padding(27, 4, 3, 4);
            btnSendFile.MinimumSize = new Size(149, 40);
            btnSendFile.Name = "btnSendFile";
            btnSendFile.Size = new Size(149, 40);
            btnSendFile.TabIndex = 2;
            btnSendFile.Text = "Envoyer code ";
            btnSendFile.UseVisualStyleBackColor = true;
            btnSendFile.Click += BtnSendFile_Click;
            // 
            // btnClear
            // 
            btnClear.AutoSize = true;
            btnClear.Location = new Point(433, 12);
            btnClear.Margin = new Padding(27, 4, 3, 4);
            btnClear.MinimumSize = new Size(149, 40);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(149, 40);
            btnClear.TabIndex = 3;
            btnClear.Text = "Effacer terminal";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += BtnClear_Click;
            // 
            // txtTerminal
            // 
            txtTerminal.AcceptsTab = true;
            txtTerminal.BackColor = Color.Black;
            txtTerminal.BorderStyle = BorderStyle.FixedSingle;
            txtTerminal.ContextMenuStrip = ctxTerminal;
            txtTerminal.Dock = DockStyle.Fill;
            txtTerminal.Font = new Font("Consolas", 11F);
            txtTerminal.ForeColor = Color.LimeGreen;
            txtTerminal.Location = new Point(0, 94);
            txtTerminal.Margin = new Padding(3, 4, 3, 4);
            txtTerminal.Name = "txtTerminal";
            txtTerminal.Size = new Size(953, 602);
            txtTerminal.TabIndex = 4;
            txtTerminal.Text = "";
            txtTerminal.KeyDown += TxtTerminal_KeyDown;
            // 
            // ctxTerminal
            // 
            ctxTerminal.ImageScalingSize = new Size(20, 20);
            ctxTerminal.Items.AddRange(new ToolStripItem[] { ctxTermCopy, ctxTermCopyAll, ctxTermSelectAll, sepTerm1, ctxTermPaste, sepTerm2, ctxTermClear });
            ctxTerminal.Name = "ctxTerminal";
            ctxTerminal.Size = new Size(244, 136);
            ctxTerminal.Opening += ctxTerminal_Opening;
            // 
            // ctxTermCopy
            // 
            ctxTermCopy.Name = "ctxTermCopy";
            ctxTermCopy.ShortcutKeyDisplayString = "Ctrl+C";
            ctxTermCopy.Size = new Size(243, 24);
            ctxTermCopy.Text = "Copier";
            ctxTermCopy.Click += ctxTermCopy_Click;
            // 
            // ctxTermCopyAll
            // 
            ctxTermCopyAll.Name = "ctxTermCopyAll";
            ctxTermCopyAll.Size = new Size(243, 24);
            ctxTermCopyAll.Text = "Copier tout";
            ctxTermCopyAll.Click += ctxTermCopyAll_Click;
            // 
            // ctxTermSelectAll
            // 
            ctxTermSelectAll.Name = "ctxTermSelectAll";
            ctxTermSelectAll.ShortcutKeyDisplayString = "Ctrl+A";
            ctxTermSelectAll.Size = new Size(243, 24);
            ctxTermSelectAll.Text = "Tout sélectionner";
            ctxTermSelectAll.Click += ctxTermSelectAll_Click;
            // 
            // sepTerm1
            // 
            sepTerm1.Name = "sepTerm1";
            sepTerm1.Size = new Size(240, 6);
            // 
            // ctxTermPaste
            // 
            ctxTermPaste.Name = "ctxTermPaste";
            ctxTermPaste.ShortcutKeyDisplayString = "Ctrl+V";
            ctxTermPaste.Size = new Size(243, 24);
            ctxTermPaste.Text = "Coller";
            ctxTermPaste.Click += ctxTermPaste_Click;
            // 
            // sepTerm2
            // 
            sepTerm2.Name = "sepTerm2";
            sepTerm2.Size = new Size(240, 6);
            // 
            // ctxTermClear
            // 
            ctxTermClear.Name = "ctxTermClear";
            ctxTermClear.Size = new Size(243, 24);
            ctxTermClear.Text = "Effacer l'affichage";
            ctxTermClear.Click += ctxTermClear_Click;
            // 
            // lblStatus
            // 
            lblStatus.BackColor = SystemColors.ControlLight;
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Location = new Point(0, 696);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(7, 0, 0, 0);
            lblStatus.Size = new Size(953, 35);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "Déconnecté";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TerminalForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(953, 731);
            Controls.Add(txtTerminal);
            Controls.Add(lblStatus);
            Controls.Add(topPanel);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(683, 518);
            Name = "TerminalForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Terminal série";
            FormClosing += TerminalForm_FormClosing;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            ctxTerminal.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuSettings;
        private System.Windows.Forms.ToolStripMenuItem mnuPortSettings;
        private System.Windows.Forms.ToolStripMenuItem mnuTextSettings;
        private System.Windows.Forms.ToolStripMenuItem mnuSendSettings;
        private System.Windows.Forms.ToolStripSeparator sepSettings;
        private System.Windows.Forms.ToolStripMenuItem mnuSuppressEcho;
        private System.Windows.Forms.FlowLayoutPanel topPanel;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RichTextBox txtTerminal;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ContextMenuStrip ctxTerminal;
        private System.Windows.Forms.ToolStripMenuItem ctxTermCopy;
        private System.Windows.Forms.ToolStripMenuItem ctxTermCopyAll;
        private System.Windows.Forms.ToolStripMenuItem ctxTermSelectAll;
        private System.Windows.Forms.ToolStripMenuItem ctxTermPaste;
        private System.Windows.Forms.ToolStripMenuItem ctxTermClear;
        private System.Windows.Forms.ToolStripSeparator sepTerm1;
        private System.Windows.Forms.ToolStripSeparator sepTerm2;
    }
}
