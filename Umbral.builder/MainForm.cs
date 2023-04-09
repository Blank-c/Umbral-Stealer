using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Umbral.builder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        [DllImport("DwmApi")] //System.Runtime.InteropServices
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }

        private void GeneralTabButton_Click(object sender, EventArgs e)
        {
            
            assemblyTab.Hide();
            builderTab.Hide();
            aboutTab.Hide();
            DropperTab.Hide();

            generalTab.Show();
        }

        private void AssemblyTabButton_Click(object sender, EventArgs e)
        {
            generalTab.Hide();
            builderTab.Hide();
            aboutTab.Hide();
            DropperTab.Hide();

            assemblyTab.Show();
        }

        private void BuilderTabButton_Click(object sender, EventArgs e)
        {
            generalTab.Hide();
            assemblyTab.Hide();
            aboutTab.Hide();
            DropperTab.Hide();

            builderTab.Show();
        }

        private void DropperTabButton_Click(object sender, EventArgs e)
        {
            generalTab.Hide();
            assemblyTab.Hide();
            builderTab.Hide();
            aboutTab.Hide();

            DropperTab.Show();
        }

        private void AboutTabButton_Click(object sender, EventArgs e)
        {
            generalTab.Hide();
            assemblyTab.Hide();
            builderTab.Hide();
            DropperTab.Hide();

            aboutTab.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            assemblyTab.Hide();
            builderTab.Hide();
            aboutTab.Hide();
            DropperTab.Hide();

            generalTab.Show();
        }
    }
}
