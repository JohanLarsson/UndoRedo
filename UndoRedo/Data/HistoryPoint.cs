namespace UndoRedo.Data
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public class HistoryPoint
    {
        protected HistoryPoint(Control control, object value, DependencyProperty property, UpdateReason updateReason)
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
        public static HistoryPoint Create(Control control, object value, DependencyProperty property, UpdateReason updateReason)
        {
            var textBoxBase = control as TextBoxBase;
            if (textBoxBase != null)
                return Create(textBoxBase, value, property, updateReason);
            return new HistoryPoint(control, value, property, updateReason);
        }
        public static TextBoxHistoryPoint Create(TextBoxBase textBox, object value, DependencyProperty property, UpdateReason updateReason)
        {
            return new TextBoxHistoryPoint(textBox, value, property, updateReason);
        }
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
        public override string ToString()
        {
            return string.Format("Timestamp: {0}, Control: {1}", Timestamp, Control);
        }
    }
}
