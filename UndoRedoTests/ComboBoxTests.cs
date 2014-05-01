namespace UndoRedoTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using NUnit.Framework;
    using UndoRedo;
    [RequiresSTA]
    public class ComboBoxTests
    {
        private ComboBox _comboBox;
        private UndoManager _undoManager;
        private FakeVm _fakeVm;

        [SetUp]
        public void SetUp()
        {
            _comboBox = new ComboBox();
            _comboBox.SetValue(UndoManager.UndoScopeNameProperty, "ScopeName");
            _fakeVm = new FakeVm();
            var selected = new Binding("SelectedEnum")
            {
                Source = _fakeVm,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(_comboBox, Selector.SelectedItemProperty, selected);

            var itemsSource = new Binding("EnumValues")
            {
                Source = _fakeVm,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true,
                Mode = BindingMode.OneWay
            };
            BindingOperations.SetBinding(_comboBox, ItemsControl.ItemsSourceProperty, itemsSource);
            _comboBox.DataContext = _fakeVm;
            _undoManager = UndoManager.GetUndoManager(_comboBox);
        }

        [Test]
        public void UndoChange()
        {
            var bindingExpression = BindingOperations.GetBindingExpression(_comboBox, Selector.SelectedItemProperty);
            bindingExpression.UpdateTarget();

            Assert.AreEqual(DummyEnum.First, _comboBox.SelectedItem);

            _comboBox.SetCurrentValue(Selector.SelectedItemProperty, DummyEnum.Last);
            bindingExpression.UpdateSource();
            Assert.AreEqual(DummyEnum.Last, _comboBox.SelectedItem);
            Assert.IsTrue(_undoManager.History.CanUndo(_comboBox));
            Assert.IsFalse(_undoManager.History.CanRedo);

            //Assert.IsFalse(_undoManager.History.IsDirty(_comboBox));
            //Assert.IsTrue(_undoManager.History.CanUndo(_comboBox));
            //_undoManager.History.Undo(_comboBox);
            //Assert.IsFalse(_undoManager.History.IsDirty(_comboBox));
            //Assert.IsFalse(_undoManager.History.CanUndo(_comboBox));
        }
    }
}
