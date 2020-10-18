namespace DeepArchiver {
    partial class SourcesForm {
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
            this.lstSources = new System.Windows.Forms.ListView();
            this.btnAddSource = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstSources
            // 
            this.lstSources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSources.HideSelection = false;
            this.lstSources.Location = new System.Drawing.Point(12, 12);
            this.lstSources.Name = "lstSources";
            this.lstSources.Size = new System.Drawing.Size(475, 321);
            this.lstSources.TabIndex = 0;
            this.lstSources.UseCompatibleStateImageBehavior = false;
            this.lstSources.View = System.Windows.Forms.View.List;
            // 
            // btnAddSource
            // 
            this.btnAddSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddSource.Location = new System.Drawing.Point(13, 340);
            this.btnAddSource.Name = "btnAddSource";
            this.btnAddSource.Size = new System.Drawing.Size(215, 61);
            this.btnAddSource.TabIndex = 1;
            this.btnAddSource.Text = "Add";
            this.btnAddSource.UseVisualStyleBackColor = true;
            this.btnAddSource.Click += new System.EventHandler(this.btnAddSource_Click);
            // 
            // SourcesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 413);
            this.Controls.Add(this.btnAddSource);
            this.Controls.Add(this.lstSources);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SourcesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sources";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstSources;
        private System.Windows.Forms.Button btnAddSource;
    }
}