using System.Text.RegularExpressions;

namespace SRTEditor_MVVM.Services.ToolKit
{
    /// <summary>
    /// This class holds methods for SRT's number lines
    /// </summary>
    public partial class Number : INumber
    {
        /// <summary>
        /// All class's Methods define here
        /// </summary>
        #region Methods
        public Boolean IsThisLineNumber(string line)
        {
            if (line == null) return false;

            /* Regular Expression Language - Quick Reference:
             * 
             * [ ]   Character group, Matches any single character in character group
             * ( )   For grouping matches
             * 
             * \s    Matches any white-space character
             * \t    Matches a tab
             * \d    Matches any decimal digit
             * 
             * *     Matches the previous element zero or more times
             * +     Matches the previous element one or more times
             * 
             * ^     The match must start at the beginning of the string
             * $     The match must occur at the end of the string or before \n at the end of the string
             * ^()$  Used for match all line at once
             */

            Match m = LineNumberRegex().Match(line);
            return m.Success;
        }

        [GeneratedRegex(@"^(([\s\t]*[\d]+[\s\t]*)+)$")]
        private static partial Regex LineNumberRegex();
        #endregion
    }
}
