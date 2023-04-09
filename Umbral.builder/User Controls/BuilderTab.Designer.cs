namespace Umbral.builder.User_Controls
{
    partial class BuilderTab
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.OutputTextBox = new System.Windows.Forms.TextBox();
            this.BuildButton = new System.Windows.Forms.Button();
            this.IconSelectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(16, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Results";
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.AcceptsReturn = true;
            this.OutputTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.OutputTextBox.ForeColor = System.Drawing.Color.Yellow;
            this.OutputTextBox.Location = new System.Drawing.Point(19, 51);
            this.OutputTextBox.Multiline = true;
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.ReadOnly = true;
            this.OutputTextBox.Size = new System.Drawing.Size(624, 296);
            this.OutputTextBox.TabIndex = 1;
            this.OutputTextBox.Text = "Ready...";
            this.OutputTextBox.TextChanged += new System.EventHandler(this.OutputTextBox_TextChanged);
            // 
            // BuildButton
            // 
            this.BuildButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuildButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.BuildButton.ForeColor = System.Drawing.SystemColors.Control;
            this.BuildButton.Location = new System.Drawing.Point(553, 395);
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(90, 32);
            this.BuildButton.TabIndex = 2;
            this.BuildButton.Text = "Build";
            this.BuildButton.UseVisualStyleBackColor = true;
            this.BuildButton.Click += new System.EventHandler(this.BuildButton_Click);
            // 
            // IconSelectButton
            // 
            this.IconSelectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IconSelectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.IconSelectButton.ForeColor = System.Drawing.SystemColors.Control;
            this.IconSelectButton.Location = new System.Drawing.Point(421, 395);
            this.IconSelectButton.Name = "IconSelectButton";
            this.IconSelectButton.Size = new System.Drawing.Size(114, 32);
            this.IconSelectButton.TabIndex = 3;
            this.IconSelectButton.Text = "Select Icon";
            this.IconSelectButton.UseVisualStyleBackColor = true;
            this.IconSelectButton.Click += new System.EventHandler(this.IconSelectButton_Click);
            // 
            // BuilderTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.Controls.Add(this.IconSelectButton);
            this.Controls.Add(this.BuildButton);
            this.Controls.Add(this.OutputTextBox);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "BuilderTab";
            this.Size = new System.Drawing.Size(667, 477);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.Button BuildButton;
        private System.Windows.Forms.Button IconSelectButton;
    }
}
