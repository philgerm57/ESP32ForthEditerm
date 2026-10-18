using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ForthEditor
{
    public partial class AproposForm : Form
    {
        private string _Version = "2.3"; 
        public AproposForm()
        {
            InitializeComponent();
            lblVersion.Text = _Version;
        }
    }
}
