using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDI.Attributes;

namespace RdiTests.RdiContainerTests.TestClasses
{
    /// <summary>
    /// Class with only default constructor
    /// </summary>
    class ClassA : IClassA
    {
        public int I { get; set; }
        public IClassB B { get; set; }
        public int K { get; set; }

        [RDInject(Name="B2")]
        public IClassB BInj { get; set; }

        public IClassB BNotInj { get; set; }
    }
}
