namespace UndoRedo
{
    using System;
    using System.Collections.Generic;

    public interface IHistory
    {
        event EventHandler OnClear;
        Stack<HistoryPoint> UndoStack { get; }
        Stack<HistoryPoint> RedoStack { get; }
        bool CanRedo { get; }
        void Clear();
    }
}