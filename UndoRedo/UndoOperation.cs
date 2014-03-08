namespace UndoRedo
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public class UndoOperation
    {
        public UndoOperation(TextBoxBase sender, UndoAction action)
        {
            Sender = sender;
            Action = action;
            Timestamp = DateTime.UtcNow;
        }
        public TextBoxBase Sender { get; private set; }
        public UndoAction Action { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}
