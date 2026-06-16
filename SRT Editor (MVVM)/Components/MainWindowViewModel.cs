using Microsoft.Extensions.DependencyInjection;
using SRTEditor_MVVM.Components.Editor.Container;
using SRTEditor_MVVM.Components.Viewer;
using SRTEditor_MVVM.Infrastructure;
using SRTEditor_MVVM.Services;
using System.ComponentModel;
using System.IO;

namespace SRTEditor_MVVM.Components
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            IsSrtFileSet = false;

            _repo = ContainerHelper.Container.GetRequiredService<ISrtRepository>();

            TitleBarViewModel = new TitleBarViewModel();
            ContainerViewModel = new ContainerViewModel();
            FileViewerViewModel = ContainerHelper.Container.GetRequiredService<FileViewerViewModel>();

            // Subscribe to static property changes to detect SrtFileAddress updates
            StaticPropertyChanged += NotifySrtFileAddressChanged;
        }

        #region Fields and Properties
        private readonly ISrtRepository _repo;

        public TitleBarViewModel TitleBarViewModel { get; set; }
        public ContainerViewModel ContainerViewModel { get; set; }
        public FileViewerViewModel FileViewerViewModel { get; set; }

        private bool _IsSrtFileSet;
        public bool IsSrtFileSet
        {
            get { return _IsSrtFileSet; }
            set { SetProperty(ref _IsSrtFileSet, value, nameof(IsSrtFileSet)); }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Updates IsSrtFileSet when the SrtFileAddress property changes,
        /// which controls the visibility of file-dependent UI elements.
        /// </summary>
        private void NotifySrtFileAddressChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SrtFileAddress")
                IsSrtFileSet = !string.IsNullOrEmpty(_repo.GetSrt().SrtFileAddress);
        }

        #endregion
    }
}