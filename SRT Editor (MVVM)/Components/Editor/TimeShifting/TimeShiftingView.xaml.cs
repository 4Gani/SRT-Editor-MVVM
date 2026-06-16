using SRTEditor_MVVM.Validation;
using System.Windows.Controls;
using System.Windows.Input;

namespace SRTEditor_MVVM.Components.Editor.TimeShifting
{
    /// <summary>
    /// Interaction logic for TimeShiftingView.xaml
    /// </summary>
    public partial class TimeShiftingView : UserControl
    {
        readonly ValidationCheck checkValidation;

        public TimeShiftingView()
        {
            InitializeComponent();
            checkValidation = new ValidationCheck();
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            ValidationCheck.NumericOnly(e);
        }

        private void TimeOnly(object sender, TextCompositionEventArgs e)
        {
            ValidationCheck.TimeOnly(e);
        }
    }
}
