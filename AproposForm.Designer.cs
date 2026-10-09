namespace ForthEditor
{
    partial class AproposForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            listBox1 = new ListBox();
            lblVersion = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.DialogResult = DialogResult.Cancel;
            button1.Location = new Point(640, 398);
            button1.Name = "button1";
            button1.Size = new Size(124, 40);
            button1.TabIndex = 0;
            button1.Text = "Fermer";
            button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 410);
            label1.Name = "label1";
            label1.Size = new Size(353, 28);
            label1.TabIndex = 1;
            label1.Text = "(c) 2026 Germain PHILIPPE  Version:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 19);
            label2.Name = "label2";
            label2.Size = new Size(447, 28);
            label2.TabIndex = 2;
            label2.Text = "Editeur Forth Terminal série  pour ESP32Forth";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(19, 64);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(760, 304);
            listBox1.TabIndex = 3;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblVersion.Location = new Point(360, 410);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(41, 28);
            lblVersion.TabIndex = 4;
            lblVersion.Text = "2.2";
            // 
            // AproposForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblVersion);
            Controls.Add(listBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "AproposForm";
            Text = "A Propos";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private ListBox listBox1;
        private Label lblVersion;
    }
}