namespace UndoRedoDemo.Dummies
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using UndoRedo.Annotations;

    public class DummyValues : INotifyPropertyChanged
    {
        private string _value1;
        private string _value2;
        private string _value3;
        private string _value4;
        private string _value5;
        private string _value6;

        public DummyValues(int i)
        {
            Value1 = i + "1";
            Value2 = i + "2";
            Value3 = i + "3";
            Value4 = i + "4";
            Value5 = i + "5";
            Value6 = i + "6";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public string Value1
        {
            get
            {
                return _value1;
            }
            set
            {
                if (value.Equals(_value1))
                {
                    return;
                }
                _value1 = value;
                OnPropertyChanged();
            }
        }

        public string Value2
        {
            get
            {
                return _value2;
            }
            set
            {
                if (value.Equals(_value2))
                {
                    return;
                }
                _value2 = value;
                OnPropertyChanged();
            }
        }

        public string Value3
        {
            get
            {
                return _value3;
            }
            set
            {
                if (value.Equals(_value3))
                {
                    return;
                }
                _value3 = value;
                OnPropertyChanged();
            }
        }

        public string Value4
        {
            get
            {
                return _value4;
            }
            set
            {
                if (value.Equals(_value4))
                {
                    return;
                }
                _value4 = value;
                OnPropertyChanged();
            }
        }

        public string Value5
        {
            get
            {
                return _value5;
            }
            set
            {
                if (value.Equals(_value5))
                {
                    return;
                }
                _value5 = value;
                OnPropertyChanged();
            }
        }

        public string Value6
        {
            get
            {
                return _value6;
            }
            set
            {
                if (value.Equals(_value6))
                {
                    return;
                }
                _value6 = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
