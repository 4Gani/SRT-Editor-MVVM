using Microsoft.Extensions.DependencyInjection;
using SRTEditor_MVVM.Components.MessageBox;
using SRTEditor_MVVM.Infrastructure;
using SRTEditor_MVVM.Services;
using SRTEditor_MVVM.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SRTEditor_MVVM.Components.Editor.TimeShifting
{
    public class TimeShiftingViewModel : ValidatableBindableBase
    {
        public TimeShiftingViewModel(ISrtRepository repo)
        {
            // Initialize with a valid default time value
            TimeText = "00:00:00.000";

            _repo = repo;
            IsRaised = AppSettings.IsRaised;
            IsApplyToAllLines = AppSettings.IsApplyToAllLines;

            ChangeCommand = new RelayCommand(OnChange, CanChange);
            ClearCommand = new RelayCommand(OnClear);

            // Subscribe to static property changes to detect SrtFileAddress updates
            StaticPropertyChanged += NotifySrtFileAddressChanged;

            // Subscribe to validation error changes to update command state
            ErrorsChanged += NotifyValidationErrorsChanged;
        }

        #region Fields and Properties
        private readonly ISrtRepository _repo;

        private string _TimeText = string.Empty;

        [Required]
        [RegularExpression(@"\d{2}:[0-5]\d:[0-5]\d\.\d{3}", ErrorMessage = "Input value not in correct format")]
        public string TimeText
        {
            get { return _TimeText; }
            set
            {
                SetProperty(ref _TimeText, value);
                AnnotationValidateProperty(nameof(TimeText), value);
            }
        }

        private string _FromLineNumber = string.Empty;

        [RegularExpression(@"\d*", ErrorMessage = "Input value is invalid")]
        public string FromLineNumber
        {
            get { return _FromLineNumber; }
            set
            {
                SetProperty(ref _FromLineNumber, value);
                AnnotationValidateProperty(nameof(FromLineNumber), value);
            }
        }

        private string _ToLineNumber = string.Empty;

        [RegularExpression(@"\d*", ErrorMessage = "Input value is invalid")]
        public string ToLineNumber
        {
            get { return _ToLineNumber; }
            set
            {
                SetProperty(ref _ToLineNumber, value);
                AnnotationValidateProperty(nameof(ToLineNumber), value);
            }
        }

        private bool _IsRaised;
        public bool IsRaised
        {
            get { return _IsRaised; }
            set
            {
                SetProperty(ref _IsRaised, value);
                AppSettings.IsRaised = value;
            }
        }

        private bool _IsApplyToAllLines;
        public bool IsApplyToAllLines
        {
            get { return _IsApplyToAllLines; }
            set
            {
                SetProperty(ref _IsApplyToAllLines, value);
                AppSettings.IsApplyToAllLines = value;
            }
        }

        public RelayCommand ChangeCommand { get; private set; }
        public RelayCommand ClearCommand { get; private set; }
        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the ChangeCommand's executable state when SrtFileAddress changes.
        /// </summary>
        private void NotifySrtFileAddressChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SrtFileAddress")
                ChangeCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Refreshes the ChangeCommand's executable state when validation errors change.
        /// </summary>
        private void NotifyValidationErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            ChangeCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Shifts SRT timestamps by the specified offset and notifies the file viewer to refresh.
        /// </summary>
        private void OnChange()
        {
            if ((new InputOutputValidation()).IsInputAndOutputInitialized())
            {
                SrtTimeShifter timeShifter = ContainerHelper.Container.GetRequiredService<SrtTimeShifter>();
                ShiftResult result = timeShifter.ShiftTime(
                    _repo.GetSrt().SrtFileAddress,
                    _repo.GetSrt().SrtSaveLocation,
                    TimeText, FromLineNumber, ToLineNumber);

                switch (result)
                {
                    case ShiftResult.Success:
                        MessageBoxHelper.Show("SRT timestamps shifted successfully", "Done",
                            MessageButton.OK, MessageIcon.Information);
                        EventAggregator.PublishSrtFileChanged();
                        break;

                    case ShiftResult.InvalidRange:
                        MessageBoxHelper.Show("Please enter valid From and To line numbers", "Warning",
                            MessageButton.OK, MessageIcon.Warning);
                        break;

                    case ShiftResult.InvalidTime:
                        MessageBoxHelper.Show("Please enter a valid time offset", "Warning",
                            MessageButton.OK, MessageIcon.Warning);
                        break;
                }
            }
        }

        /// <summary>
        /// Enables the ChangeCommand only when an SRT file address is set
        /// and there are no validation errors.
        /// </summary>
        private bool CanChange()
        {
            return !string.IsNullOrEmpty(_repo.GetSrt().SrtFileAddress) && !HasErrors;
        }

        /// <summary>
        /// Resets all input fields to their default values.
        /// </summary>
        private void OnClear()
        {
            FromLineNumber = String.Empty;
            ToLineNumber = String.Empty;
            TimeText = "00:00:00.000";
        }

        #endregion
    }
}