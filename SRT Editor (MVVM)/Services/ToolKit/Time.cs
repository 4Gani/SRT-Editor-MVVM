using System.Text;
using System.Text.RegularExpressions;

namespace SRTEditor_MVVM.Services.ToolKit
{
    /// <summary>
    /// Provides utility methods for parsing, validating and manipulating SRT timestamps
    /// </summary>
    public partial class Time : ITime
    {
        /// <summary>
        /// All class's Fields and Properties define here
        /// </summary>
        #region Fields and Properties
        public enum TimeElements { Hour, Minute, Second, MilliSecond }
        #endregion


        /// <summary>
        /// All class's Methods define here
        /// </summary>
        #region Methods
        public Boolean IsThisTimeLine(string text)
        {
            if (text == null) return false;

            /* Regular Expression Language - Quick Reference:
             * 
             * \s    Matches any white-space character
             * \d    Matches any decimal digit
             * 
             * *     Matches the previous element zero or more times
             * +     Matches the previous element one or more times
             * 
             * :     Matches the character : literally
             * ,     Matches the character , literally
             * -->   Matches the characters --> literally
             */

            Match m = TimeLineRegex().Match(text);
            return m.Success;
        }

        public TimeSpan GetTime(string text)
        {
            /* Regular Expression Language - Quick Reference:
             * 
             * \d    Matches any decimal digit
             * {2}   Matches exactly 2 times
             * 
             * :     Matches the character : literally
             */

            Match m = TimeRegex().Match(text);
            TimeSpan time = TimeSpan.Zero;

            if (m.Success)
            {
                time = TimeSpan.Parse(m.Value);
            }

            return time;
        }

        public TimeSpan GetTimeWithMiliSecond(string text)
        {
            /* Regular Expression Language - Quick Reference:
             * 
             * \d    Matches any decimal digit
             * {2}   Matches exactly 2 times
             * 
             * :     Matches the character : literally
             * \.    Matches the character . literally
             */

            Match m = TimeWithMsRegex().Match(text);
            TimeSpan time = TimeSpan.Zero;

            if (m.Success)
            {
                time = TimeSpan.Parse(m.Value);
            }

            return time;
        }

        public string TimeElementsLenghtCorrection(string time, TimeElements element)
        {
            if (element == TimeElements.MilliSecond)
            {
                if (time == "")
                    return "000";

                // Milliseconds must be in the range 0-999
                if (int.Parse(time) > 999)
                    return "999";

                if (time.Length == 1)
                    return "00" + time;

                if (time.Length == 2)
                    return "0" + time;
            }

            else  // for hour, minute and seconds
            {
                if (time == "")
                    return "00";

                // Hours, minutes and seconds must be in the range 0-59
                if (int.Parse(time) > 59)
                    return "59";

                if (time.Length == 1)
                    return "0" + time;
            }

            // Return the original value if no correction is required
            return time;
        }

        public Match[] TimeLineSpliter(string text)
        {
            /* Time format in SRT file look like 01:12:03,916 --> 02:14:05,749
             * in this function we split this line to two parts (time1 and time2)
             */

            /* Regular Expression Language - Quick Reference:
             * 
             * \d    Matches any decimal digit
             * {2}   Matches exactly 2 times
             * 
             * +     Matches the previous element one or more times
             * *     Matches the previous element zero or more times
             * 
             * :     Matches the character : literally
             * ,     Matches the character , literally
             */

            // Example:   01:12:03,916 --> 02:14:05,749

            // replace ',' character with '.' in text
            text = text.Replace(',', '.');

            Match[] time = new Match[2];
            time[0] = TimeSplitRegex().Match(text);   // result => 01:12:03.916
            time[1] = time[0].NextMatch();   // result => 02:14:05.749

            return time;
        }

        /// <summary>
        /// Normalizes an SRT timeline to the format HH:mm:ss,sss --> HH:mm:ss,sss
        /// </summary>
        public string TimeLineCorrection(string text)
        {
            /* Time format in SRT file must look like 01:12:03,916 --> 02:14:05,749
             * in this function we using TimeLineSpliter function to split this line
             * to two parts (time1 and time2) then correct each part of them with
             * TimeFormatCorrection function
             */

            Match[] time = TimeLineSpliter(text);

            StringBuilder correctedTimeLine = new();

            if (time[0].Success && time[1].Success)
            {
                correctedTimeLine.Append(TimeFormatCorrection(time[0].ToString()))
                    .Append(" --> ")
                    .Append(TimeFormatCorrection(time[1].ToString()));
                return correctedTimeLine.ToString();
            }

            return "";
        }

        /// <summary>
        /// Normalizes a timestamp to the format HH:mm:ss,sss
        /// </summary>
        public string TimeFormatCorrection(string time)
        {

            // Input format may contain incomplete values
            string[] timeParts = TimeSeparatorRegex().Split(time);

            // Build the normalized timestamp
            StringBuilder correctedTime = new();

            correctedTime.Append(TimeElementsLenghtCorrection(timeParts[0], TimeElements.Hour))
                .Append(':');
            correctedTime.Append(TimeElementsLenghtCorrection(timeParts[1], TimeElements.Minute))
                .Append(':');
            correctedTime.Append(TimeElementsLenghtCorrection(timeParts[2], TimeElements.Second))
                .Append(',');
            correctedTime.Append(TimeElementsLenghtCorrection(timeParts[3], TimeElements.MilliSecond));

            return correctedTime.ToString();
        }

        private string ShiftTime(string text, TimeSpan offset, bool add)
        {
            Match[] time = TimeLineSpliter(text);

            // Convert timestamps to TimeSpan values
            TimeSpan t1 = TimeSpan.Parse(time[0].Value);
            TimeSpan t2 = TimeSpan.Parse(time[1].Value);

            // Apply the specified offset
            t1 = add ? t1.Add(offset) : t1.Subtract(offset);
            t2 = add ? t2.Add(offset) : t2.Subtract(offset);

            return new StringBuilder()
                .Append(t1.ToString(@"hh\:mm\:ss\,fff"))
                .Append(" --> ")
                .Append(t2.ToString(@"hh\:mm\:ss\,fff"))
                .ToString();
        }

        public string AddTime(string text, TimeSpan offset) => ShiftTime(text, offset, true);

        public string SubtractTime(string text, TimeSpan offset) => ShiftTime(text, offset, false);

        [GeneratedRegex(@"\s*\d*:\d*:\d*,\d*\s*-->\s*\d*:\d*:\d*,\d*\s*")]
        private static partial Regex TimeLineRegex();

        [GeneratedRegex(@"\d{2}:\d{2}:\d{2}")]
        private static partial Regex TimeRegex();

        [GeneratedRegex(@"\d{2}:\d{2}:\d{2}\.\d{3}")]
        private static partial Regex TimeWithMsRegex();

        [GeneratedRegex(@"\d*:\d*:\d*\.\d*")]
        private static partial Regex TimeSplitRegex();

        [GeneratedRegex(@":|,|\.")]
        private static partial Regex TimeSeparatorRegex();

        #endregion
    }
}
