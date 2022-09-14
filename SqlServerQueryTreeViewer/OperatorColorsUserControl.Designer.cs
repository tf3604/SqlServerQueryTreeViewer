﻿//  Copyright(c) 2016-2022 Breanna Hansen.

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
    partial class OperatorColorsUserControl
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
            this.operatorNameLabel = new System.Windows.Forms.Label();
            this.operatorsListView = new System.Windows.Forms.ListBox();
            this.colorLabel = new System.Windows.Forms.Label();
            this.standardColorComboBox = new System.Windows.Forms.ColorComboBox();
            this.customColorButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.informationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // operatorNameLabel
            // 
            this.operatorNameLabel.AutoSize = true;
            this.operatorNameLabel.Location = new System.Drawing.Point(4, 4);
            this.operatorNameLabel.Name = "operatorNameLabel";
            this.operatorNameLabel.Size = new System.Drawing.Size(56, 13);
            this.operatorNameLabel.TabIndex = 0;
            this.operatorNameLabel.Text = "Operators:";
            // 
            // operatorsListView
            // 
            this.operatorsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.operatorsListView.Location = new System.Drawing.Point(7, 21);
            this.operatorsListView.Name = "operatorsListView";
            this.operatorsListView.Size = new System.Drawing.Size(177, 212);
            this.operatorsListView.TabIndex = 1;
            this.operatorsListView.SelectedIndexChanged += new System.EventHandler(this.OperatorsListView_SelectedIndexChanged);
            // 
            // colorLabel
            // 
            this.colorLabel.AutoSize = true;
            this.colorLabel.Location = new System.Drawing.Point(190, 58);
            this.colorLabel.Name = "colorLabel";
            this.colorLabel.Size = new System.Drawing.Size(34, 13);
            this.colorLabel.TabIndex = 2;
            this.colorLabel.Text = "Color:";
            // 
            // standardColorComboBox
            // 
            this.standardColorComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.standardColorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.standardColorComboBox.FormattingEnabled = true;
            this.standardColorComboBox.Location = new System.Drawing.Point(193, 75);
            this.standardColorComboBox.Name = "standardColorComboBox";
            this.standardColorComboBox.Size = new System.Drawing.Size(155, 21);
            this.standardColorComboBox.TabIndex = 3;
            this.standardColorComboBox.SelectedIndexChanged += new System.EventHandler(this.StandardColorComboBox_SelectedIndexChanged);
            // 
            // customColorButton
            // 
            this.customColorButton.Location = new System.Drawing.Point(355, 75);
            this.customColorButton.Name = "customColorButton";
            this.customColorButton.Size = new System.Drawing.Size(75, 23);
            this.customColorButton.TabIndex = 4;
            this.customColorButton.Text = "Custom...";
            this.customColorButton.UseVisualStyleBackColor = true;
            this.customColorButton.Click += new System.EventHandler(this.CustomColorButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(193, 103);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // informationLabel
            // 
            this.informationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.informationLabel.Location = new System.Drawing.Point(193, 21);
            this.informationLabel.Name = "informationLabel";
            this.informationLabel.Size = new System.Drawing.Size(237, 37);
            this.informationLabel.TabIndex = 6;
            this.informationLabel.Text = "Set the colors used in the vertical balanced tree view.";
            // 
            // OperatorColorsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.informationLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.customColorButton);
            this.Controls.Add(this.standardColorComboBox);
            this.Controls.Add(this.colorLabel);
            this.Controls.Add(this.operatorsListView);
            this.Controls.Add(this.operatorNameLabel);
            this.Name = "OperatorColorsUserControl";
            this.Size = new System.Drawing.Size(445, 243);
            this.Load += new System.EventHandler(this.OperatorColorsUserControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label operatorNameLabel;
        private System.Windows.Forms.ListBox operatorsListView;
        private System.Windows.Forms.Label colorLabel;
        private System.Windows.Forms.ColorComboBox standardColorComboBox;
        private System.Windows.Forms.Button customColorButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Label informationLabel;
    }
}
