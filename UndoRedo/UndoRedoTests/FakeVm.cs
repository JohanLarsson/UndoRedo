namespace UndoRedoTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UndoRedo.Annotations;

    public class FakeVm : INotifyPropertyChanged
    {
        private string _value;
        private DummyEnum _selectedEnum;

        public FakeVm()
        {
            Value = "1";
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public string Value
        {
            get { return _value; }
            set
            {
                if (value == _value) return;
                _value = value;
                OnPropertyChanged();
            }
        }

        public DummyEnum SelectedEnum
        {
            get { return _selectedEnum; }
            set
            {
                if (value == _selectedEnum) return;
                _selectedEnum = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<DummyEnum> EnumValues
        {
            get
            {
                return Enum.GetValues(typeof (DummyEnum))
                           .Cast<DummyEnum>();
            }
        }
            
            [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum DummyEnum
    {
        First, Last, Everything
    }
}