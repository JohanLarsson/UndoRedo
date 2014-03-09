namespace UndoRedo.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;

    public class UndoManager
    {
        readonly Stack<IUndoOperation> _undoStack = new Stack<IUndoOperation>();
        readonly Stack<IUndoOperation> _redoStack = new Stack<IUndoOperation>();
        private static readonly Dictionary<string, UndoManager> UndoManagers = new Dictionary<string, UndoManager>();
        public static readonly DependencyProperty UndoScopeNameProperty = DependencyProperty.RegisterAttached(
            "UndoScopeName",
            typeof(string),
            typeof(UndoManager),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.Inherits,
                    OnUseGlobalUndoRedoScopeChanged));

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
            if (args.OldValue != null || args.NewValue == null)
            {
                return;
                throw new NotImplementedException("Changing scopes not implemented");
            }
            var scopeName = (string)args.NewValue;
            UndoManager manager;
            if (!UndoManagers.TryGetValue(scopeName, out manager))
            {
                manager = new UndoManager();
                UndoManagers.Add(scopeName, manager);
            }
            var uiElement = o as UIElement;
            if (uiElement != null)
            {
                ((UIElement)o).AddHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(manager.ExecutedHandler));
                ((UIElement)o).AddHandler(CommandManager.PreviewCanExecuteEvent, new CanExecuteRoutedEventHandler(manager.CanExecuteHandler));
            }
            Binding binding;
            var textBox = o as TextBox;
            if (textBox != null)
            {
                if (textBox.IsInitialized)
                {
                    binding = BindingOperations.GetBinding(textBox, TextBox.TextProperty);
                }
                else
                {
                    textBox.Initialized += TextBoxOnInitialized;
                }
                //textBox.TextChanged += (sender,e)=> manager.AddUndoableAction(new TextBoxUndoOperation((TextBoxBase)sender, e.UndoAction), e.UndoAction);
            }
            var toggleButton = o as ToggleButton;
            if (toggleButton != null)
            {
                binding = BindingOperations.GetBinding(toggleButton, ToggleButton.IsCheckedProperty);
                //toggleButton.Checked += (sender, e) => manager.AddUndoableAction(new TobbleButtonUndoOperation((ToggleButton)sender, e.UndoAction), e.UndoAction);
            }

        }
        private static void TextBoxOnInitialized(object sender, EventArgs eventArgs)
        {
           var binding = BindingOperations.GetBinding((TextBoxBase)sender, TextBox.TextProperty);
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
                Undo();
                e.Handled = true;
            }

            if (e.Command == ApplicationCommands.Redo)
            {
                Redo();
                e.Handled = true;
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