namespace UndoRedo
{
    using System.Windows.Data;
    using Data;

    public class UndoManagerVm
    {
        public UndoManagerVm(string name, UndoManager manager)
        {
            Name = name;
            Manager = manager;

            UndoStack = new CollectionView(Manager.History.UndoStack);
            RedoStack = new CollectionView(Manager.History.RedoStack);
            manager.History.PropertyChanged += (sender, args) =>
            {
                UndoStack.Refresh();
                RedoStack.Refresh();
            };
        }
        public string Name { get; set; }
        public UndoManager Manager { get; set; }
        public CollectionView UndoStack { get; private set; }
        public CollectionView RedoStack { get; private set; }
    }
}
