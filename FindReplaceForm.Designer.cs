namespace ForthEditor
{
    partial class FindReplaceForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblFind = new System.Windows.Forms.Label();
            this.lblReplace = new System.Windows.Forms.Label();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.chkCase = new System.Windows.Forms.CheckBox();
            this.chkWord = new System.Windows.Forms.CheckBox();
            this.chkRegex = new System.Windows.Forms.CheckBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // lblFind
            this.lblFind.AutoSize = true;
            this.lblFind.Location = new System.Drawing.Point(12, 14);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(72, 15);
            this.lblFind.TabIndex = 20;
            this.lblFind.Text = "Rechercher :";
            // lblReplace
            this.lblReplace.AutoSize = true;
            this.lblReplace.Location = new System.Drawing.Point(12, 44);
            this.lblReplace.Name = "lblReplace";
            this.lblReplace.Size = new System.Drawing.Size(93, 15);
            this.lblReplace.TabIndex = 21;
            this.lblReplace.Text = "Remplacer par :";
            // txtFind
            this.txtFind.Location = new System.Drawing.Point(120, 10);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(298, 23);
            this.txtFind.TabIndex = 0;
            // txtReplace
            this.txtReplace.Location = new System.Drawing.Point(120, 40);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(298, 23);
            this.txtReplace.TabIndex = 1;
            // chkCase
            this.chkCase.AutoSize = true;
            this.chkCase.Location = new System.Drawing.Point(12, 76);
            this.chkCase.Name = "chkCase";
            this.chkCase.Size = new System.Drawing.Size(132, 19);
            this.chkCase.TabIndex = 2;
            this.chkCase.Text = "Respecter la &casse";
            // chkWord
            this.chkWord.AutoSize = true;
            this.chkWord.Location = new System.Drawing.Point(12, 98);
            this.chkWord.Name = "chkWord";
            this.chkWord.Size = new System.Drawing.Size(268, 19);
            this.chkWord.TabIndex = 3;
            this.chkWord.Text = "Mot Forth &entier (délimité par des espaces)";
            // chkRegex
            this.chkRegex.AutoSize = true;
            this.chkRegex.Location = new System.Drawing.Point(12, 120);
            this.chkRegex.Name = "chkRegex";
            this.chkRegex.Size = new System.Drawing.Size(143, 19);
            this.chkRegex.TabIndex = 4;
            this.chkRegex.Text = "Expression &régulière";
            // btnNext
            this.btnNext.Location = new System.Drawing.Point(12, 150);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(98, 28);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = "&Suivant";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // btnPrev
            this.btnPrev.Location = new System.Drawing.Point(114, 150);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(98, 28);
            this.btnPrev.TabIndex = 6;
            this.btnPrev.Text = "&Précédent";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // btnReplace
            this.btnReplace.Location = new System.Drawing.Point(216, 150);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(98, 28);
            this.btnReplace.TabIndex = 7;
            this.btnReplace.Text = "Rem&placer";
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // btnReplaceAll
            this.btnReplaceAll.Location = new System.Drawing.Point(318, 150);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(98, 28);
            this.btnReplaceAll.TabIndex = 8;
            this.btnReplaceAll.Text = "&Tout remplacer";
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // btnClose
            this.btnClose.Location = new System.Drawing.Point(318, 192);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(98, 28);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Fermer";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // lblInfo
            this.lblInfo.ForeColor = System.Drawing.Color.DimGray;
            this.lblInfo.Location = new System.Drawing.Point(12, 192);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(290, 34);
            this.lblInfo.TabIndex = 22;
            // FindReplaceForm
            this.AcceptButton = this.btnNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(430, 235);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReplaceAll);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.chkRegex);
            this.Controls.Add(this.chkWord);
            this.Controls.Add(this.chkCase);
            this.Controls.Add(this.txtReplace);
            this.Controls.Add(this.txtFind);
            this.Controls.Add(this.lblReplace);
            this.Controls.Add(this.lblFind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FindReplaceForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rechercher / Remplacer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindReplaceForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblFind;
        private System.Windows.Forms.Label lblReplace;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.CheckBox chkCase;
        private System.Windows.Forms.CheckBox chkWord;
        private System.Windows.Forms.CheckBox chkRegex;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblInfo;
    }
}
