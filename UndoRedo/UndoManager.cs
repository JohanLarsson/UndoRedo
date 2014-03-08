namespace UndoRedo
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    public class UndoManager
    {
        Stack<UndoOperation> undoStack = new Stack<UndoOperation>();
        Stack<UndoOperation> redoStack = new Stack<UndoOperation>();

        #region Dependency Properties
        public static readonly DependencyProperty SharedUndoRedoScopeProperty =
            DependencyProperty.RegisterAttached("SharedUndoRedoScope", typeof(UndoManager), typeof(UndoManager),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnUseGlobalUndoRedoScopeChanged)));

        public static void SetSharedUndoRedoScope(DependencyObject depObj, bool isSet)
        {
            // never place logic in here, because these methods are not called when things are done in XAML
            depObj.SetValue(SharedUndoRedoScopeProperty, isSet);
        }

        public static UndoManager GetSharedUndoRedoScope(DependencyObject depObj)
        {
            // never place logic in here, because these methods are not called when things are done in XAML
            return depObj.GetValue(SharedUndoRedoScopeProperty) as UndoManager;
        }

        private static void OnUseGlobalUndoRedoScopeChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            if (depObj is TextBoxBase)
            {
                if (args.OldValue != null)
                {
                    RemoveEventHandlers(depObj as TextBoxBase, args.OldValue as UndoManager);
                }
                if (args.NewValue != null)
                {
                    AttachEventHandlers(depObj as TextBoxBase, args.NewValue as UndoManager);
                }
            }
        }

        private static void AttachEventHandlers(TextBoxBase textBox, UndoManager manager)
        {
            if (textBox != null && manager != null)
            {
                textBox.AddHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(manager.ExecutedHandler), true); // we need to see all events to subvert the built-in undo/redo tracking in the text boxes
                textBox.TextChanged += new TextChangedEventHandler(manager.TextChangedHandler);
            }
        }

        private static void RemoveEventHandlers(TextBoxBase textBox, UndoManager manager)
        {
            if (textBox != null && manager != null)
            {
                textBox.RemoveHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(manager.ExecutedHandler));
                textBox.TextChanged -= new TextChangedEventHandler(manager.TextChangedHandler);
            }
        }

        #endregion

        //          <Grid Background="#feca00" >
        //<undo:UndoManager.SharedUndoRedoScope>
        //  <undo:UndoManager />
        //</undo:UndoManager.SharedUndoRedoScope>

        void TextChangedHandler(object sender, TextChangedEventArgs e)
        {
            this.AddUndoableAction(sender as TextBoxBase, e.UndoAction);
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
                redoStack.Push(new UndoOperation(sender, action));
            }
            else
            {
                if (undoStack.Count > 0)
                {
                    UndoOperation op = undoStack.Peek();
                    if ((op.Sender == sender) && (action == UndoAction.Merge))
                    {
                        // no-op
                    }
                    else
                    {
                        PushUndoOperation(sender, action);
                    }
                }
                else
                {
                    PushUndoOperation(sender, action);
                }
            }
        }

        private void PushUndoOperation(TextBoxBase sender, UndoAction action)
        {
            undoStack.Push(new UndoOperation(sender, action));
            System.Diagnostics.Debug.WriteLine("PUSHED");
        }

        public void Undo()
        {

            if (undoStack.Count > 0)
            {
                UndoOperation op = undoStack.Pop();
                op.Sender.Undo();
                op.Sender.Focus();
            }
        }

        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                UndoOperation op = redoStack.Pop();
                op.Sender.Redo();
                op.Sender.Focus();
            }
        }
    }
}