namespace UndoRedo.Data
{
    using System.Windows;
    using System.Windows.Controls.Primitives;

    public class TextBoxHistoryPoint : HistoryPoint
    {
        public TextBoxHistoryPoint(TextBoxBase textbox, object value, DependencyProperty property, UpdateReason updateReason)
            : base(textbox, value, property, updateReason)
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
        public override HistoryPoint ToUndoPoint()
        {
            return new TextBoxHistoryPoint((TextBoxBase) Control, CurrentValue, Property, UpdateReason.Undo);
        }
        public override HistoryPoint ToRedoPoint()
        {
            return new TextBoxHistoryPoint((TextBoxBase) Control, Value, Property, UpdateReason.Undo);
        }
    }
}