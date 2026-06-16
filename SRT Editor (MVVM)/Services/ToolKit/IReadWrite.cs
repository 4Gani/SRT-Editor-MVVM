using System.IO;

namespace SRTEditor_MVVM.Services.ToolKit
{
    public interface IReadWrite
    {
        StreamReader ReadFile(string fileAddress);
        void ReplaceFile(string fileAddress);
        TextWriter WriteFile(string saveLocation);
    }
}