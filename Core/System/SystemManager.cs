using Chronos.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.System
{
    public class SystemManager
    {

        public IDictionary<Type, ISystem> SystemByType { get; private set; }

        private bool initialised = false;

        private IContext context;

        public SystemManager()
        {
            SystemByType = new Dictionary<Type, ISystem>();
        }

        public void Initialise(IContext context)
        {
            this.context = context;
            initialised = true;
            foreach (ISystem system in SystemByType.Values)
            {
                system.Initialise(context);
            }
        }

        public void AddSystem(ISystem system)
        {
            SystemByType[system.GetType()] = system;
            if (initialised)
            {
                system.Initialise(context);
            }
        }

        public T GetSystem<T>() where T : ISystem
        {
            return (T)SystemByType[typeof(T)];
        }

        public void RemoveSystem(ISystem system)
        {
            SystemByType.Remove(system.GetType());
        }

    }

}
