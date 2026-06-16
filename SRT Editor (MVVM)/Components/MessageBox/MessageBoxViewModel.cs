using System.Windows.Media.Imaging;

namespace SRTEditor_MVVM.Components.MessageBox
{
    /// <summary>
    /// ViewModel for the CustomMessageBox window.
    /// Prepares all data needed by the View, including title, message,
    /// icon source, and the list of buttons to display.
    /// </summary>
    public class MessageBoxViewModel
    {
        /// <summary>
        /// Initializes the ViewModel and builds all UI data from the provided parameters.
        /// </summary>
        public MessageBoxViewModel(string messageText, string messageTitle,
            MessageButton messageButton, MessageIcon messageIcon)
        {
            // Set title and message
            MessageTitle = messageTitle;
            MessageText = messageText;

            // Resolve the icon image source based on the requested icon type
            MessageImageSource = ResolveIcon(messageIcon);

            // Build the list of buttons based on the requested button type
            Buttons = BuildButtons(messageButton);
        }

        #region Properties

        /// <summary>Gets the title text displayed in the message box header.</summary>
        public string MessageTitle { get; }

        /// <summary>Gets the main message text displayed in the message box body.</summary>
        public string MessageText { get; }

        /// <summary>Gets the image source for the message box icon.</summary>
        public BitmapImage MessageImageSource { get; }

        /// <summary>
        /// Gets the list of button definitions to be rendered in the button panel.
        /// Each entry contains the button label and its DialogResult value.
        /// </summary>
        public List<ButtonDefinition> Buttons { get; }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Resolves the icon image source URI based on the requested MessageIcon type.
        /// </summary>
        private static BitmapImage ResolveIcon(MessageIcon messageIcon)
        {
            // Use pack URI to ensure icons load correctly regardless of the working directory
            string path = messageIcon switch
            {
                MessageIcon.Error => "pack://application:,,,/Assets/Error.png",
                MessageIcon.Warning => "pack://application:,,,/Assets/Warning.png",
                MessageIcon.Question => "pack://application:,,,/Assets/Question.png",
                _ => "pack://application:,,,/Assets/Information.png",
            };

            return new BitmapImage(new Uri(path, UriKind.Absolute));
        }

        /// <summary>
        /// Builds a list of ButtonDefinition objects based on the requested MessageButton type.
        /// Each ButtonDefinition describes a button label and the DialogResult it produces.
        /// </summary>
        private static List<ButtonDefinition> BuildButtons(MessageButton messageButton)
        {
            return messageButton switch
            {
                MessageButton.OKCancel =>
                [
                    new("OK", true),
                    new("Cancel", false),
                ],
                MessageButton.YesNo =>
                [
                    new("Yes", true),
                    new("No", false),
                ],
                MessageButton.YesNoCancel =>
                [
                    new("Yes", true),
                    new("No", false),
                    new("Cancel", false),
                ],
                // Default: OK only
                _ =>
                [
                    new("OK", true),
                ],
            };
        }

        #endregion
    }

    /// <summary>
    /// Represents a single button definition for the message box.
    /// Contains the button label and the DialogResult value it produces when clicked.
    /// </summary>
    public class ButtonDefinition
    {
        /// <summary>Gets the text label displayed on the button.</summary>
        public string Label { get; }

        /// <summary>Gets the DialogResult value returned when this button is clicked.</summary>
        public bool Result { get; }

        public ButtonDefinition(string label, bool result)
        {
            Label = label;
            Result = result;
        }
    }
}