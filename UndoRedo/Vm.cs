namespace UndoRedo
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Data;
    using Annotations;
    using Data;

    public class Vm : INotifyPropertyChanged
    {
        public Vm()
        {
            Dto1 = new Dto();
            Dto2 = new Dto();
            Dto3 = new Dto();
            Dto4 = new Dto();
            UndoManagerVms= new ObservableCollection<UndoManagerVm>(Data.UndoManager.UndoManagers.Select(x=>new UndoManagerVm(x.Key,x.Value)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public Dto Dto1 { get; private set; }
        public Dto Dto2 { get; private set; }
        public Dto Dto3 { get; private set; }
        public Dto Dto4 { get; private set; }

        public ObservableCollection<UndoManagerVm> UndoManagerVms { get; private set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
