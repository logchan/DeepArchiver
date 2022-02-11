using System;

namespace DeepArchiver {
    internal sealed class AppState {
        public string Workspace { get; set; } = String.Empty;
        public WindowPosition LastPosition { get; set; } = new WindowPosition();
    }
}
