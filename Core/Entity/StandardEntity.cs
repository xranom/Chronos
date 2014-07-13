using Chronos.Core.Component;
using Chronos.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Entity
{
    public class StandardEntity : IEntity
    {

        public event EventHandler<EntityEventArgs> ComponentsChanged;

        public int Id { get; private set; }

        private IContext context;

        public StandardEntity(IContext context, int id)
        {
            this.context = context;
            Id = id;
        }

        public bool HasComponent<T>() where T : IComponent
        {
            return context.ComponentManager.HasComponent<T>(Id);
        }

        public T AddComponent<T>() where T : IComponent
        {
            return context.ComponentManager.AddComponent<T>(Id);
        }

        public T GetComponent<T>() where T : IComponent
        {
            return context.ComponentManager.GetComponent<T>(Id);
        }

        public void RemoveComponent<T>() where T : IComponent
        {
            context.ComponentManager.RemoveComponent<T>(Id);
        }

        private void FireComponentsChanged()
        {
            var evt = ComponentsChanged;
            if ( evt != null)
            {
                ComponentsChanged(this, new EntityEventArgs(this));
            }
        }

    }
}
