using System.Globalization;
using System.Windows.Data;

namespace SRTEditor_MVVM.Infrastructure
{
    /// <summary>
    /// A value converter that inverts a boolean value.
    /// Used in XAML bindings where the logical opposite of a bool property is needed.
    /// For example, binding IsEnabled to the inverse of IsReadOnly.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanConverter : IValueConverter
    {
        /// <summary>
        /// Inverts the given boolean value.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target type must be a boolean.");

            return !(bool)value;
        }

        /// <summary>
        /// Not supported — this converter is one-way only.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}