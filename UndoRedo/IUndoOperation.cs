namespace UndoRedo
{
    using System;
    using System.Windows.Controls;

    public interface IUndoOperation
    {
        DateTime Timestamp { get; }
        void Undo();
    }
}