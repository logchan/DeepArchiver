using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepArchiver.Remote {
    public sealed class LocalFileRemote : RemoteService {
        private const int BufferSize = 1 << 20;

        public string Root { get; set; }

        protected override async Task Upload(string path, string hash, Action<int> progressCallback) {
            var dir = Path.Combine(Root, hash.Substring(0, 2));
            Directory.CreateDirectory(dir);

            var file = Path.Combine(dir, hash);
            var progress = 0;
            await Task.Run(() => {
                using (var ofs = new FileStream(file, FileMode.Create, FileAccess.Write))
                using (var ifs = File.OpenRead(path)) {
                    var buffer = new byte[BufferSize];
                    var read = 0;
                    var total = 0L;
                    do {
                        read = ifs.Read(buffer, 0, BufferSize);
                        ofs.Write(buffer, 0, read);
                        
                        total += read;
                        if (total * 100 / ifs.Length > progress) {
                            progress = (int)(total * 100 / ifs.Length);
                            progressCallback(progress);
                        }
                    } while (read != 0);
                }
            });
        }

        public override string ToString() {
            return $"LocalFileRemote at {Root}";
        }
    }
}
