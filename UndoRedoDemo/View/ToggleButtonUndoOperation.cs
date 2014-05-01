namespace UndoRedo.View
{
    using System;
    using System.Windows.Controls.Primitives;

    public class ToggleButtonUndoOperation : IUndoOperation
    {
        public ToggleButtonUndoOperation(ToggleButton sender, bool? oldValue, bool? newValue)
        {
            Sender = sender;
            OldValue = oldValue;
            NewValue = newValue;
            Timestamp = DateTime.UtcNow;
        }
        public ToggleButton Sender { get; private set; }
        public bool? OldValue { get; set; }
        public bool? NewValue { get; set; }
        public DateTime Timestamp { get; private set; }
        public void Undo()
        {
            Sender.IsChecked = OldValue;
            Sender.Focus();
        }
        public void Redo()
        {
            throw new NotImplementedException();
        }
    }
}