namespace UndoRedo
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class HistoryPoint
    {
        public HistoryPoint(Control control, object value, DependencyProperty property, UpdateReason updateReason)
        {
            Timestamp = DateTime.UtcNow;
            Control = control;
            Value = value;
            Property = property;
            UpdateReason = updateReason;
        }
        public DateTime Timestamp { get; private set; }
        public Control Control { get; private set; }
        public object Value { get; private set; }
        public object CurrentValue
        {
            get
            {
                return Control.GetValue(Property);
            }
        }
        public DependencyProperty Property { get; private set; }
        public UpdateReason UpdateReason { get; private set; }
        public virtual void Undo()
        {
            Control.SetCurrentValue(Property, Value);
            //Control.Focus();
        }
        public virtual void Redo()
        {
            Control.SetCurrentValue(Property, Value);
            //Control.Focus();
        }
        public virtual HistoryPoint ToUndoPoint()
        {
            return new HistoryPoint(Control, CurrentValue, Property, UpdateReason.Undo);
        }
        public virtual HistoryPoint ToRedoPoint()
        {
            return new HistoryPoint(Control, CurrentValue, Property, UpdateReason.Undo);
        }
        public override string ToString()
        {
            return string.Format("Timestamp: {0}, Control: {1}", Timestamp, Control);
        }
    }
}
