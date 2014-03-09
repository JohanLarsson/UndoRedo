namespace UndoRedoTests
{
    using System.Windows;
    using System.Windows.Controls;
    using NUnit.Framework;
    using UndoRedo.Data;
    [RequiresSTA]
    public class UndoTests
    {
        [Test]
        public void TestNameTest()
        {
            var grid = new Grid();
            grid.Children.Add(new TextBox{Text = "Initial"});
            grid.SetValue(UndoManager.UndoScopeNameProperty,"ScopeName");
            Initialize(grid);
        }

        public void Initialize<T>(T element) where  T : FrameworkElement
        {
            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            element.Arrange(new Rect(element.DesiredSize));
            element.UpdateLayout();
        }
    }
}
