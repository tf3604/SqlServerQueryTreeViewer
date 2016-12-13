namespace SqlServerQueryTreeViewer
{
    partial class RenderStyleUserControl
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
            this.planStyleHorizontalButton = new System.Windows.Forms.RadioButton();
            this.planStyleHorizontalLabel = new System.Windows.Forms.Label();
            this.vertialBalancedTreeLabel = new System.Windows.Forms.Label();
            this.vertialBalancedTreeButton = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // planStyleHorizontalButton
            // 
            this.planStyleHorizontalButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.planStyleHorizontalButton.Location = new System.Drawing.Point(4, 4);
            this.planStyleHorizontalButton.Name = "planStyleHorizontalButton";
            this.planStyleHorizontalButton.Size = new System.Drawing.Size(438, 26);
            this.planStyleHorizontalButton.TabIndex = 0;
            this.planStyleHorizontalButton.TabStop = true;
            this.planStyleHorizontalButton.Text = "Plan-style horizontal";
            this.planStyleHorizontalButton.UseVisualStyleBackColor = true;
            this.planStyleHorizontalButton.CheckedChanged += new System.EventHandler(this.PlanStyleHorizontalButton_CheckedChanged);
            // 
            // planStyleHorizontalLabel
            // 
            this.planStyleHorizontalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.planStyleHorizontalLabel.Location = new System.Drawing.Point(26, 37);
            this.planStyleHorizontalLabel.Name = "planStyleHorizontalLabel";
            this.planStyleHorizontalLabel.Size = new System.Drawing.Size(403, 38);
            this.planStyleHorizontalLabel.TabIndex = 1;
            this.planStyleHorizontalLabel.Text = "Render query trees in a horizontal style similar to a SQL Server Management Studi" +
    "o query plan.  This style is the default beginning in v0.7.";
            // 
            // vertialBalancedTreeLabel
            // 
            this.vertialBalancedTreeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vertialBalancedTreeLabel.Location = new System.Drawing.Point(25, 103);
            this.vertialBalancedTreeLabel.Name = "vertialBalancedTreeLabel";
            this.vertialBalancedTreeLabel.Size = new System.Drawing.Size(403, 38);
            this.vertialBalancedTreeLabel.TabIndex = 3;
            this.vertialBalancedTreeLabel.Text = "Render query trees in a vertical balanced-tree style.  This style is the default " +
    "prior to v0.7.";
            // 
            // vertialBalancedTreeButton
            // 
            this.vertialBalancedTreeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vertialBalancedTreeButton.Location = new System.Drawing.Point(3, 70);
            this.vertialBalancedTreeButton.Name = "vertialBalancedTreeButton";
            this.vertialBalancedTreeButton.Size = new System.Drawing.Size(438, 26);
            this.vertialBalancedTreeButton.TabIndex = 2;
            this.vertialBalancedTreeButton.TabStop = true;
            this.vertialBalancedTreeButton.Text = "Vertical balanced tree";
            this.vertialBalancedTreeButton.UseVisualStyleBackColor = true;
            this.vertialBalancedTreeButton.CheckedChanged += new System.EventHandler(this.VertialBalancedTreeButton_CheckedChanged);
            // 
            // RenderStyleUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vertialBalancedTreeLabel);
            this.Controls.Add(this.vertialBalancedTreeButton);
            this.Controls.Add(this.planStyleHorizontalLabel);
            this.Controls.Add(this.planStyleHorizontalButton);
            this.Name = "RenderStyleUserControl";
            this.Size = new System.Drawing.Size(445, 243);
            this.Load += new System.EventHandler(this.RenderStyleUserControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton planStyleHorizontalButton;
        private System.Windows.Forms.Label planStyleHorizontalLabel;
        private System.Windows.Forms.Label vertialBalancedTreeLabel;
        private System.Windows.Forms.RadioButton vertialBalancedTreeButton;
    }
}
