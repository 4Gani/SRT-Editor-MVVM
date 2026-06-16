using System.Windows;

namespace SRTEditor_MVVM.Components.MessageBox
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml.
    /// This code-behind is intentionally minimal — all logic lives in the ViewModel.
    /// The only responsibility here is wiring up the ViewModel and handling
    /// button clicks to close the dialog.
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        /// <summary>
        /// Initializes the window with the provided ViewModel as its DataContext.
        /// Dynamically creates buttons from the ViewModel's Buttons collection.
        /// </summary>
        public CustomMessageBox(MessageBoxViewModel viewModel)
        {
            InitializeComponent();

            // Assign the ViewModel so XAML bindings can resolve
            DataContext = viewModel;

            // Dynamically build buttons from ViewModel data and add them to the panel
            foreach (var buttonDef in viewModel.Buttons)
            {
                var button = new System.Windows.Controls.Button
                {
                    Content = buttonDef.Label,
                    Margin = new Thickness(10),
                    // Apply the shared button style from the resource dictionary
                    Style = (System.Windows.Style)FindResource("normalButton"),
                };

                // Capture the result value for use inside the lambda
                bool result = buttonDef.Result;

                // Set the DialogResult and close the window when button is clicked
                button.Click += (_, _) => { DialogResult = result; };

                // Add the button to the panel defined in XAML
                MessageBoxPanel.Children.Add(button);
            }
        }
    }
}