using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeepArchiver.Data;
using DeepArchiver.Remote;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace DeepArchiver {
    public sealed class Workspace {
        public string Root { get; set; }
        public List<FileMeta> Files => _files;
        public List<string> Sources => _meta.Sources;
        public bool HasService => _service != null;
        public string ConnectionString => _meta.ConnectionString;
        public string ServiceDescription => _service?.ToString();
        public WorkspaceStats Stats { get; private set; }

        private string MetaFile => Path.Combine(Root, "meta.json");

        private WorkspaceMeta _meta = new WorkspaceMeta();
        private readonly List<FileMeta> _files = new List<FileMeta>();
        private Dictionary<string, FileMeta> _filesMap = new Dictionary<string, FileMeta>();

        private RemoteDb _remote;
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

                var connectionString = $"Data Source={Root}/remote.sqlite3";

                Log.Information($"Connecting database with: {connectionString}");

                var optionBuilder = new DbContextOptionsBuilder<RemoteDb>();
                optionBuilder.UseSqlite(connectionString);

                _remote = new RemoteDb(optionBuilder.Options);
                _remote.Database.EnsureCreated();

                Log.Information("Database ready");

                _db = new JsonDb<DeepArchiverData>(Path.Combine(Root, "database.json"));
                if (_db.Data.RemoteFiles.Count == 0) {
                    Log.Information("Migrate to JSON db...");
                    foreach (var file in _remote.Files.AsNoTracking()) {
                        _db.Data.RemoteFiles.Add(new DbRemoteFile {
                            FullName = file.FullName,
                            Hash = file.Hash,
                            Length = file.Length,
                            Modified = file.Modified,
                        });
                    }

                    SaveDatabase().Wait();
                }

                foreach (var root in _meta.Sources) {
                    Log.Information($"Scan local file in: {root}");
                    var files = ScanFiles(root);
                    AddLocalFiles(files, false);
                }

                Log.Information($"Found {_files.Count} local files");

                var numRemoteOnly = 0;
                foreach (var file in _remote.Files) {
                    if (_filesMap.ContainsKey(file.FullName)) {
                        continue;
                    }

                    numRemoteOnly += 1;
                    _files.Add(new FileMeta {
                        FullName = file.FullName,
                        Modified = file.Modified,
                        Hash = file.Hash,
                        Length = file.Length,
                        Availability = FileAvailability.RemoteOnly,
                        RemoteFile = file
                    });
                }

                Log.Information($"Found {numRemoteOnly} remote-only files");

                SortFiles();
                ComputeStats();
            });
        }

        public async Task AddSource(string path) {
            // TODO: prevent adding child path of existing sources

            _meta.Sources.Add(path);
            SaveMeta();

            await Task.Run(() => {
                Log.Information($"Add source: {path}");
                var files = ScanFiles(path);
                AddLocalFiles(files, true);
                SortFiles();
            });
        }

        public async Task Quit() {
            await _remote.DisposeAsync();
            _remote = null;
        }

        public async Task SaveDatabase() {
            await Task.Run(() => {
                _db.Save();
            });
        }

        public void SetConnectionString(string config) {
            _service = ConfigParser.Parse(config);
            _meta.ConnectionString = config;
            SaveMeta();
        }

        private void SortFiles() {
            _files.Sort((a, b) => Tklc.IO.IOHelpers.FileNameNaturalCompare(a.FullName, b.FullName));
        }

        private void AddLocalFiles(List<FileMeta> files, bool incremental) {
            var allRemoteFiles = _remote.Files.OrderByDescending(f => f.Modified).ToList();
            var remoteFilesMap = new Dictionary<string, RemoteFile>();
            allRemoteFiles.ForEach(f => {
                if (!remoteFilesMap.ContainsKey(f.FullName)) {
                    remoteFilesMap.Add(f.FullName, f);
                }
            });

            foreach (var file in files) {
                // in incremental mode, check if file is already RemoteOnly
                if (incremental) {
                    if (_filesMap.TryGetValue(file.FullName, out var existing)) {
                        existing.Availability = FileSameQuick(remoteFilesMap[file.FullName], file);
                        continue;
                    }
                }

                _filesMap.Add(file.FullName, file);
                _files.Add(file);

                if (!remoteFilesMap.TryGetValue(file.FullName, out var remoteFile)) {
                    file.Availability = FileAvailability.LocalOnly;
                    continue;
                }

                file.Availability = FileSameQuick(remoteFile, file);
                file.RemoteFile = remoteFile;
            }
        }

        private static FileAvailability FileSameQuick(RemoteFile remoteFile, FileMeta file) {
            return remoteFile.Length == file.Length && remoteFile.Modified == file.Modified ? FileAvailability.Synced : FileAvailability.Modified;
        }

        private static List<FileMeta> ScanFiles(string root) {
            var list = new List<FileMeta>();
            var stack = new Stack<string>();

            stack.Push(root);
            while (stack.Count > 0) {
                var dir = new DirectoryInfo(stack.Pop());
                foreach (var subDir in dir.GetDirectories()) {
                    stack.Push(subDir.FullName);
                }

                foreach (var file in dir.GetFiles()) {
                    list.Add(new FileMeta {
                        FullName = file.FullName,
                        Length = file.Length,
                        Modified = file.LastWriteTimeUtc.Ticks,
                    });
                }
            }

            return list;
        }

        public async Task UploadFile(FileMeta file, Action<int> progressCallback) {
            var remoteFile = await _service.UploadFile(file.FullName, _remote, file.RemoteFile, progressCallback);
            file.Hash = remoteFile.Hash;
            file.Length = remoteFile.Length;
            file.Modified = remoteFile.Modified;
            file.Availability = FileAvailability.Synced;
            await Task.Run(ComputeStats);
        }

        private void ComputeStats() {
            var stats = new WorkspaceStats {
                Count = _files.Count
            };
            
            foreach (var file in _files) {
                stats.Counts[file.Availability] += 1;
                stats.Lengths[file.Availability] += file.Length;
                stats.Length += file.Length;
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
