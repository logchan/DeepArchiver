using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Serilog;

namespace DeepArchiver.Data {
    public sealed class JsonDb<T> where T: new() {
        private readonly string _file;
        
        public T Data { get; }

        public JsonDb(string file) {
            _file = file;

            try {
                if (File.Exists(file)) {
                    using (var ifs = File.OpenRead(file))
                    using (var reader = new StreamReader(ifs, Encoding.UTF8)) {
                        Data = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                    }
                }
                else {
                    Data = new T();
                }
            }
            catch (Exception ex) {
                Log.Error(ex, "Failed to initialize database");
                throw;
            }
        }

        public void Save() {
            var lastFile = _file + "-last";
            if (File.Exists(_file)) {
                if (File.Exists(lastFile)) {
                    File.Delete(lastFile);
                }
                File.Move(_file, lastFile);
            }

            try {
                var json = JsonConvert.SerializeObject(Data);
                using (var ofs = File.Open(_file, FileMode.Create, FileAccess.Write))
                using (var writer = new StreamWriter(ofs, Encoding.UTF8)) {
                    writer.Write(json);
                }
            }
            catch (Exception ex) {
                Log.Error(ex, "Failed to save database");
                throw;
            }
        }
    }
}
