using System.Media;

namespace SRTEditor_MVVM.Components.MessageBox
{
    /// <summary>
    /// Provides a static helper for displaying message boxes throughout the application.
    /// Kept static intentionally — similar to WPF's own MessageBox.Show pattern —
    /// since message boxes are stateless UI helpers that do not benefit from injection.
    /// </summary>
    public static class MessageBoxHelper
    {
        /// <summary>
        /// Creates and displays a CustomMessageBox dialog with the specified parameters.
        /// </summary>
        /// <param name="messageText">The main message to display.</param>
        /// <param name="messageTitle">The title of the message box window.</param>
        /// <param name="messageButton">The button combination to display.</param>
        /// <param name="messageIcon">The icon to display.</param>
        public static void Show(string messageText, string messageTitle,
            MessageButton messageButton, MessageIcon messageIcon)
        {
            // Build the ViewModel with all required display data
            var viewModel = new MessageBoxViewModel(
                messageText, messageTitle, messageButton, messageIcon);

            // Create the View and assign the ViewModel as its DataContext
            var messageBox = new CustomMessageBox(viewModel);

            // Play a system sound and show the dialog
            SystemSounds.Asterisk.Play();
            messageBox.ShowDialog();
        }
    }
}