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
    using View;

    public class UndoManager
    {
        private readonly History _history = new History();
        private readonly List<Control> _controls = new List<Control>();
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
            var textBox = o as TextBox;
            if (textBox != null)
            {
                manager._controls.Add(textBox);
                if (textBox.IsInitialized)
                {
                    // binding = BindingOperations.GetBinding(textBox, TextBox.TextProperty);
                }
                else
                {
                    textBox.DataContextChanged += (sender, _) => Subscribe((TextBoxBase)sender, TextBox.TextProperty);
                }
            }
            var toggleButton = o as ToggleButton;
            if (toggleButton != null)
            {
                manager._controls.Add(toggleButton);
                toggleButton.DataContextChanged += (sender, _) => Subscribe((ToggleButton) sender, ToggleButton.IsCheckedProperty);
            }
        }

        private static void Subscribe(Control control, DependencyProperty property)
        {
            var binding = BindingOperations.GetBinding(control, property);
            if (binding != null && binding.IsBoundToDataContext())
            {
                if (!binding.IsNotifiying())
                {
                    binding = binding.CreateNotifying(control.DataContext);
                    BindingOperations.ClearBinding(control, property);
                    BindingOperations.SetBinding(control, property, binding);
                }
                control.SourceUpdated += (o, args) =>
                {
                    if (args.Property == property)
                    {
                        var undoManager = GetUndoManager(control);
                        var value = control.GetValue(property);
                        undoManager._history.Update(control, value, property, UpdateReason.UserInput);
                    }
                };
                control.TargetUpdated += (o, args) =>
                {
                    if (args.Property == property)
                    {
                        var undoManager = GetUndoManager(control);
                        var value = control.GetValue(property);
                        undoManager._history.Update(control, value, property, UpdateReason.FromData);
                    }
                };
            }
        }
        private static UndoManager GetUndoManager(Control control)
        {
            foreach (var undoManager in UndoManagers)
            {
                if (undoManager.Value._controls.Any(x => ReferenceEquals(x, control)))
                    return undoManager.Value;
            }
            throw new ArgumentOutOfRangeException("control", "No undomanager found for control");
        }
        private void CanExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Undo)
            {
                e.CanExecute = _history.CanUndo((Control)e.OriginalSource);
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
                _history.Undo((Control)e.OriginalSource);
                e.Handled = true;
            }

            if (e.Command == ApplicationCommands.Redo)
            {
                _history.Redo((Control)e.OriginalSource);
                e.Handled = true;
            }
        }
    }
}