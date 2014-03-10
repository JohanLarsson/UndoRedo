namespace UndoRedo.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using Annotations;

    public class History : INotifyPropertyChanged
    {
        private readonly Stack<HistoryPoint> _undoStack = new Stack<HistoryPoint>();
        private readonly Stack<HistoryPoint> _redoStack = new Stack<HistoryPoint>();
        public event PropertyChangedEventHandler PropertyChanged;
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
        public void Update(TextBoxHistoryPoint historyPoint)
        {
            var cv = _undoStack.FirstOrDefault(x => ReferenceEquals(x.Control, historyPoint.Control));
            if (cv != null && Equals(cv.Value, historyPoint.Value))
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
        public void Update(HistoryPoint historyPoint)
        {
            var cv = _undoStack.FirstOrDefault(x => x.UpdateReason != UpdateReason.DataUpdated && ReferenceEquals(x.Control, historyPoint.Control));
            if (cv != null && Equals(cv.Value, historyPoint.Value))
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
        public void Update(Control control, object value, DependencyProperty property, UpdateReason reason)
        {
            var textBoxBase = control as TextBoxBase;
            if(textBoxBase!=null)
                Update(new TextBoxHistoryPoint(textBoxBase,value,property,reason));
            else
                Update(new HistoryPoint(control, value, property, reason));
        }
        public void Undo(Control control)
        {
            if (IsDirty(control))
            {
                var hp = _undoStack.First(x => ReferenceEquals(x.Control, control));
                _redoStack.Push(hp.ToUndoPoint());
                hp.Undo();
            }
            else
            {
                var hp = (_undoStack.Peek().UpdateReason != UpdateReason.DataUpdated)
                    ? _undoStack.Pop()
                    : _undoStack.Peek();
                _redoStack.Push(hp.ToUndoPoint());
                hp.Undo();
            }

            OnPropertyChanged("");
        }
        public void Redo(Control control)
        {
            var hp = _redoStack.Pop();
            _undoStack.Push(hp.ToRedoPoint());
            hp.Redo();
            OnPropertyChanged("");
        }
        public bool CanUndo(Control control)
        {
            if (IsDirty(control))
                return true;
            HistoryPoint up = _undoStack.Peek();
            if (up.UpdateReason != UpdateReason.DataUpdated)
                return true;
            return !Equals(up.Value, up.CurrentValue);
        }
        public bool IsDirty(Control control)
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
