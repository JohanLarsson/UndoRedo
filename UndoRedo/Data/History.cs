namespace UndoRedo.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public class History
    {
        private readonly Stack<HistoryPoint> _undoStack = new Stack<HistoryPoint>();
        private readonly Stack<HistoryPoint> _redoStack = new Stack<HistoryPoint>();

        public void Update(Control control, object value, DependencyProperty property, UpdateReason updateReason)
        {
            if (HasHistory(control))
            {
                var undoValue = UndoValue(control);
                if (undoValue == value)
                    return;
            }
            _undoStack.Push(new HistoryPoint(control, value, property, updateReason));
            _redoStack.Clear();
        }
        public void Undo(Control control)
        {
            var undoValue = UndoValue(control);
            var currentValue = CurrentValue(control);
            var property = Property(control);
            if (undoValue != currentValue)
            {
                _redoStack.Push(new HistoryPoint(control, currentValue, property, UpdateReason.Undo));
                control.SetCurrentValue(property, undoValue);
                control.Focus();
                return;
            }
            var historyPoint = _undoStack.Pop();
            _redoStack.Push(historyPoint);
            var value = UndoValue(historyPoint.Control);
            historyPoint.Control.SetCurrentValue(property, value);
            historyPoint.Control.Focus();
        }
        public void Redo(Control control)
        {
            var undoValue = UndoValue(control);
            var currentValue = CurrentValue(control);
            if (undoValue != currentValue)
            {
                _redoStack.Clear();
                return;
            }
            var historyPoint = _redoStack.Pop();
            _undoStack.Push(historyPoint);
            historyPoint.Control.SetCurrentValue(historyPoint.Property, historyPoint.Value);
            historyPoint.Control.Focus();
        }
        public bool CanUndo(Control control)
        {
            var undoValue = UndoValue(control);
            if (undoValue != CurrentValue(control))
                return true;
            if (_undoStack.Any())
            {
                return _undoStack.Peek().UpdateReason != UpdateReason.FromData;
            }
            return false;
        }
        public bool CanRedo
        {
            get
            {
                return _redoStack.Any();
            }
        }
        private object CurrentValue(Control control)
        {
            return control.GetValue(Property(control));
        }
        private DependencyProperty Property(Control control)
        {
            var historyPoint = UndoPoint(control);
            return historyPoint.Property;
        }
        private bool HasHistory(Control control)
        {
            return _undoStack.Any(x => ReferenceEquals(x.Control, control));
        }
        private HistoryPoint UndoPoint(Control control)
        {
            return _undoStack.First(x => ReferenceEquals(x.Control, control));
        }
        private object UndoValue(Control control)
        {
            return UndoPoint(control).Value;
        }
    }
}
