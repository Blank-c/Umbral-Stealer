using System;
using System.Linq;
using System.Windows.Forms;

namespace Umbral.builder.User_Controls
{
    public partial class AssemblyTab : UserControl
    {

        public static string CompanyNameInfo;
        public static string FileDescriptionInfo;
        public static string ProductNameInfo;
        public static string LegalCopyrightInfo;
        public static string LegalTrademarksInfo;
        public static string InternalNameInfo;
        public static string OriginalFilenameInfo;
        public static int[] ProductVersionInfo;
        public static int[] FileVersionInfo;
        public static int[] AssemblyVersionInfo;

        static AssemblyTab()
        {
            ProductVersionInfo = new int[4];
            FileVersionInfo = new int[4];
            AssemblyVersionInfo = new int[4];
        }

        public AssemblyTab()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, System.EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if(textBox.Equals(companyNameTextField))
                CompanyNameInfo = textBox.Text;
            else if(textBox.Equals(fileDescriptionTextField))
                FileDescriptionInfo = textBox.Text;
            else if (textBox.Equals(productNameTextField))
                ProductNameInfo = textBox.Text;
            else if (textBox.Equals(copyrightTextField))
                LegalCopyrightInfo = textBox.Text;
            else if (textBox.Equals(trademarksTextField))
                LegalTrademarksInfo = textBox.Text;
            else if (textBox.Equals(productVersionTextField1))
                ProductVersionInfo[0] = Convert.ToInt32(textBox.Text);
            else if (textBox.Equals(productVersionTextField2))
                ProductVersionInfo[1] = Convert.ToInt32(textBox.Text);
            else if (textBox.Equals(productVersionTextField3))
                ProductVersionInfo[2] = Convert.ToInt32(textBox.Text);
            else if (textBox.Equals(productVersionTextField4))
                ProductVersionInfo[3] = Convert.ToInt32(textBox.Text);
            else if (textBox.Equals(fileVersionTextField1))
                FileVersionInfo[0] = Convert.ToInt32(textBox.Text);
            else if (textBox.Equals(fileVersionTextField2))
                FileVersionInfo[1] = Convert.ToInt32(textBox.Text);
            else if (textBox.Equals(fileVersionTextField3))
                FileVersionInfo[2] = Convert.ToInt32(textBox.Text);
            else if (textBox.Equals(fileVersionTextField4))
                FileVersionInfo[3] = Convert.ToInt32(textBox.Text);
            else if (textBox.Equals(assemblyVersionTextField1))
                AssemblyVersionInfo[0] = Convert.ToInt32(textBox.Text);
            else if (textBox.Equals(assemblyVersionTextField1))
                AssemblyVersionInfo[1] = Convert.ToInt32(textBox.Text);
            else if (textBox.Equals(assemblyVersionTextField1))
                AssemblyVersionInfo[2] = Convert.ToInt32(textBox.Text);
            else if (textBox.Equals(assemblyVersionTextField1))
                AssemblyVersionInfo[3] = Convert.ToInt32(textBox.Text);
            else if (textBox.Equals(internalNameTextField))
                InternalNameInfo = textBox.Text;
            else if (textBox.Equals(originalFilenameTextField))
                OriginalFilenameInfo = textBox.Text;
        }

        private void VersionField_TextChanged(object sender, System.EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = new string(textBox.Text.Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(textBox.Text))
                textBox.Text = "0";
            TextBox_TextChanged(sender, e);
        }
    }
}
