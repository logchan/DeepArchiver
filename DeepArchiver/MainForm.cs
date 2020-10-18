using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepArchiver {
    public partial class MainForm : Form {
        private string StateFile => Path.Combine(Program.AppDataPath, "state.json");
        private readonly AppState _state;

        private Workspace _workspace;
        private readonly Dictionary<FileAvailability, CheckBox> _filterCheckboxes = new Dictionary<FileAvailability, CheckBox>();

        public MainForm() {
            InitializeComponent();

            _filterCheckboxes.Add(FileAvailability.Synced, chkSynced);
            _filterCheckboxes.Add(FileAvailability.Modified, chkModified);
            _filterCheckboxes.Add(FileAvailability.LocalOnly, chkLocalOnly);
            _filterCheckboxes.Add(FileAvailability.RemoteOnly, chkRemoteOnly);

            _state = LoadState();
            _state.LastPosition.Restore(this);
        }

        #region States

        private void SetTitle(string content) {
            Text = $"{content} - Deep Archiver";
        }

        private void SetTitleWithWorkspace() {
            SetTitle(_workspace?.Root ?? "(no workspace)");
        }

        private AppState LoadState() {
            try {
                if (!File.Exists(StateFile)) {
                    Log.Information("State file does not exist, create new");
                    return new AppState();
                }

                var s = File.ReadAllText(StateFile);
                return JsonConvert.DeserializeObject<AppState>(s);
            }
            catch (Exception ex) {
                Log.Error(ex, "Failed to load state file");
                return new AppState();
            }
        }

        private void SaveState() {
            var s = JsonConvert.SerializeObject(_state);
            File.WriteAllText(StateFile, s);
            Log.Information("Saved state to file");
        }

        #endregion

        #region Workspace

        private async Task OpenWorkspace(string path) {
            await QuitWorkspace();

            try {
                var ws = new Workspace(path);
                await ws.Initialize();

                _workspace = ws;
                SetTitleWithWorkspace();
                SetStatsString();

                RefreshList();
                connStrBox.Text = _workspace.ConnectionString ?? String.Empty;
                remoteDescBox.Text = _workspace.ServiceDescription;
            }
            catch (Exception ex) {
                Log.Error(ex, $"Failed to open workspace {path}");
                MessageBox.Show($"Failed to open workspace {path}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task QuitWorkspace() {
            try {
                if (_workspace != null) {
                    await _workspace.Quit();
                }
            }
            catch (Exception ex) {
                Log.Error(ex, "Failed to quit workspace (discarding it anyway)");
            }

            SetTitleWithWorkspace();
            _workspace = null;
        }

        #endregion

        private async void openWorkspaceToolStripMenuItem_Click(object sender, EventArgs e) {
            var dialog = new CommonOpenFileDialog {
                InitialDirectory = _workspace?.Root ??
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                IsFolderPicker = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                await OpenWorkspace(dialog.FileName);
            }
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            _state.LastPosition.Update(this);
            _state.Workspace = _workspace?.Root ?? String.Empty;
            SaveState();

            await QuitWorkspace();
        }

        private async void MainForm_Shown(object sender, EventArgs e) {
            ResizeColumns();

            if (!String.IsNullOrEmpty(_state.Workspace)) {
                await OpenWorkspace(_state.Workspace);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e) {
            ResizeColumns();
        }

        private void ResizeColumns() {
            colHeaderPath.Width = lstFiles.Width - 440;
        }

        private void filters_CheckedChanged(object sender, EventArgs e) {
            RefreshList();
        }

        private void RefreshList() {
            if (_workspace == null) {
                return;
            }

            lstFiles.Items.Clear();

            var shownAvailability = (from pair in _filterCheckboxes
                                    where pair.Value.Checked
                                    select pair.Key).ToList();
            foreach (var file in _workspace.Files.Where(f => shownAvailability.Contains(f.Availability))) {
                var item = new ListViewItem() {
                    Tag = file
                };
                ApplyListViewItem(item, file);
                lstFiles.Items.Add(item);
            }
        }

        private static void ApplyListViewItem(ListViewItem item, FileMeta file) {
            item.SubItems.Clear();
            item.Text = file.FullName;
            item.SubItems.AddRange(new[] {
                GetSizeString(file.Length),
                new DateTime(file.Modified).ToString("yyyy-MM-dd HH:mm:ss"),
                file.Availability.ToString()
            });
            SetColor(item, false);
        }

        private void sourcesToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_workspace == null) {
                return;
            }

            var frm = new SourcesForm(_workspace);
            frm.ShowDialog();

            RefreshList();
        }

        private void SetStatsString() {
            var order = new[] { FileAvailability.Synced, FileAvailability.LocalOnly, FileAvailability.Modified, FileAvailability.RemoteOnly };
            var sb = new StringBuilder();
            var stats = _workspace.Stats;
            sb.Append($"Total: {stats.Count} ({GetSizeString(stats.Length)})");
            foreach (var v in order) {
                sb.Append($", {v}: {stats.Counts[v]} ({GetSizeString(stats.Lengths[v])})");
            }

            statsLbl.Text = sb.ToString();
        }

        private static string GetSizeString(long length) {
            var (num, unit) = Tklc.Framework.Helpers.StringHelpers.ConvertUnits(length, 1024, "B", "KB", "MB", "GB");
            return $"{num:F2}{unit}";
        }

        private void applyConnStrBtn_Click(object sender, EventArgs e) {
            if (_workspace == null) {
                return;
            }

            try {
                _workspace.SetConnectionString(connStrBox.Text);
                remoteDescBox.Text = _workspace.ServiceDescription;
            }
            catch (Exception ex) {
                remoteDescBox.Text = $"Failed to apply config: {ex.Message}";
            }
        }

        private bool _uploading = false;
        private bool _stopUpload = false;
        private Task _uploadTask = null;

        private async void uploadBtn_Click(object sender, EventArgs e) {
            if (_workspace == null || !_workspace.HasService) {
                return;
            }

            uploadBtn.Enabled = false;

            try {
                if (_uploading) {
                    await StopUpload();
                }
                else {
                    StartUpload();
                }
            }
            catch (Exception ex) {
                uploadStatusBox.Text = ex.Message;
            }

            uploadBtn.Enabled = true;
        }

        private void StartUpload() {
            _uploading = true;
            _stopUpload = false;
            filterGroup.Enabled = false;
            remoteGroup.Enabled = false;
            uploadBtn.Text = "Stop";

            var queue = new List<Tuple<FileMeta, ListViewItem>>();
            var limit = Convert.ToInt64(sizeLimitUpDown.Value) * (1 << 30);
            var totalSize = 0L;
            foreach (ListViewItem item in lstFiles.Items) {
                var file = (FileMeta) item.Tag;
                if (file.MarkedAsSkip) {
                    continue;
                }

                queue.Add(new Tuple<FileMeta, ListViewItem>(file, item));
                SetColor(item, true);

                totalSize += file.Length;
                if (limit > 0 && totalSize > limit) {
                    break;
                }
            }

            var header = $"Uploading {queue.Count} files, total size {GetSizeString(totalSize)}";
            var uploadedSize = 0L;
            var numUploaded = 0;
            var progress = 0;
            _uploadTask = Task.Run(async () => {
                foreach (var (file, item) in queue) {
                    if (_stopUpload) {
                        break;
                    }

                    var subHeader = $"Remaining {queue.Count - numUploaded} files, size {GetSizeString(totalSize - uploadedSize)}";
                    await _workspace.UploadFile(file, p => { Invoke(new Action(() => {
                        var nl = Environment.NewLine;
                        var action = p < 10 ? "Hashing" : "Transferring";
                        uploadStatusBox.Text = $"{header}{nl}{subHeader}{nl}{action} {file.FullName}";
                        currentProgress.Value = p;
                    })); });

                    uploadedSize += file.Length;
                    numUploaded += 1;
                    var newProgress = (int) (uploadedSize * 100 / totalSize);
                    if (newProgress > progress) {
                        progress = newProgress;
                    }

                    Invoke(new Action(() => {
                        allProgress.Value = newProgress;
                        ApplyListViewItem(item, file);
                        SetStatsString();
                    }));
                }

                Invoke(new Action(() => {
                    _uploading = false;
                    filterGroup.Enabled = true;
                    remoteGroup.Enabled = true;
                    uploadBtn.Text = "Start";
                    RefreshList();
                }));
            });
        }

        private async Task StopUpload() {
            _stopUpload = true;
            await _uploadTask;
        }

        private void showRemoteChk_CheckedChanged(object sender, EventArgs e) {
            connStrBox.PasswordChar = showRemoteChk.Checked ? '\0' : '*';
        }

        private void lstFiles_KeyPress(object sender, KeyPressEventArgs e) {
            if (lstFiles.SelectedItems.Count == 0) {
                return;
            }

            var selected = lstFiles.SelectedItems[0];
            var file = (FileMeta) selected.Tag;
            file.MarkedAsSkip = !file.MarkedAsSkip;
            SetColor(selected, false);
            lstFiles.SelectedIndices.Clear();
        }

        private static void SetColor(ListViewItem item, bool markUpload) {
            if (markUpload) {
                item.ForeColor = Color.DarkGreen;
                return;
            }

            var file = (FileMeta) item.Tag;
            item.ForeColor = file.MarkedAsSkip ? Color.LightGray : Color.Black;
        }
    }
}
