namespace ForthEditor
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblProfile = new System.Windows.Forms.Label();
            this.cboProfile = new System.Windows.Forms.ComboBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblExe = new System.Windows.Forms.Label();
            this.txtExe = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblArgs = new System.Windows.Forms.Label();
            this.txtArgs = new System.Windows.Forms.TextBox();
            this.lblHelp = new System.Windows.Forms.Label();
            this.lblTestArgs = new System.Windows.Forms.Label();
            this.txtTestArgs = new System.Windows.Forms.TextBox();
            this.chkSave = new System.Windows.Forms.CheckBox();
            this.chkClear = new System.Windows.Forms.CheckBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // lblProfile
            this.lblProfile.AutoSize = true;
            this.lblProfile.Location = new System.Drawing.Point(12, 16);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(86, 15);
            this.lblProfile.TabIndex = 20;
            this.lblProfile.Text = "Configuration :";
            // cboProfile
            this.cboProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProfile.FormattingEnabled = true;
            this.cboProfile.Location = new System.Drawing.Point(120, 12);
            this.cboProfile.Name = "cboProfile";
            this.cboProfile.Size = new System.Drawing.Size(206, 23);
            this.cboProfile.TabIndex = 0;
            this.cboProfile.SelectionChangeCommitted += new System.EventHandler(this.cboProfile_SelectionChangeCommitted);
            // btnNew
            this.btnNew.Location = new System.Drawing.Point(332, 11);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(72, 25);
            this.btnNew.TabIndex = 1;
            this.btnNew.Text = "Nouvelle…";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // btnRename
            this.btnRename.Location = new System.Drawing.Point(408, 11);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(70, 25);
            this.btnRename.TabIndex = 2;
            this.btnRename.Text = "Renommer";
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(482, 11);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(66, 25);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Supprimer";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // lblExe
            this.lblExe.AutoSize = true;
            this.lblExe.Location = new System.Drawing.Point(12, 55);
            this.lblExe.Name = "lblExe";
            this.lblExe.Size = new System.Drawing.Size(80, 15);
            this.lblExe.TabIndex = 21;
            this.lblExe.Text = "Interpréteur :";
            // txtExe
            this.txtExe.Location = new System.Drawing.Point(120, 52);
            this.txtExe.Name = "txtExe";
            this.txtExe.Size = new System.Drawing.Size(330, 23);
            this.txtExe.TabIndex = 4;
            // btnBrowse
            this.btnBrowse.Location = new System.Drawing.Point(456, 51);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(92, 25);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "Parcourir…";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // lblArgs
            this.lblArgs.AutoSize = true;
            this.lblArgs.Location = new System.Drawing.Point(12, 90);
            this.lblArgs.Name = "lblArgs";
            this.lblArgs.Size = new System.Drawing.Size(70, 15);
            this.lblArgs.TabIndex = 22;
            this.lblArgs.Text = "Arguments :";
            // txtArgs
            this.txtArgs.Location = new System.Drawing.Point(120, 87);
            this.txtArgs.Name = "txtArgs";
            this.txtArgs.Size = new System.Drawing.Size(428, 23);
            this.txtArgs.TabIndex = 6;
            // lblHelp
            this.lblHelp.ForeColor = System.Drawing.Color.DimGray;
            this.lblHelp.Location = new System.Drawing.Point(120, 115);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(428, 48);
            this.lblHelp.TabIndex = 23;
            this.lblHelp.Text = "Variables : {file} = fichier à exécuter, {dir} = son dossier, {name} = son nom.\nLe dossier courant du programme est celui du fichier. Un nom sans chemin est cherché dans le PATH.";
            // lblTestArgs
            this.lblTestArgs.AutoSize = true;
            this.lblTestArgs.Location = new System.Drawing.Point(12, 173);
            this.lblTestArgs.Name = "lblTestArgs";
            this.lblTestArgs.Size = new System.Drawing.Size(100, 15);
            this.lblTestArgs.TabIndex = 24;
            this.lblTestArgs.Text = "Arguments du test :";
            // txtTestArgs
            this.txtTestArgs.Location = new System.Drawing.Point(140, 170);
            this.txtTestArgs.Name = "txtTestArgs";
            this.txtTestArgs.PlaceholderText = "vide = lancer sans argument (ex. --version pour gforth)";
            this.txtTestArgs.Size = new System.Drawing.Size(408, 23);
            this.txtTestArgs.TabIndex = 7;
            // chkSave
            this.chkSave.AutoSize = true;
            this.chkSave.Location = new System.Drawing.Point(12, 205);
            this.chkSave.Name = "chkSave";
            this.chkSave.Size = new System.Drawing.Size(450, 19);
            this.chkSave.TabIndex = 8;
            this.chkSave.Text = "Enregistrer le fichier avant l'exécution (sinon un fichier temporaire est utilisé)";
            // chkClear
            this.chkClear.AutoSize = true;
            this.chkClear.Location = new System.Drawing.Point(12, 231);
            this.chkClear.Name = "chkClear";
            this.chkClear.Size = new System.Drawing.Size(266, 19);
            this.chkClear.TabIndex = 9;
            this.chkClear.Text = "Effacer la sortie avant chaque exécution";
            // btnTest
            this.btnTest.Location = new System.Drawing.Point(12, 277);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(160, 28);
            this.btnTest.TabIndex = 10;
            this.btnTest.Text = "&Tester l'interpréteur";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // btnOk
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(378, 277);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 28);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "OK";
            // btnCancel
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(468, 277);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Annuler";
            // SettingsForm
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(560, 322);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.chkClear);
            this.Controls.Add(this.chkSave);
            this.Controls.Add(this.txtTestArgs);
            this.Controls.Add(this.lblTestArgs);
            this.Controls.Add(this.lblHelp);
            this.Controls.Add(this.txtArgs);
            this.Controls.Add(this.lblArgs);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtExe);
            this.Controls.Add(this.lblExe);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.cboProfile);
            this.Controls.Add(this.lblProfile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Interpréteur Forth";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.ComboBox cboProfile;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblExe;
        private System.Windows.Forms.TextBox txtExe;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblArgs;
        private System.Windows.Forms.TextBox txtArgs;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.Label lblTestArgs;
        private System.Windows.Forms.TextBox txtTestArgs;
        private System.Windows.Forms.CheckBox chkSave;
        private System.Windows.Forms.CheckBox chkClear;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}
