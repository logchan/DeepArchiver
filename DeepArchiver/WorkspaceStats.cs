using System;
using System.Collections.Generic;

namespace DeepArchiver {
    public sealed class WorkspaceStats {
        public Dictionary<FileAvailability, int> Counts { get; set; } = new Dictionary<FileAvailability, int>();
        public Dictionary<FileAvailability, long> Lengths { get; set; } = new Dictionary<FileAvailability, long>();

        public WorkspaceStats() {
            foreach (FileAvailability v in Enum.GetValues(typeof(FileAvailability))) {
                Counts.Add(v, 0);
                Lengths.Add(v, 0);
            }
        }
    }
}
