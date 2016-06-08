namespace SqlServerParseTreeViewer
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
            this.colorLabel.Location = new System.Drawing.Point(191, 4);
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
            this.standardColorComboBox.Location = new System.Drawing.Point(194, 21);
            this.standardColorComboBox.Name = "standardColorComboBox";
            this.standardColorComboBox.Size = new System.Drawing.Size(155, 21);
            this.standardColorComboBox.TabIndex = 3;
            this.standardColorComboBox.SelectedIndexChanged += new System.EventHandler(this.StandardColorComboBox_SelectedIndexChanged);
            // 
            // customColorButton
            // 
            this.customColorButton.Location = new System.Drawing.Point(356, 21);
            this.customColorButton.Name = "customColorButton";
            this.customColorButton.Size = new System.Drawing.Size(75, 23);
            this.customColorButton.TabIndex = 4;
            this.customColorButton.Text = "Custom...";
            this.customColorButton.UseVisualStyleBackColor = true;
            this.customColorButton.Click += new System.EventHandler(this.CustomColorButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(194, 49);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // OperatorColorsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
    }
}
