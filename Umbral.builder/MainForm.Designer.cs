using Umbral.builder.User_Controls;

namespace Umbral.builder
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MenuPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AboutTabButton = new System.Windows.Forms.Button();
            this.SoonTabButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BuilderTabButton = new System.Windows.Forms.Button();
            this.AssemblyTabButton = new System.Windows.Forms.Button();
            this.GeneralTabButton = new System.Windows.Forms.Button();
            this.FrontPanel = new System.Windows.Forms.Panel();
            this.DropperTab = new Umbral.builder.User_Controls.Soon();
            this.aboutTab = new Umbral.builder.User_Controls.AboutTab();
            this.builderTab = new Umbral.builder.User_Controls.BuilderTab();
            this.assemblyTab = new Umbral.builder.User_Controls.AssemblyTab();
            this.generalTab = new Umbral.builder.User_Controls.GeneralTab();
            this.MenuPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.FrontPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuPanel
            // 
            this.MenuPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.MenuPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MenuPanel.Controls.Add(this.panel1);
            this.MenuPanel.Controls.Add(this.BuilderTabButton);
            this.MenuPanel.Controls.Add(this.AssemblyTabButton);
            this.MenuPanel.Controls.Add(this.GeneralTabButton);
            resources.ApplyResources(this.MenuPanel, "MenuPanel");
            this.MenuPanel.Name = "MenuPanel";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AboutTabButton);
            this.panel1.Controls.Add(this.SoonTabButton);
            this.panel1.Controls.Add(this.label1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // AboutTabButton
            // 
            resources.ApplyResources(this.AboutTabButton, "AboutTabButton");
            this.AboutTabButton.FlatAppearance.BorderSize = 0;
            this.AboutTabButton.ForeColor = System.Drawing.Color.White;
            this.AboutTabButton.Name = "AboutTabButton";
            this.AboutTabButton.UseVisualStyleBackColor = true;
            this.AboutTabButton.Click += new System.EventHandler(this.AboutTabButton_Click);
            // 
            // SoonTabButton
            // 
            resources.ApplyResources(this.SoonTabButton, "SoonTabButton");
            this.SoonTabButton.FlatAppearance.BorderSize = 0;
            this.SoonTabButton.ForeColor = System.Drawing.Color.White;
            this.SoonTabButton.Name = "SoonTabButton";
            this.SoonTabButton.UseVisualStyleBackColor = true;
            this.SoonTabButton.Click += new System.EventHandler(this.DropperTabButton_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.label1.Name = "label1";
            // 
            // BuilderTabButton
            // 
            resources.ApplyResources(this.BuilderTabButton, "BuilderTabButton");
            this.BuilderTabButton.FlatAppearance.BorderSize = 0;
            this.BuilderTabButton.ForeColor = System.Drawing.Color.White;
            this.BuilderTabButton.Name = "BuilderTabButton";
            this.BuilderTabButton.UseVisualStyleBackColor = true;
            this.BuilderTabButton.Click += new System.EventHandler(this.BuilderTabButton_Click);
            // 
            // AssemblyTabButton
            // 
            resources.ApplyResources(this.AssemblyTabButton, "AssemblyTabButton");
            this.AssemblyTabButton.FlatAppearance.BorderSize = 0;
            this.AssemblyTabButton.ForeColor = System.Drawing.Color.White;
            this.AssemblyTabButton.Name = "AssemblyTabButton";
            this.AssemblyTabButton.UseVisualStyleBackColor = true;
            this.AssemblyTabButton.Click += new System.EventHandler(this.AssemblyTabButton_Click);
            // 
            // GeneralTabButton
            // 
            resources.ApplyResources(this.GeneralTabButton, "GeneralTabButton");
            this.GeneralTabButton.FlatAppearance.BorderSize = 0;
            this.GeneralTabButton.ForeColor = System.Drawing.Color.White;
            this.GeneralTabButton.Name = "GeneralTabButton";
            this.GeneralTabButton.UseVisualStyleBackColor = true;
            this.GeneralTabButton.Click += new System.EventHandler(this.GeneralTabButton_Click);
            // 
            // FrontPanel
            // 
            this.FrontPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.FrontPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FrontPanel.Controls.Add(this.DropperTab);
            this.FrontPanel.Controls.Add(this.aboutTab);
            this.FrontPanel.Controls.Add(this.builderTab);
            this.FrontPanel.Controls.Add(this.assemblyTab);
            this.FrontPanel.Controls.Add(this.generalTab);
            this.FrontPanel.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.FrontPanel, "FrontPanel");
            this.FrontPanel.Name = "FrontPanel";
            // 
            // DropperTab
            // 
            this.DropperTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            resources.ApplyResources(this.DropperTab, "DropperTab");
            this.DropperTab.Name = "DropperTab";
            // 
            // aboutTab
            // 
            this.aboutTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            resources.ApplyResources(this.aboutTab, "aboutTab");
            this.aboutTab.Name = "aboutTab";
            // 
            // builderTab
            // 
            this.builderTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            resources.ApplyResources(this.builderTab, "builderTab");
            this.builderTab.ForeColor = System.Drawing.SystemColors.ControlText;
            this.builderTab.Name = "builderTab";
            // 
            // assemblyTab
            // 
            this.assemblyTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            resources.ApplyResources(this.assemblyTab, "assemblyTab");
            this.assemblyTab.Name = "assemblyTab";
            // 
            // generalTab
            // 
            this.generalTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            resources.ApplyResources(this.generalTab, "generalTab");
            this.generalTab.Name = "generalTab";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FrontPanel);
            this.Controls.Add(this.MenuPanel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MenuPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.FrontPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MenuPanel;
        private System.Windows.Forms.Panel FrontPanel;
        private System.Windows.Forms.Button GeneralTabButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BuilderTabButton;
        private System.Windows.Forms.Button AssemblyTabButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AboutTabButton;
        private System.Windows.Forms.Button SoonTabButton;
        private GeneralTab generalTab;
        private BuilderTab builderTab;
        private AssemblyTab assemblyTab;
        private User_Controls.AboutTab aboutTab;
        private Soon DropperTab;
    }
}

