using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDI.Attributes;
using RdiTests.RdiContainerTests.TestClasses;

namespace RdiTests.RdiContainerTests.TestClasses
{
    /// <summary>
    /// Class with only not default constructor
    /// </summary>
    class ClassA2 : ClassA
    {
        public ClassA2([RDInject(Name="B1")]IClassB b,int k,int i=10)
        {
            I = i;
            B = b;
            K = k;
        }
    }
}
