using Microsoft.Extensions.DependencyInjection;
using SRTEditor_MVVM.Components.MessageBox;
using SRTEditor_MVVM.Infrastructure;
using SRTEditor_MVVM.Services;
using SRTEditor_MVVM.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SRTEditor_MVVM.Components.Editor.TimeCorrecting
{
    public class TimeCorrectingViewModel : ValidatableBindableBase
    {
        public TimeCorrectingViewModel(ISrtRepository repo)
        {
            // Initialize with a valid default time value
            TimeText = "00:00:00";

            _repo = repo;
            TimeCorrectingCommand = new RelayCommand(OnTimeCorrecting, CanTimeCorrecting);

            // Subscribe to static property changes to detect SrtFileAddress updates
            StaticPropertyChanged += NotifySrtFileAddressChanged;

            // Subscribe to validation error changes to update command state
            ErrorsChanged += NotifyValidationErrorsChanged;
        }

        #region Fields and Properties
        private readonly ISrtRepository _repo;

        private string _TimeText = string.Empty;

        [Required]
        [RegularExpression(@"\d{2}:[0-5]\d:[0-5]\d", ErrorMessage = "Input value not in correct format")]
        public string TimeText
        {
            get { return _TimeText; }
            set
            {
                SetProperty(ref _TimeText, value);
                AnnotationValidateProperty(nameof(TimeText), value);
            }
        }

        public RelayCommand TimeCorrectingCommand { get; private set; }
        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the TimeCorrectingCommand's executable state when SrtFileAddress changes.
        /// </summary>
        private void NotifySrtFileAddressChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SrtFileAddress")
                TimeCorrectingCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Refreshes the TimeCorrectingCommand's executable state when validation errors change.
        /// </summary>
        private void NotifyValidationErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            TimeCorrectingCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Applies time correction to the SRT file and notifies the file viewer to refresh.
        /// </summary>
        private void OnTimeCorrecting()
        {
            if ((new InputOutputValidation()).IsInputAndOutputInitialized())
            {
                SrtTimeCorrector timeCorrector = ContainerHelper.Container.GetRequiredService<SrtTimeCorrector>();
                timeCorrector.CorrectTime(_repo.GetSrt().SrtFileAddress, _repo.GetSrt().SrtSaveLocation, TimeText);

                MessageBoxHelper.Show("Time correction completed successfully", "Done",
                    MessageButton.OK, MessageIcon.Information);

                // Notify the file viewer to refresh after changes are applied
                EventAggregator.PublishSrtFileChanged();
            }
        }

        /// <summary>
        /// Enables the TimeCorrectingCommand only when an SRT file address is set
        /// and there are no validation errors.
        /// </summary>
        private bool CanTimeCorrecting()
        {
            return !string.IsNullOrEmpty(_repo.GetSrt().SrtFileAddress) && !HasErrors;
        }

        #endregion
    }
}