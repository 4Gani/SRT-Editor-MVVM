using System.Text.RegularExpressions;

namespace SRTEditor_MVVM.Services.ToolKit
{
    public interface ITime
    {
        string AddTime(string text, TimeSpan offset);
        TimeSpan GetTime(string text);
        TimeSpan GetTimeWithMiliSecond(string text);
        bool IsThisTimeLine(string text);
        string SubtractTime(string text, TimeSpan offset);
        string TimeElementsLenghtCorrection(string time, Time.TimeElements element);
        string TimeFormatCorrection(string time);
        string TimeLineCorrection(string text);
        Match[] TimeLineSpliter(string text);
    }
}