namespace UndoRedoDemo
{
    using System.Windows.Data;
    using UndoRedo;

    public class UndoManagerVm
    {
        public UndoManagerVm(string name)
        {
            Name = name;
            Manager = UndoRedo.UndoManager.GetByName(name);

            UndoStack = new CollectionView(Manager.History.UndoStack);
            RedoStack = new CollectionView(Manager.History.RedoStack);
            UndoManager undoManager = UndoManager.GetByName(Name);
            undoManager.OnClear += (sender, args) => Update();
            undoManager.OnRedo += (sender, args) => Update();
            undoManager.OnUndo += (sender, args) => Update();
            undoManager.Changed += (sender, args) => Update();

        }
        public string Name { get; set; }
        public UndoManager Manager { get; set; }
        public CollectionView UndoStack { get; private set; }
        public CollectionView RedoStack { get; private set; }

        private void Update()
        {
            UndoStack.Refresh();
            RedoStack.Refresh();
        }
    }
}
