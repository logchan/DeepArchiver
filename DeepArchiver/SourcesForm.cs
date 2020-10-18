using System;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DeepArchiver {
    public partial class SourcesForm : Form {
        private readonly Workspace _workspace;

        public SourcesForm(Workspace workspace) {
            _workspace = workspace;

            InitializeComponent();

            foreach (var src in _workspace.Sources) {
                lstSources.Items.Add(src);
            }
        }

        private async void btnAddSource_Click(object sender, EventArgs e) {
            var dialog = new CommonOpenFileDialog {
                IsFolderPicker = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                Cursor.Current = Cursors.WaitCursor;
                btnAddSource.Enabled = false;

                await _workspace.AddSource(dialog.FileName);
                lstSources.Items.Add(dialog.FileName);

                btnAddSource.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
