namespace SRTEditor_MVVM.Services.ToolKit
{
    /// <summary>
    /// This class holds methods for SRT's blank lines
    /// </summary>
    public class Line : ILine
    {
        public Boolean IsThisFreeLine(string line)
        {
            return string.IsNullOrEmpty(line);
        }
    }
}
