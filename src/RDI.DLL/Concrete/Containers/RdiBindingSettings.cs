using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDI.Interfaces.Containers;

namespace RDI.Concrete.Containers
{
    public class RdiBindingSettings : IRdiBindingSettings
    {
        public Dictionary<string, object> ConstructorParameters { get; protected set; }
        public string InjectName { get; protected set; }

        public RdiBindingSettings()
        {
            ConstructorParameters = new Dictionary<string, object>();
        }

        public IRdiBindingSettings WithConstructorParameters(Dictionary<string, object> parameters)
        {
            ConstructorParameters = parameters;
            return this;
        }

        public IRdiBindingSettings AddConstructorParameter(string paramName, object value)
        {
            ConstructorParameters.Add(paramName, value);
            return this;
        }

        public IRdiBindingSettings WhenName(string name)
        {
            InjectName = name;
            return this;
        }
    }
}
