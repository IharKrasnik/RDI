using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDI.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor|AttributeTargets.Property|AttributeTargets.Parameter)]
    public class RDInjectAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
