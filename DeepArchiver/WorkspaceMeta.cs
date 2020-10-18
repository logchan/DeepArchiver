using System.Collections.Generic;

namespace DeepArchiver {
    public sealed class WorkspaceMeta {
        public List<string> Sources { get; set; } = new List<string>();
        public string ConnectionString { get; set; }
    }
}
