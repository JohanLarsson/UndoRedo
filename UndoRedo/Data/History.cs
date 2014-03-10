namespace UndoRedo.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using Annotations;

    public class History : INotifyPropertyChanged
    {
        private readonly Stack<HistoryPoint> _undoStack = new Stack<HistoryPoint>();
        private readonly Stack<HistoryPoint> _redoStack = new Stack<HistoryPoint>();
        //private readonly Dictionary<Control, HistoryPoint> _currentvalues = new Dictionary<Control, HistoryPoint>();
        public Stack<HistoryPoint> UndoStack
        {
            get
            {
                return _undoStack;
            }
        }
        public Stack<HistoryPoint> RedoStack
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
        public void Update(HistoryPoint historyPoint)
        {
            var cv = _undoStack.FirstOrDefault(x => ReferenceEquals(x.Control, historyPoint.Control));
            if (cv != null && cv.UpdateReason != UpdateReason.DataUpdated && Equals(cv.Value, historyPoint.Value))
                return;
            if (_redoStack.Any())
            {
                HistoryPoint redoPoint = _redoStack.Peek();
                if (ReferenceEquals(redoPoint.Control, historyPoint.Control) && Equals(redoPoint.Value, historyPoint.Value))
                    return;
            }

            _undoStack.Push(historyPoint);
            _redoStack.Clear();
            OnPropertyChanged("");
        }
        public void Update(Control historyPoint, object value, DependencyProperty property, UpdateReason userInput)
        {
            Update(HistoryPoint.Create(historyPoint, value, property, userInput));
        }
        public void Undo(Control control)
        {
            if (IsDirty(control))
            {
                var hp = _undoStack.First(x => ReferenceEquals(x.Control, control));
                _redoStack.Push(HistoryPoint.Create(control, hp.CurrentValue, hp.Property, UpdateReason.Undo));
                hp.Undo();
            }
            else
            {
                var hp = (_undoStack.Peek().UpdateReason != UpdateReason.DataUpdated)
                    ? _undoStack.Pop()
                    : _undoStack.Peek();
                _redoStack.Push(HistoryPoint.Create(hp.Control, hp.CurrentValue, hp.Property, UpdateReason.Undo));
                hp.Undo();
            }

            OnPropertyChanged("");
        }
        public void Redo(Control control)
        {
            var hp = _redoStack.Pop();
            hp.Redo();
            OnPropertyChanged("");
        }
        public bool CanUndo(Control control)
        {
            if (IsDirty(control))
                return true;
            HistoryPoint up = _undoStack.FirstOrDefault(x => ReferenceEquals(x.Control, control));
            if (up.UpdateReason != UpdateReason.DataUpdated)
                return true;
            return up.Value != up.CurrentValue;
        }
        private bool IsDirty(Control control)
        {
            HistoryPoint up = _undoStack.FirstOrDefault(x => ReferenceEquals(x.Control, control));
            if (up != null)
            {
                if (!Equals(up.Value, up.CurrentValue))
                {
                    return true;
                }
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
