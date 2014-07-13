using Chronos.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Service
{
    public class ServiceManager
    {

        private IDictionary<Type, IService> serviceByType
            = new Dictionary<Type, IService>();

        private IContext context;

        private bool initialised = false;

        public ServiceManager()
        {

        }

        public void Initialise(IContext context)
        {
            this.context = context;
            initialised = true;
            foreach (IService service in serviceByType.Values)
            {
                service.Initialise(context);
            }
        }

        public void AddService(IService service)
        {
            serviceByType[service.GetType()] = service;
            if (initialised)
            {
                service.Initialise(context);
            }
        }

        public T GetService<T>() where T : IService
        {
            return (T)serviceByType[typeof(T)];
        }

        public void RemoveService<T>()
        {
            serviceByType.Remove(typeof(T));
        }

    }
}
