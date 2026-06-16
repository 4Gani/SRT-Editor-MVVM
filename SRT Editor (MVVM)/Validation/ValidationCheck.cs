using System.Text.RegularExpressions;
using System.Windows.Input;

namespace SRTEditor_MVVM.Validation
{
    /// <summary>
    /// Provides input filtering helpers for TextBox controls.
    /// Called from View code-behind to restrict keyboard input to specific character sets.
    /// </summary>
    public partial class ValidationCheck
    {
        /// <summary>
        /// Blocks any character that is not a digit (0–9).
        /// Used for line number input fields.
        /// </summary>
        public static void NumericOnly(TextCompositionEventArgs e)
        {
            e.Handled = !NumericRegex().IsMatch(e.Text);
        }

        /// <summary>
        /// Blocks any character that is not valid in a time string (digits, colon, dot).
        /// Used for time input fields.
        /// </summary>
        public static void TimeOnly(TextCompositionEventArgs e)
        {
            e.Handled = !TimeCharacterRegex().IsMatch(e.Text);
        }

        /// <summary>
        /// Returns true if the string matches the expected time format HH:MM:SS.mmm.
        /// </summary>
        public static bool FormatCheck(string str)
        {
            return TimeFormatRegex().IsMatch(str);
        }

        [GeneratedRegex(@"[0-9]")]
        private static partial Regex NumericRegex();

        [GeneratedRegex(@"[0-9:.]")]
        private static partial Regex TimeCharacterRegex();

        [GeneratedRegex(@"\d{2}:[0-5]\d:[0-5]\d\.\d{3}")]
        private static partial Regex TimeFormatRegex();
    }
}