namespace UndoRedoTests
{
    using System.Windows.Controls;
    using System.Windows.Data;
    using NUnit.Framework;
    [RequiresSTA]
    public class UpdateSourceAndTarget
    {
        [Test]
        public void UpdateTargetTest()
        {
            var textBox = new TextBox();
            var fakeVm = new FakeVm();
            var binding = new Binding("Value")
            {
                Source = fakeVm,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            textBox.DataContext = fakeVm;
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            bindingExpression.UpdateTarget();
            Assert.AreEqual("1", textBox.Text);
            fakeVm.Value = "2";
            Assert.AreEqual("1", textBox.Text);
            bindingExpression.UpdateTarget();
            Assert.AreEqual("2", textBox.Text);
        }

        [Test]
        public void UpdateSourceTest()
        {
            var textBox = new TextBox();
            var fakeVm = new FakeVm();
            var binding = new Binding("Value")
            {
                Source = fakeVm,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
            textBox.DataContext = fakeVm;
            var bindingExpression = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            bindingExpression.UpdateTarget();
            Assert.AreEqual("1", textBox.Text);
            textBox.SetCurrentValue(TextBox.TextProperty, "2");
            Assert.AreEqual("1", fakeVm.Value);
            bindingExpression.UpdateSource();
            Assert.AreEqual("2", fakeVm.Value);
        }

        [Test]
        public void TextBoxUndoTest()
        {
            var textBox = new TextBox();
            Assert.IsTrue(textBox.IsUndoEnabled);
            Assert.AreEqual(100,textBox.UndoLimit);
            textBox.AppendText("1");
            Assert.IsTrue(textBox.CanUndo);
            // Assert.AreEqual(null, textBox.Text);
        }
    }
}
