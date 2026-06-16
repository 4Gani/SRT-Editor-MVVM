using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace SRTEditor_MVVM.Infrastructure
{
    public static class RichTextboxHelper
    {
        public static readonly DependencyProperty BoundDocument =
            DependencyProperty.RegisterAttached("BoundDocument",
                typeof(string), typeof(RichTextboxHelper),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnBoundDocumentChanged));

        public static string? GetBoundDocument(DependencyObject obj)
        {
            return obj.GetValue(BoundDocument) as string;
        }

        public static void SetBoundDocument(DependencyObject obj, string value)
        {
            obj.SetValue(BoundDocument, value);
        }

        /// <summary>
        /// Updates the RichTextBox content when the bound ViewModel property changes
        /// </summary>
        private static void OnBoundDocumentChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            if (obj is not RichTextBox box) return;

            RemoveEventHandler(box);

            // Reset current document content
            box.Document.Blocks.Clear();

            string? contents = GetBoundDocument(obj);
            if (!string.IsNullOrEmpty(contents))
            {
                // Populate the document with the updated text
                box.Document.Blocks.Add(new Paragraph(new Run(contents)));
            }

            AttachEventHandler(box);
        }

        private static void RemoveEventHandler(RichTextBox box)
        {
            Binding binding = BindingOperations.GetBinding(box, BoundDocument);

            if (binding != null)
            {
                if (binding.UpdateSourceTrigger == UpdateSourceTrigger.Default ||
                binding.UpdateSourceTrigger == UpdateSourceTrigger.LostFocus)
                {
                    box.LostFocus -= HandleLostFocus;
                }
                else
                {
                    box.TextChanged -= HandleTextChanged;
                }
            }
        }

        private static void AttachEventHandler(RichTextBox box)
        {
            Binding binding = BindingOperations.GetBinding(box, BoundDocument);
            if (binding != null)
            {
                if (binding.UpdateSourceTrigger == UpdateSourceTrigger.Default ||
                binding.UpdateSourceTrigger == UpdateSourceTrigger.LostFocus)
                {
                    box.LostFocus += HandleLostFocus;
                }
                else
                {
                    box.TextChanged += HandleTextChanged;
                }
            }
        }

        private static void HandleLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not RichTextBox box) return;

            // Preserve the current scroll position before updating the binding
            double currentPosition = box.VerticalOffset;

            string doc = GetContent(box.Document);

            // Refresh the document content to keep the bound value synchronized
            box.Document.Blocks.Clear();

            SetBoundDocument(box, doc);

            // Restore the previous scroll position
            box.ScrollToVerticalOffset(currentPosition);

            // Explicitly update the binding source
            box.GetBindingExpression(BoundDocument)?.UpdateSource();
        }

        private static void HandleTextChanged(object sender, RoutedEventArgs e) { }

        private static string GetContent(FlowDocument document)
        {
            return new TextRange(document.ContentStart, document.ContentEnd).Text;
        }
    }
}