//  Copyright(c) 2016 Brian Hansen.

//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
    
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//  of the Software.
    
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//  DEALINGS IN THE SOFTWARE.

namespace SqlServerParseTreeViewer
{
    partial class AboutForm
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
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.appNameLabel = new System.Windows.Forms.Label();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.companyLinkLabel = new System.Windows.Forms.LinkLabel();
            this.disclaimerLabel = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.versionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Location = new System.Drawing.Point(13, 13);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(50, 51);
            this.iconPictureBox.TabIndex = 0;
            this.iconPictureBox.TabStop = false;
            // 
            // appNameLabel
            // 
            this.appNameLabel.AutoSize = true;
            this.appNameLabel.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appNameLabel.Location = new System.Drawing.Point(80, 13);
            this.appNameLabel.Name = "appNameLabel";
            this.appNameLabel.Size = new System.Drawing.Size(284, 29);
            this.appNameLabel.TabIndex = 10;
            this.appNameLabel.Text = "SQL Server Parse Tree Viewer";
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(82, 51);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(118, 13);
            this.copyrightLabel.TabIndex = 11;
            this.copyrightLabel.Text = "Copyright Brian Hansen";
            // 
            // companyLinkLabel
            // 
            this.companyLinkLabel.Location = new System.Drawing.Point(304, 51);
            this.companyLinkLabel.Name = "companyLinkLabel";
            this.companyLinkLabel.Size = new System.Drawing.Size(184, 23);
            this.companyLinkLabel.TabIndex = 12;
            this.companyLinkLabel.TabStop = true;
            this.companyLinkLabel.Text = "linkLabel1";
            this.companyLinkLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.companyLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.CompanyLinkLabel_LinkClicked);
            // 
            // disclaimerLabel
            // 
            this.disclaimerLabel.Location = new System.Drawing.Point(12, 78);
            this.disclaimerLabel.Multiline = true;
            this.disclaimerLabel.Name = "disclaimerLabel";
            this.disclaimerLabel.ReadOnly = true;
            this.disclaimerLabel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.disclaimerLabel.Size = new System.Drawing.Size(476, 146);
            this.disclaimerLabel.TabIndex = 13;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(215, 246);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 14;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(392, 25);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(100, 23);
            this.versionLabel.TabIndex = 15;
            this.versionLabel.Text = "version";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // AboutForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.okButton;
            this.ClientSize = new System.Drawing.Size(504, 284);
            this.ControlBox = false;
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.disclaimerLabel);
            this.Controls.Add(this.companyLinkLabel);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.appNameLabel);
            this.Controls.Add(this.iconPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Text = "About SQL Server Parse Tree Viewer";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox iconPictureBox;
        private System.Windows.Forms.Label appNameLabel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.LinkLabel companyLinkLabel;
        private System.Windows.Forms.TextBox disclaimerLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label versionLabel;
    }
}