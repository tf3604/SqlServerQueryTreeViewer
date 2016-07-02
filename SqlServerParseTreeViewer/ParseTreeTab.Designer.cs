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
    partial class ParseTreeTab
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
            this.treeTab = new System.Windows.Forms.TabControl();
            this.treeViewTab = new System.Windows.Forms.TabPage();
            this.treeViewImage = new System.Windows.Forms.PictureBox();
            this.treeTextTab = new System.Windows.Forms.TabPage();
            this.treeTextBox = new System.Windows.Forms.RichTextBox();
            this.treeTab.SuspendLayout();
            this.treeViewTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeViewImage)).BeginInit();
            this.treeTextTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeTab
            // 
            this.treeTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeTab.Controls.Add(this.treeViewTab);
            this.treeTab.Controls.Add(this.treeTextTab);
            this.treeTab.Location = new System.Drawing.Point(0, 0);
            this.treeTab.Name = "treeTab";
            this.treeTab.SelectedIndex = 0;
            this.treeTab.Size = new System.Drawing.Size(683, 447);
            this.treeTab.TabIndex = 0;
            // 
            // treeViewTab
            // 
            this.treeViewTab.AutoScroll = true;
            this.treeViewTab.Controls.Add(this.treeViewImage);
            this.treeViewTab.Location = new System.Drawing.Point(4, 22);
            this.treeViewTab.Name = "treeViewTab";
            this.treeViewTab.Padding = new System.Windows.Forms.Padding(3);
            this.treeViewTab.Size = new System.Drawing.Size(675, 421);
            this.treeViewTab.TabIndex = 1;
            this.treeViewTab.Text = "Tree View";
            this.treeViewTab.UseVisualStyleBackColor = true;
            // 
            // treeViewImage
            // 
            this.treeViewImage.Location = new System.Drawing.Point(0, 0);
            this.treeViewImage.Name = "treeViewImage";
            this.treeViewImage.Size = new System.Drawing.Size(2000, 1000);
            this.treeViewImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.treeViewImage.TabIndex = 0;
            this.treeViewImage.TabStop = false;
            this.treeViewImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TreeViewImage_MouseClick);
            this.treeViewImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TreeViewImage_MouseMove);
            // 
            // treeTextTab
            // 
            this.treeTextTab.Controls.Add(this.treeTextBox);
            this.treeTextTab.Location = new System.Drawing.Point(4, 22);
            this.treeTextTab.Name = "treeTextTab";
            this.treeTextTab.Padding = new System.Windows.Forms.Padding(3);
            this.treeTextTab.Size = new System.Drawing.Size(675, 421);
            this.treeTextTab.TabIndex = 0;
            this.treeTextTab.Text = "Tree Text";
            this.treeTextTab.UseVisualStyleBackColor = true;
            // 
            // treeTextBox
            // 
            this.treeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeTextBox.Location = new System.Drawing.Point(0, 0);
            this.treeTextBox.Name = "treeTextBox";
            this.treeTextBox.Size = new System.Drawing.Size(672, 428);
            this.treeTextBox.TabIndex = 0;
            this.treeTextBox.Text = "";
            this.treeTextBox.WordWrap = false;
            // 
            // ParseTreeTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeTab);
            this.Name = "ParseTreeTab";
            this.Size = new System.Drawing.Size(686, 450);
            this.treeTab.ResumeLayout(false);
            this.treeViewTab.ResumeLayout(false);
            this.treeViewTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeViewImage)).EndInit();
            this.treeTextTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl treeTab;
        private System.Windows.Forms.TabPage treeTextTab;
        private System.Windows.Forms.RichTextBox treeTextBox;
        private System.Windows.Forms.TabPage treeViewTab;
        private System.Windows.Forms.PictureBox treeViewImage;
    }
}
