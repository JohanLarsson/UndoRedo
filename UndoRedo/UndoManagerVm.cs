namespace UndoRedo
{
    using View;

    public class UndoManagerVm
    {
        public UndoManagerVm(string name, UndoManager manager)
        {
            Name = name;
            Manager = manager;
        }
        public string Name { get; set; }
        public UndoManager Manager { get; set; }
    }
}
