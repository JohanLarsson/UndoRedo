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
        readonly Stack<IUndoOperation> _undoStack = new Stack<IUndoOperation>();
        readonly Stack<IUndoOperation> _redoStack = new Stack<IUndoOperation>();
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
            UndoManager manager;
            var scopeName = (string)args.NewValue;
            if (!_undoManagers.TryGetValue(scopeName, out manager))
            {
                manager = new UndoManager();
                _undoManagers.Add(scopeName, manager);
            }
            var uiElement = o as UIElement;
            if (uiElement != null)
            {
                ((UIElement)o).AddHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(manager.ExecutedHandler));
                ((UIElement)o).AddHandler(CommandManager.PreviewCanExecuteEvent, new CanExecuteRoutedEventHandler(manager.CanExecuteHandler));
            }
            var textBox = o as TextBoxBase;
            if (textBox != null)
            {
                textBox.TextChanged += (sender,e)=> manager.AddUndoableAction(new TextBoxUndoOperation((TextBoxBase)sender, e.UndoAction), e.UndoAction);
            }
            var toggleButton = o as ToggleButton;
            if (toggleButton != null)
            {
                //toggleButton.Checked += (sender, e) => manager.AddUndoableAction(new TobbleButtonUndoOperation((ToggleButton)sender, e.UndoAction), e.UndoAction);
            }
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
        private void AddUndoableAction(IUndoOperation undoOperation, UndoAction action)
        {
            if (action == UndoAction.Undo)
            {
                _redoStack.Push(undoOperation);
            }
            else if (action == UndoAction.Redo)
            {
                _undoStack.Push(undoOperation);
            }
            else
            {
                _undoStack.Push(undoOperation);
                _redoStack.Clear();
            }
        }
        public void Undo()
        {
            IUndoOperation op = _undoStack.Pop();
            op.Undo();
        }
        public void Redo()
        {
            IUndoOperation op = _redoStack.Pop();
            op.Undo();
        }
    }
}