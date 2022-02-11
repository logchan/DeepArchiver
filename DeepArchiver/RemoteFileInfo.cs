namespace DeepArchiver {
    public sealed class RemoteFileInfo {
        public string FullName { get; set; }
        public string Hash { get; set; }
        public long Length { get; set; }
        public long Modified { get; set; }

        public LocalFileInfo LocalFile { get; set; }
        public FileAvailability Availability => LocalFile == null ? FileAvailability.RemoteOnly : FileAvailability.Synced;
    }
}
