using RDI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdiTests.RdiContainerTests.TestClasses
{
    class ClassA3 : ClassA
    {
        public ClassA3()
        {
            I = 1;
        }

        [RDInject]
        public ClassA3(IClassB b)
        {
            I = 2;
            B = b;
        }

        
        public ClassA3(int I=113)
        {
            I = 3;
        }
    }
}
