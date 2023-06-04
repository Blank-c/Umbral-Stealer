using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vestris.ResourceLib;

namespace Umbral.builder.Components.Forms
{
    public partial class AssemblyEditorForm : Form
    {
        public AssemblyEditorForm()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        static extern private int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern private bool ReleaseCapture();

        private void Drag_On_Mousedown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Settings.CompanyName = txtCompanyName.Text;
            Settings.FileDescription = txtFileDescription.Text;
            Settings.ProductName = txtProductName.Text;
            Settings.LegalCopyright = txtLegalCopyright.Text;
            Settings.LegalTrademarks = txtLegalTrademarks.Text;
            Settings.InternalName = txtInternalName.Text;
            Settings.OriginalFilename = txtOriginalFilename.Text;

            Close();
        }

        private void AssemblyEditorForm_Shown(object sender, EventArgs e)
        {
            txtCompanyName.Text = Settings.CompanyName;
            txtFileDescription.Text = Settings.FileDescription;
            txtProductName.Text = Settings.ProductName;
            txtLegalCopyright.Text = Settings.LegalCopyright;
            txtLegalTrademarks.Text = Settings.LegalTrademarks;
            txtInternalName.Text = Settings.InternalName;
            txtOriginalFilename.Text = Settings.OriginalFilename;
        }
    }
}
