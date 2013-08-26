using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RDI.Interfaces.Containers;

namespace RDI.Concrete.Containers
{
    public class RdiSettingsContainer : IRdiSettingsContainer
    {
        public Type InterfaceType { get; set; }
        public Type ResolvedType { get; set; }
        public IRdiBindingSettings BindingSettings { get; protected set; }

        public RdiSettingsContainer(IRdiBindingSettings settings = null)
        {
            BindingSettings = settings;
        }

        public IRdiBindingSettings With<TConcrete>()
        {
            Type concreteType = typeof(TConcrete);
            if (InterfaceType.IsAssignableFrom(concreteType))
            {
                ResolvedType = concreteType;
                return BindingSettings = new RdiBindingSettings();
            }
            else
            {
                throw new InvalidCastException(String.Format("type {0} is not assignable from type {1}", 
                                               concreteType.ToString(),InterfaceType.ToString()));
            }
        }


    }
}
