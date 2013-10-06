using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDI.Interfaces.Containers
{
    public interface IGeneralRdiContainer
    {
        /// <summary>
        /// Setting type to resolve
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <returns>config for interface type</returns>
        IRdiSettingsContainer Resolve<T>();

        /// <summary>
        /// Resolve type from DI-settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="injectName"></param>
        /// <returns>resolved object</returns>
        T Get<T>(string injectName = null);
    }
}
