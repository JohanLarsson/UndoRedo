namespace UndoRedo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UndoManagerRepository : IUndoManagerRepository
    {
        public IUndoManager GetByName(string undoscopeName)
        {
            return UndoManager.GetByName(undoscopeName);
        }
    }
}
