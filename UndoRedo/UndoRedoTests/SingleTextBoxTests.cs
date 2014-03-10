namespace UndoRedoTests
{
    using System.Windows.Controls;
    using System.Windows.Data;
    using NUnit.Framework;
    using UndoRedo.Data;
    [RequiresSTA]
    public class SingleTextBoxTests
    {
        private TextBox _textBox;
        private UndoManager _undoManager;
        private FakeVm _fakeVm;

        [SetUp]
        public void SetUp()
        {
            _textBox = new TextBox();
            _textBox.SetValue(UndoManager.UndoScopeNameProperty, "ScopeName");
            _fakeVm = new FakeVm();
            var binding = new Binding("Value")
            {
                Source = _fakeVm,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(_textBox, TextBox.TextProperty, binding);
            _textBox.DataContext = _fakeVm;
            _undoManager = UndoManager.GetUndoManager(_textBox);
        }
        [Test]
        public void IsDirty()
        {
            var bindingExpression = BindingOperations.GetBindingExpression(_textBox, TextBox.TextProperty);
            bindingExpression.UpdateTarget();

            Assert.AreEqual("1", _textBox.Text);

            _textBox.SetCurrentValue(TextBox.TextProperty, "2");
            Assert.IsTrue(_undoManager.History.IsDirty(_textBox));
        }

        [Test]
        public void UndoDirty()
        {
            var bindingExpression = BindingOperations.GetBindingExpression(_textBox, TextBox.TextProperty);
            bindingExpression.UpdateTarget();

            Assert.AreEqual("1", _textBox.Text);

            _textBox.SetCurrentValue(TextBox.TextProperty, "2");
            Assert.IsTrue(_undoManager.History.IsDirty(_textBox));
            Assert.IsTrue(_undoManager.History.CanUndo(_textBox));
            Assert.IsTrue(_textBox.CanUndo);
            _undoManager.History.Undo(_textBox);
            Assert.IsFalse(_undoManager.History.IsDirty(_textBox));
            Assert.IsFalse(_undoManager.History.CanUndo(_textBox));
        }

        [Test]
        public void UndoChange()
        {
            var bindingExpression = BindingOperations.GetBindingExpression(_textBox, TextBox.TextProperty);
            bindingExpression.UpdateTarget();

            Assert.AreEqual("1", _textBox.Text);

            _textBox.SetCurrentValue(TextBox.TextProperty, "2");
            bindingExpression.UpdateSource();
            Assert.IsFalse(_undoManager.History.IsDirty(_textBox));
            Assert.IsTrue(_undoManager.History.CanUndo(_textBox));
            _undoManager.History.Undo(_textBox);
            Assert.IsFalse(_undoManager.History.IsDirty(_textBox));
            Assert.IsFalse(_undoManager.History.CanUndo(_textBox));
        }
    }
}
