using SRTEditor_MVVM.Model;

namespace SRTEditor_MVVM.Services
{
    public static class SrtDataBase
    {
        static SrtDataBase()
        {
            // Default value
            CurrentFile = new Srt();
        }

        public static Srt CurrentFile { get; set; }
    }
}
