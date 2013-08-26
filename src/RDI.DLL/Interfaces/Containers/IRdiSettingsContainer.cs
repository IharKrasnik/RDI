using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDI.Interfaces.Containers
{
    public interface IRdiSettingsContainer
    {
        Type InterfaceType { get; set; }
        Type ResolvedType { get; set; }

        IRdiBindingSettings BindingSettings { get; }

        /// <summary>
        /// Set ResolvedType with T type value and bind  it with InterfaceType
        /// </summary>
        /// <typeparam name="T">resolve type</typeparam>
        /// <returns>settings</returns>
        IRdiBindingSettings With<T>();  
    }
}
