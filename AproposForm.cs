// ForthEditor — éditeur de code Forth et terminal série pour ESP32Forth
// Copyright (C) 2026 Germain PHILIPPE
// Licence : GNU General Public License v3.0 ou ultérieure — voir le fichier LICENSE.

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
        public AproposForm()
        {
            InitializeComponent();
            lblVersion.Text = AppInfo.Version;
            listBox1.Items.AddRange(new object[] {

                "Licence : GNU General Public License version 3 (GPLv3)",
                "",
                "Ce programme est un logiciel libre : vous pouvez le redistribuer et/ou le modifier",
                "selon les termes de la Licence Publique Générale GNU, version 3 ou ultérieure,",
                "publiée par la Free Software Foundation.",
                "",
                "Il est distribué dans l'espoir qu'il sera utile, mais SANS AUCUNE GARANTIE, sans même",
                "la garantie implicite de COMMERCIALISATION ou d'ADÉQUATION À UN USAGE PARTICULIER.",
                "Voir le fichier LICENSE fourni avec le programme, ou https://www.gnu.org/licenses/gpl-3.0.html",
                "",
                "Composants tiers :",
                "   PDF.js (Mozilla) — licence Apache 2.0",
                "   Microsoft Edge WebView2 — licence Microsoft",
                "   System.IO.Ports, .NET — licence MIT"
            });
        }
    }
}
