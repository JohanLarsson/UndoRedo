namespace UndoRedo.Data
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
        public DependencyProperty Property { get; set; }
        public UpdateReason UpdateReason { get; private set; }
    }
}
