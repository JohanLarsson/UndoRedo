namespace UndoRedo.View
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Controls;
    using Annotations;

    public class History : INotifyPropertyChanged
    {
        readonly Stack<IUndoOperation> _undoStack = new Stack<IUndoOperation>();
        readonly Stack<IUndoOperation> _redoStack = new Stack<IUndoOperation>();
        public event PropertyChangedEventHandler PropertyChanged;
        public Stack<IUndoOperation> UndoStack
        {
            get
            {
                return _undoStack;
            }
        }
        public Stack<IUndoOperation> RedoStack
        {
            get
            {
                return _redoStack;
            }
        } 
        public bool CanRedo
        {
            get
            {
                return _redoStack.Any();
            }
        }
        public bool CanUndo
        {
            get
            {
                return _undoStack.Any();
            }
        }
        public void Add(IUndoOperation undoOperation)
        {
            _undoStack.Push(undoOperation);
            OnPropertyChanged("");
        }
        public void Undo()
        {
            IUndoOperation undoOperation = _undoStack.Pop();
            _redoStack.Push(undoOperation);
            undoOperation.Undo();
            OnPropertyChanged("");
        }
        public void Redo()
        {
            IUndoOperation undoOperation = _redoStack.Pop();
            undoOperation.Redo();
            OnPropertyChanged("");
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
