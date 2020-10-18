using DeepArchiver.Data;

namespace DeepArchiver {
    public sealed class FileMeta {
        public string FullName { get; set; }
        public string Hash { get; set; }
        public long Length { get; set; }
        public long Modified { get; set; }
        public FileAvailability Availability { get; set; }
        public RemoteFile RemoteFile { get; set; }

        public bool MarkedAsSkip { get; set; }
    }
}
