using Microsoft.Extensions.DependencyInjection;
using SRTEditor_MVVM.Components.MessageBox;
using SRTEditor_MVVM.Infrastructure;
using SRTEditor_MVVM.Services;
using SRTEditor_MVVM.Validation;
using System.ComponentModel;

namespace SRTEditor_MVVM.Components.Editor.Repairing
{
    public class RepairingViewModel : BindableBase
    {
        public RepairingViewModel(ISrtRepository repo)
        {
            _repo = repo;
            RepairCommand = new RelayCommand(OnRepair, CanRepair);

            // Subscribe to static property changes to detect SrtFileAddress updates
            StaticPropertyChanged += NotifySrtFileAddressChanged;
        }

        #region Fields and Properties
        private readonly ISrtRepository _repo;
        public RelayCommand RepairCommand { get; private set; }
        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the RepairCommand's executable state when SrtFileAddress changes.
        /// </summary>
        private void NotifySrtFileAddressChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SrtFileAddress")
                RepairCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Repairs the loaded SRT file and notifies the file viewer to refresh.
        /// </summary>
        private void OnRepair()
        {
            if ((new InputOutputValidation()).IsInputAndOutputInitialized())
            {
                SrtRepairer repairer = ContainerHelper.Container.GetRequiredService<SrtRepairer>();
                repairer.RepairFile(_repo.GetSrt().SrtFileAddress, _repo.GetSrt().SrtSaveLocation);

                MessageBoxHelper.Show("SRT file repaired successfully", "Done",
                    MessageButton.OK, MessageIcon.Information);

                // Notify the file viewer to refresh after changes are applied
                EventAggregator.PublishSrtFileChanged();
            }
        }

        /// <summary>
        /// Enables the RepairCommand only when an SRT file address has been set.
        /// </summary>
        private bool CanRepair()
        {
            return !string.IsNullOrEmpty(_repo.GetSrt().SrtFileAddress);
        }

        #endregion
    }
}