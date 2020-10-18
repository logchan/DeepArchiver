using System.Windows.Forms;

namespace DeepArchiver {
    public sealed class WindowPosition {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public FormWindowState WindowState { get; set; }

        public void Update(Form window) {
            Top = window.Top;
            Left = window.Left;
            Width = window.Width;
            Height = window.Height;
            WindowState = window.WindowState;
        }

        public void Restore(Form window) {
            if (Width <= 0) {
                return;
            }

            window.Top = Top;
            window.Left = Left;
            window.Width = Width;
            window.Height = Height;
            window.WindowState = WindowState;
        }
    }
}
