namespace DeepArchiver.Data {
    public sealed class DbRemoteFile {
        public string FullName { get; set; }
        public string Hash { get; set; }
        public long Length { get; set; }
        public long Modified { get; set; }
    }
}
