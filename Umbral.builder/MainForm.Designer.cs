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
            this.MenuPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.MenuPanel.Location = new System.Drawing.Point(0, 0);
            this.MenuPanel.Name = "MenuPanel";
            this.MenuPanel.Size = new System.Drawing.Size(221, 477);
            this.MenuPanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AboutTabButton);
            this.panel1.Controls.Add(this.SoonTabButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 285);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(219, 190);
            this.panel1.TabIndex = 3;
            // 
            // AboutTabButton
            // 
            this.AboutTabButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.AboutTabButton.FlatAppearance.BorderSize = 0;
            this.AboutTabButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AboutTabButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AboutTabButton.ForeColor = System.Drawing.Color.White;
            this.AboutTabButton.Location = new System.Drawing.Point(0, 1);
            this.AboutTabButton.Name = "AboutTabButton";
            this.AboutTabButton.Size = new System.Drawing.Size(219, 95);
            this.AboutTabButton.TabIndex = 3;
            this.AboutTabButton.Text = "About";
            this.AboutTabButton.UseVisualStyleBackColor = true;
            this.AboutTabButton.Click += new System.EventHandler(this.AboutTabButton_Click);
            // 
            // SoonTabButton
            // 
            this.SoonTabButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SoonTabButton.FlatAppearance.BorderSize = 0;
            this.SoonTabButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SoonTabButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SoonTabButton.ForeColor = System.Drawing.Color.White;
            this.SoonTabButton.Location = new System.Drawing.Point(0, 95);
            this.SoonTabButton.Name = "SoonTabButton";
            this.SoonTabButton.Size = new System.Drawing.Size(219, 95);
            this.SoonTabButton.TabIndex = 2;
            this.SoonTabButton.Text = "Soon...";
            this.SoonTabButton.UseVisualStyleBackColor = true;
            this.SoonTabButton.Click += new System.EventHandler(this.DropperTabButton_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 1);
            this.label1.TabIndex = 0;
            // 
            // BuilderTabButton
            // 
            this.BuilderTabButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.BuilderTabButton.FlatAppearance.BorderSize = 0;
            this.BuilderTabButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuilderTabButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BuilderTabButton.ForeColor = System.Drawing.Color.White;
            this.BuilderTabButton.Location = new System.Drawing.Point(0, 190);
            this.BuilderTabButton.Name = "BuilderTabButton";
            this.BuilderTabButton.Size = new System.Drawing.Size(219, 95);
            this.BuilderTabButton.TabIndex = 2;
            this.BuilderTabButton.Text = "Builder";
            this.BuilderTabButton.UseVisualStyleBackColor = true;
            this.BuilderTabButton.Click += new System.EventHandler(this.BuilderTabButton_Click);
            // 
            // AssemblyTabButton
            // 
            this.AssemblyTabButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.AssemblyTabButton.FlatAppearance.BorderSize = 0;
            this.AssemblyTabButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AssemblyTabButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssemblyTabButton.ForeColor = System.Drawing.Color.White;
            this.AssemblyTabButton.Location = new System.Drawing.Point(0, 95);
            this.AssemblyTabButton.Name = "AssemblyTabButton";
            this.AssemblyTabButton.Size = new System.Drawing.Size(219, 95);
            this.AssemblyTabButton.TabIndex = 1;
            this.AssemblyTabButton.Text = "Assembly";
            this.AssemblyTabButton.UseVisualStyleBackColor = true;
            this.AssemblyTabButton.Click += new System.EventHandler(this.AssemblyTabButton_Click);
            // 
            // GeneralTabButton
            // 
            this.GeneralTabButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.GeneralTabButton.FlatAppearance.BorderSize = 0;
            this.GeneralTabButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GeneralTabButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GeneralTabButton.ForeColor = System.Drawing.Color.White;
            this.GeneralTabButton.Location = new System.Drawing.Point(0, 0);
            this.GeneralTabButton.Name = "GeneralTabButton";
            this.GeneralTabButton.Size = new System.Drawing.Size(219, 95);
            this.GeneralTabButton.TabIndex = 0;
            this.GeneralTabButton.Text = "General";
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
            this.FrontPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FrontPanel.Location = new System.Drawing.Point(221, 0);
            this.FrontPanel.Name = "FrontPanel";
            this.FrontPanel.Size = new System.Drawing.Size(667, 477);
            this.FrontPanel.TabIndex = 1;
            // 
            // DropperTab
            // 
            this.DropperTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.DropperTab.Location = new System.Drawing.Point(0, -1);
            this.DropperTab.Name = "DropperTab";
            this.DropperTab.Size = new System.Drawing.Size(667, 477);
            this.DropperTab.TabIndex = 4;
            // 
            // aboutTab
            // 
            this.aboutTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.aboutTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aboutTab.Location = new System.Drawing.Point(0, 0);
            this.aboutTab.Name = "aboutTab";
            this.aboutTab.Size = new System.Drawing.Size(665, 475);
            this.aboutTab.TabIndex = 3;
            // 
            // builderTab
            // 
            this.builderTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.builderTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.builderTab.ForeColor = System.Drawing.SystemColors.ControlText;
            this.builderTab.Location = new System.Drawing.Point(0, 0);
            this.builderTab.Name = "builderTab";
            this.builderTab.Size = new System.Drawing.Size(665, 475);
            this.builderTab.TabIndex = 2;
            // 
            // assemblyTab
            // 
            this.assemblyTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.assemblyTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assemblyTab.Location = new System.Drawing.Point(0, 0);
            this.assemblyTab.Name = "assemblyTab";
            this.assemblyTab.Size = new System.Drawing.Size(665, 475);
            this.assemblyTab.TabIndex = 1;
            // 
            // generalTab
            // 
            this.generalTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.generalTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generalTab.Location = new System.Drawing.Point(0, 0);
            this.generalTab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.generalTab.Name = "generalTab";
            this.generalTab.Size = new System.Drawing.Size(665, 475);
            this.generalTab.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 477);
            this.Controls.Add(this.FrontPanel);
            this.Controls.Add(this.MenuPanel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Umbral Stealer (Builder)";
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

