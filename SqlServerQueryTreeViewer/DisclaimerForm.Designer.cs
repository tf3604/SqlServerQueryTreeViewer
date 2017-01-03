//  Copyright(c) 2016-2017 Brian Hansen.

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

namespace SqlServerQueryTreeViewer
{
    partial class DisclaimerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisclaimerForm));
            this.disclaimerLabel = new System.Windows.Forms.Label();
            this.agreeButton = new System.Windows.Forms.Button();
            this.dontAgreeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // disclaimerLabel
            // 
            this.disclaimerLabel.Location = new System.Drawing.Point(12, 9);
            this.disclaimerLabel.Name = "disclaimerLabel";
            this.disclaimerLabel.Size = new System.Drawing.Size(507, 78);
            this.disclaimerLabel.TabIndex = 0;
            this.disclaimerLabel.Text = resources.GetString("disclaimerLabel.Text");
            // 
            // agreeButton
            // 
            this.agreeButton.Location = new System.Drawing.Point(228, 90);
            this.agreeButton.Name = "agreeButton";
            this.agreeButton.Size = new System.Drawing.Size(75, 23);
            this.agreeButton.TabIndex = 1;
            this.agreeButton.Text = "I agree";
            this.agreeButton.UseVisualStyleBackColor = true;
            this.agreeButton.Click += new System.EventHandler(this.AgreeButton_Click);
            // 
            // dontAgreeButton
            // 
            this.dontAgreeButton.Location = new System.Drawing.Point(114, 123);
            this.dontAgreeButton.Name = "dontAgreeButton";
            this.dontAgreeButton.Size = new System.Drawing.Size(302, 23);
            this.dontAgreeButton.TabIndex = 2;
            this.dontAgreeButton.Text = "I\'m sufficient frightened and I don\'t want to use this tool";
            this.dontAgreeButton.UseVisualStyleBackColor = true;
            this.dontAgreeButton.Click += new System.EventHandler(this.DontAgreeButton_Click);
            // 
            // DisclaimerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 159);
            this.Controls.Add(this.dontAgreeButton);
            this.Controls.Add(this.agreeButton);
            this.Controls.Add(this.disclaimerLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DisclaimerForm";
            this.Text = "Disclaimer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label disclaimerLabel;
        private System.Windows.Forms.Button agreeButton;
        private System.Windows.Forms.Button dontAgreeButton;
    }
}