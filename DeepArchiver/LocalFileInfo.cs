namespace DeepArchiver {
    public sealed class LocalFileInfo {
        public string FullName { get; set; }
        public long Length { get; set; }
        public long Modified { get; set; }

        public RemoteFileInfo RemoteFile { get; set; }
        public FileAvailability Availability => RemoteFile == null ? FileAvailability.LocalOnly : FileAvailability.Synced;
    }
}
