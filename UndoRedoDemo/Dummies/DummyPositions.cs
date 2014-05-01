namespace UndoRedoDemo.Dummies
{
    using UndoRedo;

    public class DummyPositions
    {
        public DummyPositions(int i, string name)
        {
            Name = name;
            StartValues = new DummyValues(i);
            EndValues = new DummyValues(i + 1);
            UndoManager = new UndoManagerVm(name);
        }

        public string Name { get; private set; }
        public DummyValues StartValues { get; private set; }
        public DummyValues EndValues { get; private set; }
        public UndoManagerVm UndoManager { get; private set; }
    }
}
