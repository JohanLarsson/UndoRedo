using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndoRedoTests
{
    using System.Reflection;
    using NUnit.Framework;

    public class Class1
    {
        [Test]
        public void TestNameTest()
        {
            PropertyInfo[] propertyInfos = typeof (B).GetProperties();

        }
    }

    public class A
    {
        public int  AProp { get; set; }
    }

    public class B : A
    {
        
    }
}
