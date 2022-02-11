using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Amazon;
using Serilog;

namespace DeepArchiver {
    static class Program {
        public static string AppDataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "co.logu.DeepArchiver");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            InitLogger();
            AWSConfigsS3.EnableUnicodeEncodingForObjectMetadata = true;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static void InitLogger() {
            var logDir = Path.Combine(AppDataPath, "logs");
            Directory.CreateDirectory(logDir);

            var logFile = Path.Combine(logDir, "launch-" + DateTime.Now.ToString("yyyyMMdd-HHmmss.txt") + ".log");
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logFile)
                .CreateLogger();
        }
    }
}
