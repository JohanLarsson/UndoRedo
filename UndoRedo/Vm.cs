namespace UndoRedo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;

    public class Vm 
    {
        public Vm()
        {
            Dto1 = new Dto();
            Dto2 = new Dto();
            Dto3 = new Dto();
        }
        public Dto Dto1 { get; private set; }
        public Dto Dto2 { get; private set; }
        public Dto Dto3 { get; private set; }
    }
}
