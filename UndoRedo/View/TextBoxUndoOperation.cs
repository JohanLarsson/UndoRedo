namespace UndoRedo.View
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public class TextBoxUndoOperation : IUndoOperation
    {
        public TextBoxUndoOperation(TextBoxBase sender, TextChangedEventArgs args)
        {
            Sender = sender;
            Args = args;
            Timestamp = DateTime.UtcNow;
        }
        public TextBoxBase Sender { get; private set; }
        public TextChangedEventArgs Args { get; private set; }
        public DateTime Timestamp { get; private set; }
        public void Undo()
        {
            Sender.Undo();
            Sender.Focus();
        }
        public void Redo()
        {
            Sender.Redo();
            Sender.Focus();
        }
    }
}
