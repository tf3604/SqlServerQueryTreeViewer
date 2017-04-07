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
    partial class ViewerForm
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

                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewerForm));
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.tabScript = new System.Windows.Forms.TabPage();
            this.queryRichTextBox = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentQueriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showThumbnailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.connectButton = new System.Windows.Forms.ToolStripButton();
            this.disconnectButton = new System.Windows.Forms.ToolStripButton();
            this.dbContextComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.executeButton = new System.Windows.Forms.ToolStripButton();
            this.cancelQueryButton = new System.Windows.Forms.ToolStripButton();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.zoomComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.executionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainTabControl.SuspendLayout();
            this.tabScript.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTabControl
            // 
            this.mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTabControl.Controls.Add(this.tabScript);
            this.mainTabControl.Location = new System.Drawing.Point(0, 52);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1124, 503);
            this.mainTabControl.TabIndex = 0;
            this.mainTabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainTabControl_KeyDown);
            // 
            // tabScript
            // 
            this.tabScript.Controls.Add(this.queryRichTextBox);
            this.tabScript.Location = new System.Drawing.Point(4, 22);
            this.tabScript.Name = "tabScript";
            this.tabScript.Padding = new System.Windows.Forms.Padding(3);
            this.tabScript.Size = new System.Drawing.Size(1116, 477);
            this.tabScript.TabIndex = 0;
            this.tabScript.Text = "Script";
            this.tabScript.UseVisualStyleBackColor = true;
            // 
            // queryRichTextBox
            // 
            this.queryRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queryRichTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.queryRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.queryRichTextBox.Name = "queryRichTextBox";
            this.queryRichTextBox.Size = new System.Drawing.Size(1116, 481);
            this.queryRichTextBox.TabIndex = 0;
            this.queryRichTextBox.Text = "";
            this.queryRichTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.QueryRichTextBox_KeyDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1122, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.disconnectToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.connectToolStripMenuItem.Text = "Co&nnect...";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.ConnectToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.saveToolStripMenuItem.Text = "&Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Enabled = false;
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.disconnectToolStripMenuItem.Text = "&Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.DisconnectToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.recentQueriesToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            // 
            // recentQueriesToolStripMenuItem
            // 
            this.recentQueriesToolStripMenuItem.Name = "recentQueriesToolStripMenuItem";
            this.recentQueriesToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.recentQueriesToolStripMenuItem.Text = "&Recent Queries...";
            this.recentQueriesToolStripMenuItem.Click += new System.EventHandler(this.RecentQueriesToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.showThumbnailToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.optionsToolStripMenuItem.Text = "&Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsToolStripMenuItem_Click);
            // 
            // showThumbnailToolStripMenuItem
            // 
            this.showThumbnailToolStripMenuItem.Name = "showThumbnailToolStripMenuItem";
            this.showThumbnailToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.showThumbnailToolStripMenuItem.Text = "Show &Thumbnail ...";
            this.showThumbnailToolStripMenuItem.Click += new System.EventHandler(this.ShowThumbnailToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectButton,
            this.disconnectButton,
            this.dbContextComboBox,
            this.executeButton,
            this.cancelQueryButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1122, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "primaryToolStrip";
            // 
            // connectButton
            // 
            this.connectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.connectButton.Image = ((System.Drawing.Image)(resources.GetObject("connectButton.Image")));
            this.connectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(56, 22);
            this.connectButton.Text = "Connect";
            this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.disconnectButton.Enabled = false;
            this.disconnectButton.Image = ((System.Drawing.Image)(resources.GetObject("disconnectButton.Image")));
            this.disconnectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(70, 22);
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // dbContextComboBox
            // 
            this.dbContextComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dbContextComboBox.Name = "dbContextComboBox";
            this.dbContextComboBox.Size = new System.Drawing.Size(121, 25);
            this.dbContextComboBox.SelectedIndexChanged += new System.EventHandler(this.DbContextComboBox_SelectedIndexChanged);
            // 
            // executeButton
            // 
            this.executeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.executeButton.Image = ((System.Drawing.Image)(resources.GetObject("executeButton.Image")));
            this.executeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(51, 22);
            this.executeButton.Text = "Execute";
            this.executeButton.Click += new System.EventHandler(this.ExecuteButton_Click);
            // 
            // cancelQueryButton
            // 
            this.cancelQueryButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cancelQueryButton.Enabled = false;
            this.cancelQueryButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelQueryButton.Image")));
            this.cancelQueryButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cancelQueryButton.Name = "cancelQueryButton";
            this.cancelQueryButton.Size = new System.Drawing.Size(47, 22);
            this.cancelQueryButton.Text = "Cancel";
            this.cancelQueryButton.Click += new System.EventHandler(this.CancelQueryButton_Click);
            // 
            // statusBar
            // 
            this.statusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusBar.AutoSize = false;
            this.statusBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.statusBar.Dock = System.Windows.Forms.DockStyle.None;
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomComboBox,
            this.executionStatus,
            this.timerLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 558);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1124, 22);
            this.statusBar.TabIndex = 3;
            // 
            // zoomComboBox
            // 
            this.zoomComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.zoomComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zoomComboBox.Name = "zoomComboBox";
            this.zoomComboBox.Size = new System.Drawing.Size(75, 22);
            this.zoomComboBox.SelectedIndexChanged += new System.EventHandler(this.ZoomComboBox_SelectedIndexChanged);
            // 
            // executionStatus
            // 
            this.executionStatus.AutoSize = false;
            this.executionStatus.Name = "executionStatus";
            this.executionStatus.Size = new System.Drawing.Size(200, 17);
            this.executionStatus.Text = "Ready";
            this.executionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timerLabel
            // 
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(70, 17);
            this.timerLabel.Text = "00:00:00.000";
            // 
            // ViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 578);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ViewerForm";
            this.Text = "SQL Server Query Tree Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewerForm_FormClosing);
            this.Load += new System.EventHandler(this.ViewerForm_Load);
            this.mainTabControl.ResumeLayout(false);
            this.tabScript.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage tabScript;
        private System.Windows.Forms.RichTextBox queryRichTextBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentQueriesToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton connectButton;
        private System.Windows.Forms.ToolStripButton executeButton;
        private System.Windows.Forms.ToolStripMenuItem showThumbnailToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel executionStatus;
        private System.Windows.Forms.ToolStripButton cancelQueryButton;
        private System.Windows.Forms.ToolStripStatusLabel timerLabel;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton disconnectButton;
        private System.Windows.Forms.ToolStripComboBox zoomComboBox;
        private System.Windows.Forms.ToolStripComboBox dbContextComboBox;
    }
}

