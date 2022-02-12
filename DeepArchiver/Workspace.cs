using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeepArchiver.Data;
using DeepArchiver.Remote;
using Newtonsoft.Json;
using Serilog;

namespace DeepArchiver {
    public sealed class Workspace {
        public string Root { get; set; }

        public List<LocalFileInfo> LocalFiles { get; } = new List<LocalFileInfo>();
        public List<RemoteFileInfo> RemoteFiles { get; } = new List<RemoteFileInfo>();

        public List<string> Sources => _meta.Sources;
        public bool HasService => _service != null;
        public string ConnectionString => _meta.ConnectionString;
        public string ServiceDescription => _service?.ToString();
        public WorkspaceStats Stats { get; private set; }

        private string MetaFile => Path.Combine(Root, "meta.json");

        private WorkspaceMeta _meta = new WorkspaceMeta();
        private readonly Dictionary<string, LocalFileInfo> _localFilesMap = new Dictionary<string, LocalFileInfo>();
        private readonly Dictionary<string, List<RemoteFileInfo>> _remoteFilesMap = new Dictionary<string, List<RemoteFileInfo>>();

        private RemoteService _service;
        private JsonDb<DeepArchiverData> _db;

        public Workspace(string root) {
            if (!Directory.Exists(root)) {
                Directory.CreateDirectory(root);
            }

            Root = root;
        }

        public async Task Initialize() {
            LoadMeta();
            if (!String.IsNullOrEmpty(_meta.ConnectionString)) {
                _service = ConfigParser.Parse(_meta.ConnectionString);
            }

            await Task.Run(() => {
                _db = new JsonDb<DeepArchiverData>(Path.Combine(Root, "database.json"));

                // register remote files
                foreach (var file in _db.Data.RemoteFiles.Select(f => new RemoteFileInfo {
                    FullName = f.FullName,
                    Hash = f.Hash,
                    Length = f.Length,
                    Modified = f.Modified,
                })) {
                    AddRemoteFile(file);
                }

                foreach (var list in _remoteFilesMap.Values) {
                    list.Sort((a, b) => b.Modified.CompareTo(a.Modified));
                }

                RemoteFiles.Sort((a, b) => Tklc.IO.IOHelpers.FileNameNaturalCompare(a.FullName, b.FullName));

                // register local files
                foreach (var root in _meta.Sources) {
                    Log.Information($"Scan local file in: {root}");
                    AddLocalFiles(ScanFiles(root));
                }
                
                ComputeStats();
            });
        }

        public async Task AddSource(string path) {
            // TODO: prevent adding child path of existing sources

            _meta.Sources.Add(path);
            SaveMeta();

            await Task.Run(() => {
                Log.Information($"Add source: {path}");
                AddLocalFiles(ScanFiles(path));
            });
        }

        public void SetConnectionString(string config) {
            _service = ConfigParser.Parse(config);
            _meta.ConnectionString = config;
            SaveMeta();
        }

        private void AddRemoteFile(RemoteFileInfo file) {
            RemoteFiles.Add(file);
            if (!_remoteFilesMap.TryGetValue(file.FullName, out var list)) {
                list = new List<RemoteFileInfo>();
                _remoteFilesMap[file.FullName] = list;
            }

            list.Add(file);
        }

        private void AddLocalFiles(List<LocalFileInfo> files) {
            foreach (var file in files) {
                LocalFiles.Add(file);
                _localFilesMap[file.FullName] = file;

                if (!_remoteFilesMap.TryGetValue(file.FullName, out var list)) {
                    continue;
                }

                var remoteFile = list.FirstOrDefault(f => f.Length == file.Length && f.Modified == file.Modified);
                if (remoteFile != null) {
                    remoteFile.LocalFile = file;
                    file.RemoteFile = remoteFile;
                }
            }

            LocalFiles.Sort((a, b) => Tklc.IO.IOHelpers.FileNameNaturalCompare(a.FullName, b.FullName));
        }
        
        private static List<LocalFileInfo> ScanFiles(string root) {
            var list = new List<LocalFileInfo>();
            var stack = new Stack<string>();

            stack.Push(root);
            while (stack.Count > 0) {
                var dir = new DirectoryInfo(stack.Pop());
                foreach (var subDir in dir.GetDirectories()) {
                    stack.Push(subDir.FullName);
                }

                foreach (var file in dir.GetFiles()) {
                    list.Add(new LocalFileInfo {
                        FullName = file.FullName,
                        Length = file.Length,
                        Modified = file.LastWriteTimeUtc.Ticks,
                    });
                }
            }

            return list;
        }

        public async Task UploadFile(LocalFileInfo file, Action<int> progressCallback) {
            var remoteFile = await _service.UploadFile(file, _db, progressCallback);
            AddRemoteFile(remoteFile);
            await Task.Run(ComputeStats);
        }

        public async Task DeleteFile(RemoteFileInfo file) {
            await _service.DeleteFile(file, _db);
            RemoteFiles.Remove(file);
            _remoteFilesMap[file.FullName].Remove(file);

            if (_localFilesMap.TryGetValue(file.FullName, out var localFile)) {
                if (ReferenceEquals(localFile.RemoteFile, file)) {
                    localFile.RemoteFile = null;
                }
            }
            await Task.Run(ComputeStats);
        }

        private void ComputeStats() {
            var stats = new WorkspaceStats();
            
            foreach (var file in LocalFiles) {
                stats.Counts[file.Availability] += 1;
                stats.Lengths[file.Availability] += file.Length;
            }

            foreach (var file in RemoteFiles) {
                if (file.Availability == FileAvailability.Synced) {
                    continue;
                }

                stats.Counts[file.Availability] += 1;
                stats.Lengths[file.Availability] += file.Length;
            }

            Stats = stats;
        }

        #region Metadata

        private void LoadMeta() {
            try {
                if (!File.Exists(MetaFile)) {
                    return;
                }

                var s = File.ReadAllText(MetaFile);
                _meta = JsonConvert.DeserializeObject<WorkspaceMeta>(s);
            }
            catch (Exception ex) {
                Log.Error(ex, $"Failed to read meta json {MetaFile}");
            }
        }

        private void SaveMeta() {
            try {
                var s = JsonConvert.SerializeObject(_meta);
                File.WriteAllText(MetaFile, s);
            }
            catch (Exception ex) {
                Log.Error(ex, $"Failed to save meta json {MetaFile}");
            }
        }

        #endregion
    }
}
