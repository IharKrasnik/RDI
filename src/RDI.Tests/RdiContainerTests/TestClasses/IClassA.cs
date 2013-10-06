using RDI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdiTests.RdiContainerTests.TestClasses
{
    public interface IClassA
    {
        int I { get; set; }
        IClassB B { get; set; }
        int K { get; set; }

        [RDInject(Name="B2")]
        IClassB BInj { get; set; }

        IClassB BNotInj { get; set; }
    }
}
