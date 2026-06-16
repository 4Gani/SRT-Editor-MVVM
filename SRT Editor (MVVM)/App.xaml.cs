using System.IO;
using System.Windows;
using SRTEditor_MVVM.Services;

namespace SRTEditor_MVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Directory.CreateDirectory(AppSettings.Directory);
        }
    }
}