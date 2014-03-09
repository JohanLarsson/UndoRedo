namespace UndoRedo.Data
{
    using System.Windows;
    using System.Windows.Controls.Primitives;

    public class TextBoxHistoryPoint : HistoryPoint
    {
        public TextBoxHistoryPoint(TextBoxBase control, object value, DependencyProperty property, UpdateReason updateReason)
            : base(control, value, property, updateReason)
        {
        }
        public override void Undo()
        {
            ((TextBoxBase)Control).Undo();
            Control.Focus();
        }
        public override void Redo()
        {
            ((TextBoxBase)Control).Redo();
            Control.Focus();
        }
    }
}