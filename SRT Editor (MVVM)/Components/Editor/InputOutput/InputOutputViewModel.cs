using Microsoft.Win32;
using SRTEditor_MVVM.Components.MessageBox;
using SRTEditor_MVVM.Infrastructure;
using SRTEditor_MVVM.Services;
using System.ComponentModel;
using System.IO;
using System.Windows;
using static SRTEditor_MVVM.Services.ToolKit.ToolKitEnums;

namespace SRTEditor_MVVM.Components.Editor.InputOutput
{
    public class InputOutputViewModel : ValidatableBindableBase
    {
        public InputOutputViewModel(ISrtRepository repo)
        {
            _repo = repo;
            IsOverWritable = AppSettings.IsOverWritable;
            _SrtFileName = _repo.GetSrt().SrtFileName;
            _SrtFileAddress = _repo.GetSrt().SrtFileAddress;
            _SrtSaveLocation = _repo.GetSrt().SrtSaveLocation;

            InputBrowserCommand = new RelayCommand(OnInputBrowser);
            OutputBrowserCommand = new RelayCommand(OnOutputBrowser);

            // Subscribe to property changes to react to IsOverWritable toggling
            PropertyChanged += NotifyIsOverWritableChanged;
        }

        #region Fields and Properties
        private readonly ISrtRepository _repo;

        private bool _IsOverWritable;
        public bool IsOverWritable
        {
            get { return _IsOverWritable; }
            set
            {
                SetProperty(ref _IsOverWritable, value, nameof(IsOverWritable));
                AppSettings.IsOverWritable = value;
            }
        }

        private string _SrtFileName = string.Empty;
        public string SrtFileName
        {
            get { return _SrtFileName; }
            set
            {
                _repo.UpdateSrtProperty(PropertyNames.SrtFileName, value);
                SetProperty(ref _SrtFileName, value, nameof(SrtFileName));
            }
        }

        private string _SrtFileAddress;
        public string SrtFileAddress
        {
            get { return _SrtFileAddress; }
            set
            {
                // Validate the address before updating the repository
                if (ValidateProperty("FullAddressValidation", nameof(SrtFileAddress), value))
                    _repo.UpdateSrtProperty(PropertyNames.SrtFileAddress, value);
                else
                    _repo.UpdateSrtProperty(PropertyNames.SrtFileAddress, "");

                SetProperty(ref _SrtFileAddress, value, nameof(SrtFileAddress));
            }
        }

        private string _SrtSaveLocation;
        public string SrtSaveLocation
        {
            get { return _SrtSaveLocation; }
            set
            {
                // Validate the save location before updating the repository
                if (ValidateProperty("AddressValidation", nameof(SrtSaveLocation), value))
                    _repo.UpdateSrtProperty(PropertyNames.SrtSaveLocation, value);
                else
                    _repo.UpdateSrtProperty(PropertyNames.SrtSaveLocation, "");

                SetProperty(ref _SrtSaveLocation, value, nameof(SrtSaveLocation));
            }
        }

        public RelayCommand InputBrowserCommand { get; private set; }
        public RelayCommand OutputBrowserCommand { get; private set; }
        #endregion

        #region Methods

        /// <summary>
        /// When overwrite mode is disabled, restores the save location to its last valid value,
        /// or clears it if it was pointing to the same path as the input file.
        /// </summary>
        private void NotifyIsOverWritableChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsOverWritable) && !IsOverWritable)
            {
                SrtSaveLocation = _repo.GetSrt().SrtSaveLocation == _repo.GetSrt().SrtFileAddress
                    ? ""
                    : _repo.GetSrt().SrtSaveLocation;
            }
        }

        /// <summary>
        /// Opens a file browser dialog and sets the selected SRT file as the input.
        /// </summary>
        private void OnInputBrowser()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Open an SRT File",
                Filter = "SRT Files|*.srt",
                FilterIndex = 1
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SrtFileAddress = openFileDialog.FileName;
                SrtFileName = openFileDialog.SafeFileName;
            }
        }

        /// <summary>
        /// Opens a save dialog and sets the selected path as the output save location.
        /// </summary>
        private void OnOutputBrowser()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save the SRT File",
                Filter = "SRT Files|*.srt",
                FilterIndex = 1
            };

            if (!string.IsNullOrEmpty(SrtFileAddress))
            {
                // Pre-fill the dialog with a suggested file name based on the input file
                saveFileDialog.FileName = Path.GetFileNameWithoutExtension(SrtFileAddress) + " (new)";
                saveFileDialog.InitialDirectory = SrtFileAddress;
            }

            if (saveFileDialog.ShowDialog() == true)
                SrtSaveLocation = saveFileDialog.FileName;
        }

        /// <summary>
        /// Handles drag-over events by allowing file drop operations.
        /// </summary>
        public void DragOver(DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        }

        /// <summary>
        /// Handles file drop events and sets the dropped SRT file as the input.
        /// </summary>
        public void Drop(DragEventArgs e)
        {
            string[]? address = (string[]?)e.Data.GetData(DataFormats.FileDrop, true);

            if (address == null || address.Length != 1)
            {
                MessageBoxHelper.Show(
                    address == null
                        ? "Only SRT files can be dropped here"
                        : "Please drop only one file at a time",
                    "Warning", MessageButton.OK, MessageIcon.Warning);
                return;
            }

            if (Path.GetExtension(address[0]).Equals(".srt", StringComparison.InvariantCultureIgnoreCase))
                SrtFileAddress = address[0];
            else
                MessageBoxHelper.Show("Only SRT files are supported", "Warning",
                    MessageButton.OK, MessageIcon.Warning);
        }

        #endregion
    }
}