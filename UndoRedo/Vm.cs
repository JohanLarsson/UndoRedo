namespace UndoRedo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class Vm : INotifyPropertyChanged
    {
        public Vm()
        {
            Dto1 = new Dto();
            Dto2 = new Dto();
            Dto3 = new Dto();
            Dto4 = new Dto();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public Dto Dto1 { get; private set; }
        public Dto Dto2 { get; private set; }
        public Dto Dto3 { get; private set; }
        public Dto Dto4 { get; private set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
