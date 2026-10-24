namespace ForthEditor
{
    partial class PdfViewerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form
        private void InitializeComponent()
        {
            split = new SplitContainer();
            lstPdf = new ListBox();
            pnlTop = new Panel();
            cboRecent = new ComboBox();
            btnZoomOut = new Button();
            btnZoomIn = new Button();
            btnZoom100 = new Button();
            btnExplorer = new Button();
            btnOpenFile = new Button();
            btnFolder = new Button();
            txtFolder = new TextBox();
            lblInfo = new Label();
            web = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)split).BeginInit();
            split.Panel1.SuspendLayout();
            split.Panel2.SuspendLayout();
            split.SuspendLayout();
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)web).BeginInit();
            SuspendLayout();
            //
            // split
            //
            split.Dock = DockStyle.Fill;
            split.Name = "split";
            split.SplitterDistance = 240;
            split.Panel1.Controls.Add(lstPdf);
            split.Panel1.Controls.Add(pnlTop);
            split.Panel2.Controls.Add(web);
            //
            // pnlTop
            //
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 164;
            pnlTop.Name = "pnlTop";
            pnlTop.Controls.Add(lblInfo);
            pnlTop.Controls.Add(cboRecent);
            pnlTop.Controls.Add(btnZoomOut);
            pnlTop.Controls.Add(btnZoomIn);
            pnlTop.Controls.Add(btnZoom100);
            pnlTop.Controls.Add(btnExplorer);
            pnlTop.Controls.Add(btnOpenFile);
            pnlTop.Controls.Add(btnFolder);
            pnlTop.Controls.Add(txtFolder);
            //
            // txtFolder
            //
            txtFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtFolder.Location = new Point(4, 4);
            txtFolder.Name = "txtFolder";
            txtFolder.ReadOnly = true;
            txtFolder.Size = new Size(230, 27);
            //
            // btnFolder
            //
            btnFolder.Location = new Point(4, 34);
            btnFolder.Name = "btnFolder";
            btnFolder.Size = new Size(88, 28);
            btnFolder.Text = "Dossier…";
            btnFolder.UseVisualStyleBackColor = true;
            //
            // btnOpenFile
            //
            btnOpenFile.Location = new Point(96, 34);
            btnOpenFile.Name = "btnOpenFile";
            btnOpenFile.Size = new Size(88, 28);
            btnOpenFile.Text = "Fichier…";
            btnOpenFile.UseVisualStyleBackColor = true;
            //
            // btnExplorer
            //
            btnExplorer.Location = new Point(188, 34);
            btnExplorer.Name = "btnExplorer";
            btnExplorer.Size = new Size(46, 28);
            btnExplorer.Text = "📂";
            btnExplorer.UseVisualStyleBackColor = true;
            //
            // btnZoomOut / btnZoomIn / btnZoom100
            //
            btnZoomOut.Location = new Point(4, 66);
            btnZoomOut.Name = "btnZoomOut";
            btnZoomOut.Size = new Size(46, 28);
            btnZoomOut.Text = "−";
            btnZoomOut.UseVisualStyleBackColor = true;
            btnZoomIn.Location = new Point(54, 66);
            btnZoomIn.Name = "btnZoomIn";
            btnZoomIn.Size = new Size(46, 28);
            btnZoomIn.Text = "+";
            btnZoomIn.UseVisualStyleBackColor = true;
            btnZoom100.Location = new Point(104, 66);
            btnZoom100.Name = "btnZoom100";
            btnZoom100.Size = new Size(80, 28);
            btnZoom100.Text = "100 %";
            btnZoom100.UseVisualStyleBackColor = true;
            //
            // cboRecent
            //
            cboRecent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboRecent.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRecent.FormattingEnabled = true;
            cboRecent.Location = new Point(4, 100);
            cboRecent.Name = "cboRecent";
            cboRecent.Size = new Size(230, 28);
            //
            // lblInfo
            //
            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(6, 136);
            lblInfo.Name = "lblInfo";
            lblInfo.Text = "";
            //
            // lstPdf
            //
            lstPdf.Dock = DockStyle.Fill;
            lstPdf.FormattingEnabled = true;
            lstPdf.IntegralHeight = false;
            lstPdf.Name = "lstPdf";
            //
            // web
            //
            web.AllowExternalDrop = true;
            web.CreationProperties = null;
            web.DefaultBackgroundColor = Color.White;
            web.Dock = DockStyle.Fill;
            web.Name = "web";
            web.ZoomFactor = 1D;
            //
            // PdfViewerForm
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 740);
            Controls.Add(split);
            Name = "PdfViewerForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Documentation";
            ((System.ComponentModel.ISupportInitialize)web).EndInit();
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            split.Panel1.ResumeLayout(false);
            split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)split).EndInit();
            split.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion

        private SplitContainer split;
        private ListBox lstPdf;
        private Panel pnlTop;
        private ComboBox cboRecent;
        private Button btnZoomOut;
        private Button btnZoomIn;
        private Button btnZoom100;
        private Button btnExplorer;
        private Button btnOpenFile;
        private Button btnFolder;
        private TextBox txtFolder;
        private Label lblInfo;
        private Microsoft.Web.WebView2.WinForms.WebView2 web;
    }
}
