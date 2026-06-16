using SRTEditor_MVVM.Infrastructure;
using SRTEditor_MVVM.Services;
using SRTEditor_MVVM.Services.ToolKit;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace SRTEditor_MVVM.Components.Viewer
{
    public class FileViewerViewModel : BindableBase
    {
        public FileViewerViewModel(ISrtRepository repo, IReadWrite readWrite)
        {
            _repo = repo;
            ReadWriteClass = readWrite;

            FileName = _repo.GetSrt().SrtFileName;
            IsContentChanged = false;
            IsMessageVisible = Visibility.Hidden;

            // Initialize the timer used to auto-hide the save confirmation message
            timer.Tick += TimerTick;
            timer.Interval = TimeSpan.FromSeconds(3);

            SelectionChangedCommand = new RelayCommand(OnSelectionChanged);
            TextChangedCommand = new RelayCommand(OnTextChanged);
            SaveCommand = new RelayCommand(OnSave, CanSave);

            // Subscribe to file change events to refresh the viewer when other services modify the SRT file
            EventAggregator.SrtFileChanged += OnSrtFileChanged;
        }

        #region Fields and Properties
        private readonly ISrtRepository _repo;
        private readonly IReadWrite ReadWriteClass;

        private string _FileName = string.Empty;
        public string FileName
        {
            get { return _FileName; }
            set { SetProperty(ref _FileName, value, nameof(FileName)); }
        }

        private string _FileContents = string.Empty;
        public string FileContents
        {
            get { return _FileContents; }
            set { SetProperty(ref _FileContents, value, nameof(FileContents)); }
        }

        private bool IsContentChanged { get; set; }

        private Visibility _IsMessageVisible;
        public Visibility IsMessageVisible
        {
            get { return _IsMessageVisible; }
            set { SetProperty(ref _IsMessageVisible, value, nameof(IsMessageVisible)); }
        }

        readonly DispatcherTimer timer = new();

        public RelayCommand SelectionChangedCommand { get; private set; }
        public RelayCommand TextChangedCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        #endregion

        #region Methods

        /// <summary>
        /// Reads the current SRT file from disk and loads its contents into the viewer.
        /// </summary>
        private void LoadFile()
        {
            using StreamReader file = ReadWriteClass.ReadFile(_repo.GetSrt().SrtFileAddress);
            FileContents = file.ReadToEnd();
        }

        /// <summary>
        /// Refreshes the file name and reloads the file contents when the tab is selected.
        /// </summary>
        private void OnSelectionChanged()
        {
            FileName = _repo.GetSrt().SrtFileName;
            LoadFile();
        }

        /// <summary>
        /// Marks the content as changed and enables the Save button.
        /// </summary>
        private void OnTextChanged()
        {
            IsContentChanged = true;
            SaveCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Reloads the file contents when an external service modifies the SRT file.
        /// </summary>
        private void OnSrtFileChanged(object? sender, EventArgs e)
        {
            LoadFile();
        }

        /// <summary>
        /// Saves the current viewer contents back to the original SRT file.
        /// Writes to a temporary file first, then replaces the original.
        /// </summary>
        private void OnSave()
        {
            string tempPath = Path.Combine(AppSettings.Directory, "temp_SubView.srt");

            // Write contents to a temporary file
            using (TextWriter file = new StreamWriter(tempPath, false, Encoding.UTF8))
                file.Write(FileContents);

            // Replace the original file with the temporary file
            File.Copy(tempPath, _repo.GetSrt().SrtFileAddress, true);
            File.Delete(tempPath);

            // Show the save confirmation message for 3 seconds
            IsMessageVisible = Visibility.Visible;
            timer.Start();

            // Reset unsaved changes state
            IsContentChanged = false;
            SaveCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Hides the save confirmation message after the timer elapses.
        /// </summary>
        private void TimerTick(object? sender, EventArgs e)
        {
            IsMessageVisible = Visibility.Hidden;
            timer.Stop();
        }

        /// <summary>
        /// Enables the SaveCommand only when there are unsaved changes.
        /// </summary>
        private bool CanSave()
        {
            return IsContentChanged;
        }

        #endregion
    }
}