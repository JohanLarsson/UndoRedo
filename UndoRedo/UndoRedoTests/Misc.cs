namespace UndoRedoTests
{
    using System.Windows;

    public static class Misc
    {
        public static void Initialize<T>(T element) where T : FrameworkElement
        {
            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            element.Arrange(new Rect(element.DesiredSize));
            element.UpdateLayout();
        }
    }
}
