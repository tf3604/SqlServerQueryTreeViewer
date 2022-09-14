//  Copyright(c) 2016-2022 Breanna Hansen.

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
    partial class InfoTrackerUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoTrackerUserControl));
            this.optimizerInfoCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.transformationStatsCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // optimizerInfoCheckBox
            // 
            this.optimizerInfoCheckBox.AutoSize = true;
            this.optimizerInfoCheckBox.Location = new System.Drawing.Point(4, 4);
            this.optimizerInfoCheckBox.Name = "optimizerInfoCheckBox";
            this.optimizerInfoCheckBox.Size = new System.Drawing.Size(121, 17);
            this.optimizerInfoCheckBox.TabIndex = 0;
            this.optimizerInfoCheckBox.Text = "Track Optimizer Info";
            this.optimizerInfoCheckBox.UseVisualStyleBackColor = true;
            this.optimizerInfoCheckBox.CheckedChanged += new System.EventHandler(this.OptimizerInfoCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(28, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(401, 80);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // transformationStatsCheckBox
            // 
            this.transformationStatsCheckBox.AutoSize = true;
            this.transformationStatsCheckBox.Location = new System.Drawing.Point(4, 111);
            this.transformationStatsCheckBox.Name = "transformationStatsCheckBox";
            this.transformationStatsCheckBox.Size = new System.Drawing.Size(154, 17);
            this.transformationStatsCheckBox.TabIndex = 2;
            this.transformationStatsCheckBox.Text = "Track Transformation Stats";
            this.transformationStatsCheckBox.UseVisualStyleBackColor = true;
            this.transformationStatsCheckBox.CheckedChanged += new System.EventHandler(this.TransformationStatsCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(28, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(401, 80);
            this.label2.TabIndex = 3;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // InfoTrackerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.transformationStatsCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.optimizerInfoCheckBox);
            this.Name = "InfoTrackerUserControl";
            this.Size = new System.Drawing.Size(445, 243);
            this.Load += new System.EventHandler(this.InfoTrackerUserControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox optimizerInfoCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox transformationStatsCheckBox;
        private System.Windows.Forms.Label label2;
    }
}
