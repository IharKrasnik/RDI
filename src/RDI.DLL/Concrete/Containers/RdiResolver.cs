using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDI.Interfaces.Containers;

namespace RDI.Concrete.Containers
{
    public class RdiResolver
    {
        private static RdiResolver instance;
        public static RdiResolver Instance
        {
            get
            {
                return instance ?? (instance = new RdiResolver());
            }
        }

        protected RdiResolver()
        {
        }

        private  IGeneralRdiContainer rdiContainer = null;

        public  void UseContainer(IGeneralRdiContainer container)
        {
            rdiContainer = container;
        }

        public  T Get<T>()
        {
            if (rdiContainer != null)
            {
                return rdiContainer.Get<T>();
            }
            else
            {
                throw new InvalidOperationException("Container is not initialized");
            }
        }
    }
}
