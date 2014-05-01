namespace UndoRedoDemo.Dummies
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UndoRedo;
    using UndoRedo.Annotations;

    public class Dto : INotifyPropertyChanged
    {
        private string _value1;
        private string _value2;
        private bool? _isChecked;
        private DummyEnum _selectedDummyEnum;

        public Dto(string name)
        {
            Name = name;
            Value1 = "OriginalValue";
            Value2 = "OriginalValue";
            UndoManager = new UndoManagerVm(name);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; private set; }
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
        public DummyEnum SelectedDummyEnum
        {
            get { return _selectedDummyEnum; }
            set
            {
                if (value == _selectedDummyEnum)
                {
                    return;
                }
                _selectedDummyEnum = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<DummyEnum> EnumValues
        {
            get
            {
                return Enum.GetValues(typeof(DummyEnum)).Cast<DummyEnum>();
            }
        }

        public UndoManagerVm UndoManager { get; private set; }
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

    public enum DummyEnum
    {
        First, Second, Third
    }
}
