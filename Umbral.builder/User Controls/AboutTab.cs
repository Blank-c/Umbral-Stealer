﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Umbral.builder.User_Controls
{
    public partial class AboutTab : UserControl
    {
        public AboutTab()
        {
            InitializeComponent();
        }

        private void githubButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Blank-c/Umbral-Stealer");
        }
    }
}