using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepArchiver.Data {
    public sealed class RemoteFile {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Hash { get; set; }
        public long Length { get; set; }
        public long Modified { get; set; }
    }
}
