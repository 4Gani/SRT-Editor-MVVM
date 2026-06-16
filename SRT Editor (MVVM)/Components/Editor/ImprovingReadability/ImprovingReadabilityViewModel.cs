using SRTEditor_MVVM.Components.MessageBox;
using SRTEditor_MVVM.Infrastructure;
using SRTEditor_MVVM.Services;

namespace SRTEditor_MVVM.Components.Editor.ImprovingReadability
{
    public class ImprovingReadabilityViewModel : BindableBase
    {
        public ImprovingReadabilityViewModel(ISrtRepository repo)
        {
            _repo = repo;
            ImproveReadabilityCommand = new RelayCommand(OnImproveReadability, CanImproveReadability);
        }


        /// <summary>
        /// All class's Fields and Properties define here
        /// </summary>
        #region Fields and Properties
        private readonly ISrtRepository _repo;
        public RelayCommand ImproveReadabilityCommand { get; private set; }
        #endregion


        /// <summary>
        /// All class's Methods define here
        /// </summary>
        #region Methods
        private void OnImproveReadability()
        {
            MessageBoxHelper.Show("This section has been unavailable since Version 1.30", "Info",
                MessageButton.OK, MessageIcon.Information);
        }

        private bool CanImproveReadability()
        {
            return false;
        }
        #endregion

    }
}
