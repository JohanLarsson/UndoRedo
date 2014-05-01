namespace UndoRedo
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;

    public class UndoManager : IUndoManager
    {
        private readonly History _history = new History();
        private readonly List<Control> _controls = new List<Control>();
        internal static readonly ConcurrentDictionary<string, UndoManager> UndoManagers = new ConcurrentDictionary<string, UndoManager>();
        public static readonly DependencyProperty UndoScopeNameProperty = DependencyProperty.RegisterAttached(
            "UndoScopeName",
            typeof(string),
            typeof(UndoManager),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.Inherits,
                    OnScopeNameChanged));

        public UndoManager()
        {
            _history.OnClear += (sender, args) =>  OnOnClear();
            _history.OnChanged += (sender, args) => OnChanged();
        }
        public History History
        {
            get
            {
                return _history;
            }
        }
        public event EventHandler Changed;

        public event EventHandler OnUndo;

        public event EventHandler OnRedo;

        public event EventHandler OnClear;

        public static void SetUndoScopeName(DependencyObject o, string name)
        {
            o.SetValue(UndoScopeNameProperty, name);
        }
        public static string GetUndoScopeName(DependencyObject o)
        {
            return (string)o.GetValue(UndoScopeNameProperty);
        }

        public static UndoManager GetByName(string name)
        {
            return UndoManagers.GetOrAdd(name, x => new UndoManager());
        }

        public static UndoManager GetUndoManager(Control control)
        {
            foreach (var undoManager in UndoManagers)
            {
                if (undoManager.Value._controls.Any(x => ReferenceEquals(x, control)))
                {
                    return undoManager.Value;
                }
            }
            throw new ArgumentOutOfRangeException("control", "No undomanager found for control");
        }

        public void UndoAll()
        {
            while (_history.UndoStack.Any())
            {
                _history.Undo(_controls.FirstOrDefault(x=>x.IsFocused));
            }
        }

        public void Clear()
        {
            _history.Clear();
        }
        private static void OnScopeNameChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var adorner = o as Adorner;
            if (adorner != null)
            {
                return;
            }
            if (args.OldValue != null || args.NewValue == null)
            {
                throw new NotImplementedException("Changing scopes not implemented");
            }
            var scopeName = (string)args.NewValue;
            UndoManager manager = GetByName(scopeName);
            var uiElement = o as UIElement;
            if (uiElement != null)
            {
                uiElement.AddHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(manager.ExecutedHandler));
                uiElement.AddHandler(CommandManager.PreviewCanExecuteEvent, new CanExecuteRoutedEventHandler(manager.CanExecuteHandler));
            }
            var textBox = o as TextBox;
            if (textBox != null)
            {
                manager._controls.Add(textBox);
                textBox.Loaded += (sender, _) => Subscribe((TextBoxBase)sender, TextBox.TextProperty);
            }
            var toggleButton = o as ToggleButton;
            if (toggleButton != null)
            {
                manager._controls.Add(toggleButton);
                toggleButton.Loaded += (sender, _) => Subscribe((ToggleButton)sender, ToggleButton.IsCheckedProperty);
            }

            var selector = o as Selector;
            if (selector != null)
            {
                manager._controls.Add(selector);
                selector.Loaded += (sender, _) => Subscribe((Selector)sender, Selector.SelectedItemProperty);
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
                        undoManager._history.Update(control, value, property, UpdateReason.DataUpdated);
                    }
                };
            }
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
                OnOnUndo();
            }

            if (e.Command == ApplicationCommands.Redo)
            {
                _history.Redo((Control)e.OriginalSource);
                e.Handled = true;
                OnOnRedo();
            }
        }

        protected virtual void OnOnUndo()
        {
            EventHandler handler = OnUndo;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnOnRedo()
        {
            EventHandler handler = OnRedo;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnOnClear()
        {
            EventHandler handler = OnClear;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnChanged()
        {
            EventHandler handler = Changed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}