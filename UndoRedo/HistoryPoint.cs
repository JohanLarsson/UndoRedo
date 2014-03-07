namespace UndoRedo
{
    using System;
    using System.Reflection;

    public class HistoryPoint
    {
        public HistoryPoint(object source, PropertyInfo propertyInfo)
        {
            Time = DateTime.UtcNow;
            PropertyInfo = propertyInfo;
            Source = source;
            Value = propertyInfo.GetMethod.Invoke(source, null);
        }
        public DateTime Time { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }
        public object Source { get; private set; }
        public object Value { get; private set; }
        public void Undo()
        {
            PropertyInfo.SetMethod.Invoke(Source, new[] { Value });
        }
    }
}