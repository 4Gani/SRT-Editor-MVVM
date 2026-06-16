using SRTEditor_MVVM.Infrastructure;
using System.Windows;
using System.Windows.Input;

namespace SRTEditor_MVVM.Components
{
    public class TitleBarViewModel
    {
        public TitleBarViewModel()
        {
            window = Application.Current.MainWindow;

            CloseCommand = new RelayCommand(OnClose);
            MinimizeCommand = new RelayCommand(OnMinimize);
            DragCommand = new RelayCommand(OnDrag);
        }


        /// <summary>
        /// All class's Fields and Properties define here
        /// </summary>
        #region Fields and Properties
        readonly Window window;

        public RelayCommand CloseCommand { get; private set; }
        public RelayCommand MinimizeCommand { get; private set; }
        public RelayCommand DragCommand { get; private set; }
        #endregion


        /// <summary>
        /// All class's Methods define here
        /// </summary>
        #region Methods
        private void OnClose()
        {
            Application.Current.Shutdown();
        }

        private void OnMinimize()
        {
            window.WindowState = WindowState.Minimized;

        }

        private void OnDrag()
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                window.DragMove();
            }
        }
        #endregion
    }
}
