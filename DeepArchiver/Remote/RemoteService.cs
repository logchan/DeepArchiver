using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DeepArchiver.Data;

namespace DeepArchiver.Remote {
    public abstract class RemoteService {
        public async Task<RemoteFileInfo> UploadFile(LocalFileInfo localFile, JsonDb<DeepArchiverData> db, Action<int> progressCallback) {
            // progress mapping:
            // - 0 ~ 10% hashing
            // - 10 ~ 100% uploading
            var path = localFile.FullName;
            var fi = new FileInfo(path);
            if (!fi.Exists) {
                throw new FileNotFoundException(path);
            }

            progressCallback(1);
            var hash = await FileHash(path);
            progressCallback(10);

            await Upload(path, hash, p => progressCallback(10 + (int)(p * 0.9)));

            var dbFile = new DbRemoteFile {
                FullName = path,
                Hash = hash,
                Length = fi.Length,
                Modified = fi.LastWriteTimeUtc.Ticks,
            };
            db.Data.RemoteFiles.Add(dbFile);
            db.Save();

            return new RemoteFileInfo {
                FullName = path,
                Hash = hash,
                Length = dbFile.Length,
                Modified = dbFile.Modified,
                LocalFile = localFile,
            };
        }

        protected abstract Task Upload(string path, string hash, Action<int> progressCallback);

        private static async Task<string> FileHash(string path) {
            return await Task.Run(() => {
                using (var sha = SHA256.Create())
                using (var fs = File.OpenRead(path)) {
                    var hash = sha.ComputeHash(fs);
                    return String.Concat(hash.Select(b => b.ToString("x2")));
                }
            });
        }
    }
}
