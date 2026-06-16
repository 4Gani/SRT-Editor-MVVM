namespace SRTEditor_MVVM.Services
{
    public static class EventAggregator
    {
        public static event EventHandler? SrtFileChanged;

        public static void PublishSrtFileChanged()
        {
            SrtFileChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}