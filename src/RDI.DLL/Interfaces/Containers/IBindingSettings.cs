using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDI.Interfaces.Containers1
{
    public interface IRdiBindingSettings
    {
        Dictionary<string, object> ConstructorParameters { get; }

        /// <summary>
        /// Used when there are multiple resolve settings for one interface. Controllerd by RDInjectAttribute.Name
        /// </summary>
        string InjectName { get; }

        IRdiBindingSettings WithConstructorParameters(Dictionary<string,object> parameters);

        IRdiBindingSettings AddConstructorParameter(string paramName, object value);
        IRdiBindingSettings WhenName(string name);     
    }
}
