namespace UndoRedo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    public class UndoManager
    {
        readonly Stack<UndoOperation> _undoStack = new Stack<UndoOperation>();
        readonly Stack<UndoOperation> _redoStack = new Stack<UndoOperation>();
        private static Dictionary<string, UndoManager> _undoManagers = new Dictionary<string, UndoManager>();
        public static readonly DependencyProperty UndoScopeNameProperty = DependencyProperty.RegisterAttached(
            "UndoScopeName",
            typeof(string),
            typeof(UndoManager),
                new FrameworkPropertyMetadata(
                    "",
                    FrameworkPropertyMetadataOptions.Inherits, OnUseGlobalUndoRedoScopeChanged));

        public static void SetUndoScopeName(DependencyObject o, string name)
        {
            o.SetValue(UndoScopeNameProperty, name);
        }
        public static string GetUndoScopeName(DependencyObject o)
        {
            return (string)o.GetValue(UndoScopeNameProperty);
        }
        private static void OnUseGlobalUndoRedoScopeChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            UndoManager mgr;
            var scopeName = (string)args.NewValue;
            if (!_undoManagers.TryGetValue(scopeName, out mgr))
            {
                mgr = new UndoManager();
                _undoManagers.Add(scopeName, mgr);
            }
            var uiElement = o as UIElement;
            if (uiElement != null)
            {
                ((UIElement)o).AddHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(mgr.ExecutedHandler));
                ((UIElement)o).AddHandler(CommandManager.PreviewCanExecuteEvent, new CanExecuteRoutedEventHandler(mgr.CanExecuteHandler));
            }
            var box = o as TextBoxBase;
            if (box != null)
            {
                AttachEventHandlers(box, mgr);
            }
        }

        private static void AttachEventHandlers(TextBoxBase textBox, UndoManager manager)
        {
            textBox.TextChanged += manager.TextChangedHandler;
        }
        void TextChangedHandler(object sender, TextChangedEventArgs e)
        {
            this.AddUndoableAction(sender as TextBoxBase, e.UndoAction);
        }
        private void CanExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Undo)
            {
                e.CanExecute = _undoStack.Any();
                e.Handled = true;
            }

            if (e.Command == ApplicationCommands.Redo)
            {
                e.CanExecute = _redoStack.Any();
                e.Handled = true;
            }

        }
        private void ExecutedHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Undo)
            {
                e.Handled = true;
                Undo();
            }

            if (e.Command == ApplicationCommands.Redo)
            {
                e.Handled = true;
                Redo();
            }
        }
        private void AddUndoableAction(TextBoxBase sender, UndoAction action)
        {
            if (action == UndoAction.Undo)
            {
                _redoStack.Push(new UndoOperation(sender, action));
            }
            else if (action == UndoAction.Redo)
            {
                _undoStack.Push(new UndoOperation(sender, action));
            }
            else
            {
                _undoStack.Push(new UndoOperation(sender, action));
                _redoStack.Clear();
            }
        }
        public void Undo()
        {
            UndoOperation op = _undoStack.Pop();
            op.Sender.Undo();
            op.Sender.Focus();
        }
        public void Redo()
        {
            UndoOperation op = _redoStack.Pop();
            op.Sender.Redo();
            op.Sender.Focus();
        }
    }
}