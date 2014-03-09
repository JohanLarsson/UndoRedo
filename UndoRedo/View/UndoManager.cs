namespace UndoRedo.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    public class UndoManager
    {
        private readonly History _history = new History();
        public static readonly Dictionary<string, UndoManager> UndoManagers = new Dictionary<string, UndoManager>();
        public static readonly DependencyProperty UndoScopeNameProperty = DependencyProperty.RegisterAttached(
            "UndoScopeName",
            typeof(string),
            typeof(UndoManager),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.Inherits,
                    OnUseGlobalUndoRedoScopeChanged));

        public History History
        {
            get
            {
                return _history;
            }
        }
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
            var textBox = o as TextBoxBase;
            if (textBox != null)
            {
                textBox.TextChanged += (sender, e) =>
                {
                    manager._history.Add(new TextBoxUndoOperation((TextBoxBase) sender, e));
                };
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
                e.CanExecute = _history.CanUndo;
                e.Handled = true;
            }

            if (e.Command == ApplicationCommands.Redo)
            {
                e.CanExecute = _history.CanRedo;
                e.Handled = true;
            }
        }
        private void ExecutedHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Undo)
            {
                _history.Undo();
                e.Handled = true;
            }

            if (e.Command == ApplicationCommands.Redo)
            {
                _history.Redo();
                e.Handled = true;
            }
        }
    }
}