namespace UndoRedo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class Dto : INotifyPropertyChanged
    {
        private string _value1;
        private string _value2;
        private bool? _isChecked;

        public Dto()
        {
            Value1 = "OriginalValue";
            Value2 = "OriginalValue";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public string Value1
        {
            get { return _value1; }
            set
            {
                if (value == _value1)
                {
                    return;
                }
                _value1 = value;
                OnPropertyChanged();
            }
        }
        public string Value2
        {
            get { return _value2; }
            set
            {
                if (value == _value2)
                {
                    return;
                }
                _value2 = value;
                OnPropertyChanged();
            }
        }
        public bool? IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value.Equals(_isChecked)) return;
                _isChecked = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
