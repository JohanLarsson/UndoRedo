namespace UndoRedo
{
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public class UndoOperation
    {
        public TextBoxBase Sender;
        public UndoAction Action;

        public UndoOperation(TextBoxBase sender, UndoAction action)
        {
            this.Sender = sender;
            this.Action = action;
        }
    }
}
