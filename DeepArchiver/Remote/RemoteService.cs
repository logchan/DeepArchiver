using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DeepArchiver.Data;

namespace DeepArchiver.Remote {
    public abstract class RemoteService {
        public async Task<RemoteFile> UploadFile(string path, RemoteDb db, RemoteFile fileInfo, Action<int> progressCallback) {
            // progress mapping:
            // - 0 ~ 10% hashing
            // - 10 ~ 100% uploading

            var fi = new FileInfo(path);
            if (!fi.Exists) {
                throw new FileNotFoundException(path);
            }

            progressCallback(1);
            var hash = await FileHash(path);
            progressCallback(10);

            await Upload(path, hash, p => progressCallback(10 + (int)(p * 0.9)));

            if (fileInfo == null) {
                fileInfo = new RemoteFile();
                db.Files.Add(fileInfo);
            }

            fileInfo.FullName = path;
            fileInfo.Length = fi.Length;
            fileInfo.Modified = fi.LastWriteTimeUtc.Ticks;
            fileInfo.Hash = hash;

            await db.SaveChangesAsync();
            return fileInfo;
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
