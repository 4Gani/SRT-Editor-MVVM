using System.Windows;
using System.Windows.Controls;

namespace SRTEditor_MVVM.Components.Editor.InputOutput
{
    /// <summary>
    /// Interaction logic for InputOutputView.xaml
    /// </summary>
    public partial class InputOutputView : UserControl
    {
        public InputOutputView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
            var vm = DataContext as InputOutputViewModel;
            vm?.DragOver(e);
        }

        private void TextBox_PreviewDrop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            var vm = DataContext as InputOutputViewModel;
            vm?.Drop(e);
        }
    }
}
