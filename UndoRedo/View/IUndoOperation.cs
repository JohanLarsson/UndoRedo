namespace UndoRedo.View
{
    using System;

    public interface IUndoOperation
    {
        DateTime Timestamp { get; }
        void Undo();
    }
}