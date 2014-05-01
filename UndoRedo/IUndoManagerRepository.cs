namespace UndoRedo
{
    public interface IUndoManagerRepository
    {
        IUndoManager GetByName(string undoscopeName);
    }
}