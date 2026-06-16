using System.IO;

namespace SRTEditor_MVVM.Services
{
    public static class AppSettings
    {
        static AppSettings()
        {
            // Default runtime configuration values
            IsOverWritable = true;
            IsRaised = true;
            IsApplyToAllLines = false;
        }

        // Application data directory (AppData)
        private static readonly string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SRT Editor");

        public static string Directory => directory;  // Encapsulated field
        public static bool IsOverWritable { get; set; }
        public static bool IsRaised { get; set; }
        public static bool IsApplyToAllLines { get; set; }
    }
}
