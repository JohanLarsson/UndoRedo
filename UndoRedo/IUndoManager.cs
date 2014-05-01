namespace UndoRedo
{
    using System;

    public interface IUndoManager
    {
        History History { get; }
        event EventHandler OnUndo;
        event EventHandler OnRedo;
        event EventHandler OnClear;
        void UndoAll();
        void Clear();
    }
}