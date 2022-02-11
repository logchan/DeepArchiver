using System.Collections.Generic;

namespace DeepArchiver.Data {
    public sealed class DeepArchiverData {
        public List<DbRemoteFile> RemoteFiles { get; set; } = new List<DbRemoteFile>();
    }
}
