namespace ForthEditor
{
    partial class TerminalSettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblPort = new System.Windows.Forms.Label();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.cmbBaudRate = new System.Windows.Forms.ComboBox();
            this.lblDataBits = new System.Windows.Forms.Label();
            this.cmbDataBits = new System.Windows.Forms.ComboBox();
            this.lblParity = new System.Windows.Forms.Label();
            this.cmbParity = new System.Windows.Forms.ComboBox();
            this.lblStopBits = new System.Windows.Forms.Label();
            this.cmbStopBits = new System.Windows.Forms.ComboBox();
            this.lblConnected = new System.Windows.Forms.Label();
            this.lblEncoding = new System.Windows.Forms.Label();
            this.cmbEncoding = new System.Windows.Forms.ComboBox();
            this.lblEol = new System.Windows.Forms.Label();
            this.cmbEol = new System.Windows.Forms.ComboBox();
            this.lblBackColor = new System.Windows.Forms.Label();
            this.btnBackColor = new System.Windows.Forms.Button();
            this.lblForeColor = new System.Windows.Forms.Label();
            this.btnForeColor = new System.Windows.Forms.Button();
            this.lblSendMode = new System.Windows.Forms.Label();
            this.cmbSendMode = new System.Windows.Forms.ComboBox();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.txtPrompt = new System.Windows.Forms.TextBox();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.nudTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblDelay = new System.Windows.Forms.Label();
            this.nudDelay = new System.Windows.Forms.NumericUpDown();
            this.chkSkipComments = new System.Windows.Forms.CheckBox();
            this.lblSendHelp = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabPort = new System.Windows.Forms.TabPage();
            this.tabText = new System.Windows.Forms.TabPage();
            this.tabSend = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).BeginInit();
            this.tabs.SuspendLayout();
            this.tabPort.SuspendLayout();
            this.tabText.SuspendLayout();
            this.tabSend.SuspendLayout();
            this.SuspendLayout();
            // tabs
            this.tabs.Controls.Add(this.tabPort);
            this.tabs.Controls.Add(this.tabText);
            this.tabs.Controls.Add(this.tabSend);
            this.tabs.Location = new System.Drawing.Point(12, 12);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(418, 282);
            this.tabs.TabIndex = 0;
            // tabPort
            this.tabPort.Controls.Add(this.lblPort);
            this.tabPort.Controls.Add(this.cmbPort);
            this.tabPort.Controls.Add(this.btnRefresh);
            this.tabPort.Controls.Add(this.lblBaudRate);
            this.tabPort.Controls.Add(this.cmbBaudRate);
            this.tabPort.Controls.Add(this.lblDataBits);
            this.tabPort.Controls.Add(this.cmbDataBits);
            this.tabPort.Controls.Add(this.lblParity);
            this.tabPort.Controls.Add(this.cmbParity);
            this.tabPort.Controls.Add(this.lblStopBits);
            this.tabPort.Controls.Add(this.cmbStopBits);
            this.tabPort.Controls.Add(this.lblConnected);
            this.tabPort.Location = new System.Drawing.Point(4, 24);
            this.tabPort.Name = "tabPort";
            this.tabPort.Padding = new System.Windows.Forms.Padding(3);
            this.tabPort.Size = new System.Drawing.Size(410, 254);
            this.tabPort.TabIndex = 0;
            this.tabPort.Text = "Port série";
            this.tabPort.UseVisualStyleBackColor = true;
            // tabText
            this.tabText.Controls.Add(this.lblEncoding);
            this.tabText.Controls.Add(this.cmbEncoding);
            this.tabText.Controls.Add(this.lblEol);
            this.tabText.Controls.Add(this.cmbEol);
            this.tabText.Controls.Add(this.lblBackColor);
            this.tabText.Controls.Add(this.btnBackColor);
            this.tabText.Controls.Add(this.lblForeColor);
            this.tabText.Controls.Add(this.btnForeColor);
            this.tabText.Location = new System.Drawing.Point(4, 24);
            this.tabText.Name = "tabText";
            this.tabText.Padding = new System.Windows.Forms.Padding(3);
            this.tabText.Size = new System.Drawing.Size(410, 254);
            this.tabText.TabIndex = 1;
            this.tabText.Text = "Texte et couleurs";
            this.tabText.UseVisualStyleBackColor = true;
            // tabSend
            this.tabSend.Controls.Add(this.lblSendMode);
            this.tabSend.Controls.Add(this.cmbSendMode);
            this.tabSend.Controls.Add(this.lblPrompt);
            this.tabSend.Controls.Add(this.txtPrompt);
            this.tabSend.Controls.Add(this.lblTimeout);
            this.tabSend.Controls.Add(this.nudTimeout);
            this.tabSend.Controls.Add(this.lblDelay);
            this.tabSend.Controls.Add(this.nudDelay);
            this.tabSend.Controls.Add(this.chkSkipComments);
            this.tabSend.Controls.Add(this.lblSendHelp);
            this.tabSend.Location = new System.Drawing.Point(4, 24);
            this.tabSend.Name = "tabSend";
            this.tabSend.Padding = new System.Windows.Forms.Padding(3);
            this.tabSend.Size = new System.Drawing.Size(410, 254);
            this.tabSend.TabIndex = 2;
            this.tabSend.Text = "Envoi de l'éditeur";
            this.tabSend.UseVisualStyleBackColor = true;
            // lblPort
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(16, 24);
            this.lblPort.Text = "Port :";
            this.lblPort.Name = "lblPort";
            this.lblPort.TabIndex = 1;
            // cmbPort
            this.cmbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(170, 20);
            this.cmbPort.Size = new System.Drawing.Size(150, 23);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.TabIndex = 2;
            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(326, 19);
            this.btnRefresh.Size = new System.Drawing.Size(80, 25);
            this.btnRefresh.Text = "Rafraîchir";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.TabIndex = 3;
            // lblBaudRate
            this.lblBaudRate.AutoSize = true;
            this.lblBaudRate.Location = new System.Drawing.Point(16, 59);
            this.lblBaudRate.Text = "Vitesse (bauds) :";
            this.lblBaudRate.Name = "lblBaudRate";
            this.lblBaudRate.TabIndex = 4;
            // cmbBaudRate
            this.cmbBaudRate.FormattingEnabled = true;
            this.cmbBaudRate.Location = new System.Drawing.Point(170, 55);
            this.cmbBaudRate.Size = new System.Drawing.Size(150, 23);
            this.cmbBaudRate.Name = "cmbBaudRate";
            this.cmbBaudRate.TabIndex = 5;
            // lblDataBits
            this.lblDataBits.AutoSize = true;
            this.lblDataBits.Location = new System.Drawing.Point(16, 94);
            this.lblDataBits.Text = "Bits de données :";
            this.lblDataBits.Name = "lblDataBits";
            this.lblDataBits.TabIndex = 6;
            // cmbDataBits
            this.cmbDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataBits.FormattingEnabled = true;
            this.cmbDataBits.Location = new System.Drawing.Point(170, 90);
            this.cmbDataBits.Size = new System.Drawing.Size(150, 23);
            this.cmbDataBits.Name = "cmbDataBits";
            this.cmbDataBits.TabIndex = 7;
            // lblParity
            this.lblParity.AutoSize = true;
            this.lblParity.Location = new System.Drawing.Point(16, 129);
            this.lblParity.Text = "Parité :";
            this.lblParity.Name = "lblParity";
            this.lblParity.TabIndex = 8;
            // cmbParity
            this.cmbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParity.FormattingEnabled = true;
            this.cmbParity.Location = new System.Drawing.Point(170, 125);
            this.cmbParity.Size = new System.Drawing.Size(150, 23);
            this.cmbParity.Name = "cmbParity";
            this.cmbParity.TabIndex = 9;
            // lblStopBits
            this.lblStopBits.AutoSize = true;
            this.lblStopBits.Location = new System.Drawing.Point(16, 164);
            this.lblStopBits.Text = "Bits de stop :";
            this.lblStopBits.Name = "lblStopBits";
            this.lblStopBits.TabIndex = 10;
            // cmbStopBits
            this.cmbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStopBits.FormattingEnabled = true;
            this.cmbStopBits.Location = new System.Drawing.Point(170, 160);
            this.cmbStopBits.Size = new System.Drawing.Size(150, 23);
            this.cmbStopBits.Name = "cmbStopBits";
            this.cmbStopBits.TabIndex = 11;
            // lblConnected
            this.lblConnected.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblConnected.Location = new System.Drawing.Point(16, 200);
            this.lblConnected.Size = new System.Drawing.Size(390, 44);
            this.lblConnected.Text = "Le port est ouvert : fermez-le pour modifier le port, la vitesse, les bits, la parité, les bits de stop et l'encodage.";
            this.lblConnected.Visible = false;
            this.lblConnected.Name = "lblConnected";
            this.lblConnected.TabIndex = 12;
            // lblEncoding
            this.lblEncoding.AutoSize = true;
            this.lblEncoding.Location = new System.Drawing.Point(16, 24);
            this.lblEncoding.Text = "Encodage :";
            this.lblEncoding.Name = "lblEncoding";
            this.lblEncoding.TabIndex = 13;
            // cmbEncoding
            this.cmbEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEncoding.FormattingEnabled = true;
            this.cmbEncoding.Location = new System.Drawing.Point(170, 20);
            this.cmbEncoding.Size = new System.Drawing.Size(150, 23);
            this.cmbEncoding.Name = "cmbEncoding";
            this.cmbEncoding.TabIndex = 14;
            // lblEol
            this.lblEol.AutoSize = true;
            this.lblEol.Location = new System.Drawing.Point(16, 59);
            this.lblEol.Text = "Fin de ligne émise :";
            this.lblEol.Name = "lblEol";
            this.lblEol.TabIndex = 15;
            // cmbEol
            this.cmbEol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEol.FormattingEnabled = true;
            this.cmbEol.Location = new System.Drawing.Point(170, 55);
            this.cmbEol.Size = new System.Drawing.Size(150, 23);
            this.cmbEol.Name = "cmbEol";
            this.cmbEol.TabIndex = 16;
            // lblBackColor
            this.lblBackColor.AutoSize = true;
            this.lblBackColor.Location = new System.Drawing.Point(16, 98);
            this.lblBackColor.Text = "Couleur de fond :";
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.TabIndex = 17;
            // btnBackColor
            this.btnBackColor.Location = new System.Drawing.Point(170, 92);
            this.btnBackColor.Size = new System.Drawing.Size(60, 26);
            this.btnBackColor.Text = "";
            this.btnBackColor.UseVisualStyleBackColor = true;
            this.btnBackColor.Click += new System.EventHandler(this.btnBackColor_Click);
            this.btnBackColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBackColor.Name = "btnBackColor";
            this.btnBackColor.TabIndex = 18;
            // lblForeColor
            this.lblForeColor.AutoSize = true;
            this.lblForeColor.Location = new System.Drawing.Point(16, 133);
            this.lblForeColor.Text = "Couleur du texte :";
            this.lblForeColor.Name = "lblForeColor";
            this.lblForeColor.TabIndex = 19;
            // btnForeColor
            this.btnForeColor.Location = new System.Drawing.Point(170, 127);
            this.btnForeColor.Size = new System.Drawing.Size(60, 26);
            this.btnForeColor.Text = "";
            this.btnForeColor.UseVisualStyleBackColor = true;
            this.btnForeColor.Click += new System.EventHandler(this.btnForeColor_Click);
            this.btnForeColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnForeColor.Name = "btnForeColor";
            this.btnForeColor.TabIndex = 20;
            // lblSendMode
            this.lblSendMode.AutoSize = true;
            this.lblSendMode.Location = new System.Drawing.Point(16, 24);
            this.lblSendMode.Text = "Envoi de l'éditeur :";
            this.lblSendMode.Name = "lblSendMode";
            this.lblSendMode.TabIndex = 21;
            // cmbSendMode
            this.cmbSendMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSendMode.FormattingEnabled = true;
            this.cmbSendMode.Location = new System.Drawing.Point(170, 20);
            this.cmbSendMode.Size = new System.Drawing.Size(180, 23);
            this.cmbSendMode.SelectedIndexChanged += new System.EventHandler(this.cmbSendMode_SelectedIndexChanged);
            this.cmbSendMode.Name = "cmbSendMode";
            this.cmbSendMode.TabIndex = 22;
            // lblPrompt
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(16, 59);
            this.lblPrompt.Text = "Invite attendue :";
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.TabIndex = 23;
            // txtPrompt
            this.txtPrompt.Location = new System.Drawing.Point(170, 55);
            this.txtPrompt.Size = new System.Drawing.Size(80, 23);
            this.txtPrompt.Text = "ok";
            this.txtPrompt.Name = "txtPrompt";
            this.txtPrompt.TabIndex = 24;
            // lblTimeout
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(16, 94);
            this.lblTimeout.Text = "Délai max. (ms) :";
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.TabIndex = 25;
            // nudTimeout
            this.nudTimeout.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudTimeout.Location = new System.Drawing.Point(170, 90);
            this.nudTimeout.Maximum = new decimal(new int[] { 60000, 0, 0, 0 });
            this.nudTimeout.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudTimeout.Size = new System.Drawing.Size(90, 23);
            this.nudTimeout.Value = new decimal(new int[] { 3000, 0, 0, 0 });
            this.nudTimeout.Name = "nudTimeout";
            this.nudTimeout.TabIndex = 26;
            // lblDelay
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(16, 129);
            this.lblDelay.Text = "Pause par ligne (ms) :";
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.TabIndex = 27;
            // nudDelay
            this.nudDelay.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            this.nudDelay.Location = new System.Drawing.Point(170, 125);
            this.nudDelay.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            this.nudDelay.Size = new System.Drawing.Size(90, 23);
            this.nudDelay.Value = new decimal(new int[] { 30, 0, 0, 0 });
            this.nudDelay.Name = "nudDelay";
            this.nudDelay.TabIndex = 28;
            // chkSkipComments
            this.chkSkipComments.AutoSize = true;
            this.chkSkipComments.Checked = true;
            this.chkSkipComments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSkipComments.Location = new System.Drawing.Point(16, 165);
            this.chkSkipComments.Text = "Ignorer les lignes vides et les commentaires \\";
            this.chkSkipComments.UseVisualStyleBackColor = true;
            this.chkSkipComments.Name = "chkSkipComments";
            this.chkSkipComments.TabIndex = 29;
            // lblSendHelp
            this.lblSendHelp.ForeColor = System.Drawing.Color.DimGray;
            this.lblSendHelp.Location = new System.Drawing.Point(16, 198);
            this.lblSendHelp.Size = new System.Drawing.Size(390, 50);
            this.lblSendHelp.Text = "Pause fixe : N ms entre les lignes. Attendre l'invite : chaque ligne n'est envoyée qu'après la réponse de la cible (ex. « ok »).";
            this.lblSendHelp.Name = "lblSendHelp";
            this.lblSendHelp.TabIndex = 30;
            // btnOk
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(264, 304);
            this.btnOk.Size = new System.Drawing.Size(80, 28);
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.btnOk.Name = "btnOk";
            this.btnOk.TabIndex = 31;
            // btnCancel
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(350, 304);
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.Text = "Annuler";
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 32;
            // TerminalSettingsForm
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(442, 344);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TerminalSettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Paramètres du terminal série";
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).EndInit();
            this.tabs.ResumeLayout(false);
            this.tabPort.ResumeLayout(false);
            this.tabPort.PerformLayout();
            this.tabText.ResumeLayout(false);
            this.tabText.PerformLayout();
            this.tabSend.ResumeLayout(false);
            this.tabSend.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblBaudRate;
        private System.Windows.Forms.ComboBox cmbBaudRate;
        private System.Windows.Forms.Label lblDataBits;
        private System.Windows.Forms.ComboBox cmbDataBits;
        private System.Windows.Forms.Label lblParity;
        private System.Windows.Forms.ComboBox cmbParity;
        private System.Windows.Forms.Label lblStopBits;
        private System.Windows.Forms.ComboBox cmbStopBits;
        private System.Windows.Forms.Label lblConnected;
        private System.Windows.Forms.Label lblEncoding;
        private System.Windows.Forms.ComboBox cmbEncoding;
        private System.Windows.Forms.Label lblEol;
        private System.Windows.Forms.ComboBox cmbEol;
        private System.Windows.Forms.Label lblBackColor;
        private System.Windows.Forms.Button btnBackColor;
        private System.Windows.Forms.Label lblForeColor;
        private System.Windows.Forms.Button btnForeColor;
        private System.Windows.Forms.Label lblSendMode;
        private System.Windows.Forms.ComboBox cmbSendMode;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.TextBox txtPrompt;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.NumericUpDown nudTimeout;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.NumericUpDown nudDelay;
        private System.Windows.Forms.CheckBox chkSkipComments;
        private System.Windows.Forms.Label lblSendHelp;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabPort;
        private System.Windows.Forms.TabPage tabText;
        private System.Windows.Forms.TabPage tabSend;
    }
}
