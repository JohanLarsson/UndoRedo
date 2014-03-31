namespace UndoRedo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DummyPositions
    {
        public DummyPositions(int i)
        {
            StartValues = new DummyValues(i);
            EndValues = new DummyValues(i+1);
        }
        public DummyValues StartValues { get; private set; }
        public DummyValues EndValues { get; private set; }
    }
}
