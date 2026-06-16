using SRTEditor_MVVM.Validation;
using System.Windows.Controls;
using System.Windows.Input;

namespace SRTEditor_MVVM.Components.Editor.TimeCorrecting
{
    /// <summary>
    /// Interaction logic for TimeCorrectingView.xaml
    /// </summary>
    public partial class TimeCorrectingView : UserControl
    {
        readonly ValidationCheck checkValidation;

        public TimeCorrectingView()
        {
            InitializeComponent();
            checkValidation = new ValidationCheck();
        }

        private void TimeOnly(object sender, TextCompositionEventArgs e)
        {
            ValidationCheck.TimeOnly(e);
        }
    }
}
