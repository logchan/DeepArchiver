namespace DeepArchiver {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstFiles = new System.Windows.Forms.ListView();
            this.colHeaderPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHeaderLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHeaderModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHeaderAvailability = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.connStrBox = new System.Windows.Forms.TextBox();
            this.applyConnStrBtn = new System.Windows.Forms.Button();
            this.remoteDescBox = new System.Windows.Forms.RichTextBox();
            this.uploadBtn = new System.Windows.Forms.Button();
            this.remoteGroup = new System.Windows.Forms.GroupBox();
            this.showRemoteChk = new System.Windows.Forms.CheckBox();
            this.uploadGroup = new System.Windows.Forms.GroupBox();
            this.uploadStatusBox = new System.Windows.Forms.RichTextBox();
            this.allProgress = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.currentProgress = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.sizeLimitUpDown = new System.Windows.Forms.NumericUpDown();
            this.filterGroup = new System.Windows.Forms.GroupBox();
            this.filterBox = new System.Windows.Forms.TextBox();
            this.radioSynced = new System.Windows.Forms.RadioButton();
            this.radioRemoteOnly = new System.Windows.Forms.RadioButton();
            this.radioLocalOnly = new System.Windows.Forms.RadioButton();
            this.statsLbl = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.remoteGroup.SuspendLayout();
            this.uploadGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeLimitUpDown)).BeginInit();
            this.filterGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.workspaceToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1086, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openWorkspaceToolStripMenuItem,
            this.recentToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openWorkspaceToolStripMenuItem
            // 
            this.openWorkspaceToolStripMenuItem.Name = "openWorkspaceToolStripMenuItem";
            this.openWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(250, 34);
            this.openWorkspaceToolStripMenuItem.Text = "Open Workspace";
            this.openWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.openWorkspaceToolStripMenuItem_Click);
            // 
            // recentToolStripMenuItem
            // 
            this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
            this.recentToolStripMenuItem.Size = new System.Drawing.Size(250, 34);
            this.recentToolStripMenuItem.Text = "Recent";
            // 
            // workspaceToolStripMenuItem
            // 
            this.workspaceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourcesToolStripMenuItem});
            this.workspaceToolStripMenuItem.Name = "workspaceToolStripMenuItem";
            this.workspaceToolStripMenuItem.Size = new System.Drawing.Size(115, 29);
            this.workspaceToolStripMenuItem.Text = "Workspace";
            // 
            // sourcesToolStripMenuItem
            // 
            this.sourcesToolStripMenuItem.Name = "sourcesToolStripMenuItem";
            this.sourcesToolStripMenuItem.Size = new System.Drawing.Size(176, 34);
            this.sourcesToolStripMenuItem.Text = "Sources";
            this.sourcesToolStripMenuItem.Click += new System.EventHandler(this.sourcesToolStripMenuItem_Click);
            // 
            // lstFiles
            // 
            this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHeaderPath,
            this.colHeaderLength,
            this.colHeaderModified,
            this.colHeaderAvailability});
            this.lstFiles.FullRowSelect = true;
            this.lstFiles.HideSelection = false;
            this.lstFiles.Location = new System.Drawing.Point(12, 129);
            this.lstFiles.MultiSelect = false;
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(1062, 440);
            this.lstFiles.TabIndex = 5;
            this.lstFiles.UseCompatibleStateImageBehavior = false;
            this.lstFiles.View = System.Windows.Forms.View.Details;
            this.lstFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstFiles_KeyDown);
            // 
            // colHeaderPath
            // 
            this.colHeaderPath.Text = "Full path";
            this.colHeaderPath.Width = 387;
            // 
            // colHeaderLength
            // 
            this.colHeaderLength.Text = "Size";
            this.colHeaderLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colHeaderLength.Width = 100;
            // 
            // colHeaderModified
            // 
            this.colHeaderModified.Text = "Modified";
            this.colHeaderModified.Width = 200;
            // 
            // colHeaderAvailability
            // 
            this.colHeaderAvailability.Text = "Availability";
            this.colHeaderAvailability.Width = 100;
            // 
            // connStrBox
            // 
            this.connStrBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connStrBox.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connStrBox.Location = new System.Drawing.Point(19, 25);
            this.connStrBox.Name = "connStrBox";
            this.connStrBox.PasswordChar = '*';
            this.connStrBox.Size = new System.Drawing.Size(843, 31);
            this.connStrBox.TabIndex = 6;
            // 
            // applyConnStrBtn
            // 
            this.applyConnStrBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.applyConnStrBtn.Location = new System.Drawing.Point(949, 25);
            this.applyConnStrBtn.Name = "applyConnStrBtn";
            this.applyConnStrBtn.Size = new System.Drawing.Size(107, 31);
            this.applyConnStrBtn.TabIndex = 7;
            this.applyConnStrBtn.Text = "Apply";
            this.applyConnStrBtn.UseVisualStyleBackColor = true;
            this.applyConnStrBtn.Click += new System.EventHandler(this.applyConnStrBtn_Click);
            // 
            // remoteDescBox
            // 
            this.remoteDescBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.remoteDescBox.Location = new System.Drawing.Point(19, 62);
            this.remoteDescBox.Name = "remoteDescBox";
            this.remoteDescBox.ReadOnly = true;
            this.remoteDescBox.Size = new System.Drawing.Size(1037, 83);
            this.remoteDescBox.TabIndex = 8;
            this.remoteDescBox.Text = "";
            // 
            // uploadBtn
            // 
            this.uploadBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uploadBtn.Location = new System.Drawing.Point(886, 19);
            this.uploadBtn.Name = "uploadBtn";
            this.uploadBtn.Size = new System.Drawing.Size(170, 31);
            this.uploadBtn.TabIndex = 9;
            this.uploadBtn.Text = "Start";
            this.uploadBtn.UseVisualStyleBackColor = true;
            this.uploadBtn.Click += new System.EventHandler(this.uploadBtn_Click);
            // 
            // remoteGroup
            // 
            this.remoteGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.remoteGroup.Controls.Add(this.showRemoteChk);
            this.remoteGroup.Controls.Add(this.connStrBox);
            this.remoteGroup.Controls.Add(this.applyConnStrBtn);
            this.remoteGroup.Controls.Add(this.remoteDescBox);
            this.remoteGroup.Location = new System.Drawing.Point(12, 575);
            this.remoteGroup.Name = "remoteGroup";
            this.remoteGroup.Size = new System.Drawing.Size(1062, 160);
            this.remoteGroup.TabIndex = 10;
            this.remoteGroup.TabStop = false;
            this.remoteGroup.Text = "Remote";
            // 
            // showRemoteChk
            // 
            this.showRemoteChk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showRemoteChk.AutoSize = true;
            this.showRemoteChk.Location = new System.Drawing.Point(868, 29);
            this.showRemoteChk.Name = "showRemoteChk";
            this.showRemoteChk.Size = new System.Drawing.Size(75, 24);
            this.showRemoteChk.TabIndex = 9;
            this.showRemoteChk.Text = "Show";
            this.showRemoteChk.UseVisualStyleBackColor = true;
            this.showRemoteChk.CheckedChanged += new System.EventHandler(this.showRemoteChk_CheckedChanged);
            // 
            // uploadGroup
            // 
            this.uploadGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uploadGroup.Controls.Add(this.uploadStatusBox);
            this.uploadGroup.Controls.Add(this.allProgress);
            this.uploadGroup.Controls.Add(this.label3);
            this.uploadGroup.Controls.Add(this.label2);
            this.uploadGroup.Controls.Add(this.currentProgress);
            this.uploadGroup.Controls.Add(this.label1);
            this.uploadGroup.Controls.Add(this.sizeLimitUpDown);
            this.uploadGroup.Controls.Add(this.uploadBtn);
            this.uploadGroup.Location = new System.Drawing.Point(12, 742);
            this.uploadGroup.Name = "uploadGroup";
            this.uploadGroup.Size = new System.Drawing.Size(1062, 214);
            this.uploadGroup.TabIndex = 11;
            this.uploadGroup.TabStop = false;
            this.uploadGroup.Text = "Upload";
            // 
            // uploadStatusBox
            // 
            this.uploadStatusBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uploadStatusBox.Location = new System.Drawing.Point(19, 120);
            this.uploadStatusBox.Name = "uploadStatusBox";
            this.uploadStatusBox.ReadOnly = true;
            this.uploadStatusBox.Size = new System.Drawing.Size(1037, 83);
            this.uploadStatusBox.TabIndex = 9;
            this.uploadStatusBox.Text = "";
            // 
            // allProgress
            // 
            this.allProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.allProgress.Location = new System.Drawing.Point(143, 91);
            this.allProgress.Name = "allProgress";
            this.allProgress.Size = new System.Drawing.Size(913, 23);
            this.allProgress.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "Overall:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "Current file:";
            // 
            // currentProgress
            // 
            this.currentProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentProgress.Location = new System.Drawing.Point(143, 62);
            this.currentProgress.Name = "currentProgress";
            this.currentProgress.Size = new System.Drawing.Size(913, 23);
            this.currentProgress.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Stop after (GB):";
            // 
            // sizeLimitUpDown
            // 
            this.sizeLimitUpDown.Location = new System.Drawing.Point(143, 28);
            this.sizeLimitUpDown.Name = "sizeLimitUpDown";
            this.sizeLimitUpDown.Size = new System.Drawing.Size(120, 26);
            this.sizeLimitUpDown.TabIndex = 10;
            // 
            // filterGroup
            // 
            this.filterGroup.Controls.Add(this.filterBox);
            this.filterGroup.Controls.Add(this.radioSynced);
            this.filterGroup.Controls.Add(this.radioRemoteOnly);
            this.filterGroup.Controls.Add(this.radioLocalOnly);
            this.filterGroup.Location = new System.Drawing.Point(12, 36);
            this.filterGroup.Name = "filterGroup";
            this.filterGroup.Size = new System.Drawing.Size(1062, 58);
            this.filterGroup.TabIndex = 12;
            this.filterGroup.TabStop = false;
            this.filterGroup.Text = "Filter";
            // 
            // filterBox
            // 
            this.filterBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterBox.Location = new System.Drawing.Point(357, 23);
            this.filterBox.Name = "filterBox";
            this.filterBox.Size = new System.Drawing.Size(699, 26);
            this.filterBox.TabIndex = 3;
            this.filterBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.filterBox_KeyDown);
            // 
            // radioSynced
            // 
            this.radioSynced.AutoSize = true;
            this.radioSynced.Location = new System.Drawing.Point(264, 25);
            this.radioSynced.Name = "radioSynced";
            this.radioSynced.Size = new System.Drawing.Size(87, 24);
            this.radioSynced.TabIndex = 2;
            this.radioSynced.Text = "Synced";
            this.radioSynced.UseVisualStyleBackColor = true;
            // 
            // radioRemoteOnly
            // 
            this.radioRemoteOnly.AutoSize = true;
            this.radioRemoteOnly.Location = new System.Drawing.Point(132, 25);
            this.radioRemoteOnly.Name = "radioRemoteOnly";
            this.radioRemoteOnly.Size = new System.Drawing.Size(126, 24);
            this.radioRemoteOnly.TabIndex = 1;
            this.radioRemoteOnly.Text = "Remote Only";
            this.radioRemoteOnly.UseVisualStyleBackColor = true;
            // 
            // radioLocalOnly
            // 
            this.radioLocalOnly.AutoSize = true;
            this.radioLocalOnly.Checked = true;
            this.radioLocalOnly.Location = new System.Drawing.Point(19, 25);
            this.radioLocalOnly.Name = "radioLocalOnly";
            this.radioLocalOnly.Size = new System.Drawing.Size(107, 24);
            this.radioLocalOnly.TabIndex = 0;
            this.radioLocalOnly.TabStop = true;
            this.radioLocalOnly.Text = "Local Only";
            this.radioLocalOnly.UseVisualStyleBackColor = true;
            // 
            // statsLbl
            // 
            this.statsLbl.AutoSize = true;
            this.statsLbl.Location = new System.Drawing.Point(12, 106);
            this.statsLbl.Name = "statsLbl";
            this.statsLbl.Size = new System.Drawing.Size(47, 20);
            this.statsLbl.TabIndex = 13;
            this.statsLbl.Text = "Stats";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 970);
            this.Controls.Add(this.statsLbl);
            this.Controls.Add(this.filterGroup);
            this.Controls.Add(this.uploadGroup);
            this.Controls.Add(this.remoteGroup);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Deep Archiver";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.remoteGroup.ResumeLayout(false);
            this.remoteGroup.PerformLayout();
            this.uploadGroup.ResumeLayout(false);
            this.uploadGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeLimitUpDown)).EndInit();
            this.filterGroup.ResumeLayout(false);
            this.filterGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWorkspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sourcesToolStripMenuItem;
        private System.Windows.Forms.ListView lstFiles;
        private System.Windows.Forms.ColumnHeader colHeaderPath;
        private System.Windows.Forms.ColumnHeader colHeaderLength;
        private System.Windows.Forms.ColumnHeader colHeaderAvailability;
        private System.Windows.Forms.ColumnHeader colHeaderModified;
        private System.Windows.Forms.TextBox connStrBox;
        private System.Windows.Forms.Button applyConnStrBtn;
        private System.Windows.Forms.RichTextBox remoteDescBox;
        private System.Windows.Forms.Button uploadBtn;
        private System.Windows.Forms.GroupBox remoteGroup;
        private System.Windows.Forms.GroupBox uploadGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown sizeLimitUpDown;
        private System.Windows.Forms.ProgressBar allProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar currentProgress;
        private System.Windows.Forms.RichTextBox uploadStatusBox;
        private System.Windows.Forms.GroupBox filterGroup;
        private System.Windows.Forms.Label statsLbl;
        private System.Windows.Forms.CheckBox showRemoteChk;
        private System.Windows.Forms.RadioButton radioSynced;
        private System.Windows.Forms.RadioButton radioRemoteOnly;
        private System.Windows.Forms.RadioButton radioLocalOnly;
        private System.Windows.Forms.TextBox filterBox;
    }
}

