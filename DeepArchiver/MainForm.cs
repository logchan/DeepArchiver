using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepArchiver {
    public partial class MainForm : Form {
        private string StateFile => Path.Combine(Program.AppDataPath, "state.json");
        private readonly AppState _state;

        private Workspace _workspace;
        private FileAvailability _shownAvailability;

        public MainForm() {
            InitializeComponent();

            var availRadios = new List<Tuple<RadioButton, FileAvailability>> {
                new Tuple<RadioButton, FileAvailability>(radioLocalOnly, FileAvailability.LocalOnly),
                new Tuple<RadioButton, FileAvailability>(radioRemoteOnly, FileAvailability.RemoteOnly),
                new Tuple<RadioButton, FileAvailability>(radioSynced, FileAvailability.Synced)
            };
            availRadios.ForEach(tuple => {
                tuple.Item1.CheckedChanged += (ev, sender) => {
                    if (tuple.Item1.Checked) {
                        _shownAvailability = tuple.Item2;
                        uploadBtn.Enabled = _shownAvailability == FileAvailability.LocalOnly;
                        RefreshList();
                    }
                };
            });

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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            _state.LastPosition.Update(this);
            _state.Workspace = _workspace?.Root ?? String.Empty;
            SaveState();
            Log.CloseAndFlush();
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
        
        private void RefreshList() {
            if (_workspace == null) {
                return;
            }

            lstFiles.Items.Clear();

            var search = filterBox.Text;
            if (_shownAvailability == FileAvailability.RemoteOnly) {
                foreach (var file in _workspace.RemoteFiles.Where(f => f.Availability == FileAvailability.RemoteOnly && 
                                                                       (String.IsNullOrWhiteSpace(search) || f.FullName.ToLowerInvariant().Contains(search)))) {
                    var item = new ListViewItem {
                        Tag = file
                    };
                    ApplyListViewItem(item, file.FullName, file.Length, file.Modified, file.Availability);
                    lstFiles.Items.Add(item);
                }
            }
            else {
                foreach (var file in _workspace.LocalFiles.Where(f => f.Availability == _shownAvailability &&
                                                                      (String.IsNullOrWhiteSpace(search) || f.FullName.ToLowerInvariant().Contains(search)))) {
                    var item = new ListViewItem {
                        Tag = file
                    };
                    ApplyListViewItem(item, file.FullName, file.Length, file.Modified, file.Availability);
                    lstFiles.Items.Add(item);
                }
            }
        }

        private static void ApplyListViewItem(ListViewItem item, string fullName, long length, long modified, FileAvailability availability) {
            item.SubItems.Clear();
            item.Text = fullName;
            item.SubItems.AddRange(new[] {
                GetSizeString(length),
                new DateTime(modified).ToString("yyyy-MM-dd HH:mm:ss"),
                availability.ToString()
            });
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
            var order = new[] { FileAvailability.Synced, FileAvailability.LocalOnly, FileAvailability.RemoteOnly };
            var stats = _workspace.Stats;

            statsLbl.Text = String.Join(" | ", order.Select(v => $"{v}: {stats.Counts[v]} ({GetSizeString(stats.Lengths[v])})"));
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

            var queue = new List<Tuple<LocalFileInfo, ListViewItem>>();
            var limit = Convert.ToInt64(sizeLimitUpDown.Value) * (1 << 30);
            var totalSize = 0L;
            foreach (ListViewItem item in lstFiles.Items) {
                var file = (LocalFileInfo) item.Tag;
                if (file.Availability == FileAvailability.RemoteOnly || file.Availability == FileAvailability.Synced) {
                    continue;
                }

                queue.Add(new Tuple<LocalFileInfo, ListViewItem>(file, item));
                item.ForeColor = Color.DarkGreen;

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
                        ApplyListViewItem(item, file.FullName, file.Length, file.Modified, file.Availability);
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

        private async void lstFiles_KeyDown(object sender, KeyEventArgs e) {
            if (lstFiles.SelectedItems.Count == 0) {
                return;
            }

            if (_shownAvailability == FileAvailability.RemoteOnly && e.Shift && e.KeyCode == Keys.Delete) {
                await _workspace.DeleteFile((RemoteFileInfo) lstFiles.SelectedItems[0].Tag);
                RefreshList();
                SetStatsString();
            }
        }

        private void filterBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                RefreshList();
            }
        }
    }
}
