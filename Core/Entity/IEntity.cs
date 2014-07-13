using Chronos.Core.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Entity
{
    public interface IEntity
    {

        event EventHandler<EntityEventArgs> ComponentsChanged;

        int Id { get; }

        bool HasComponent<T>() where T : IComponent;

        T AddComponent<T>() where T : IComponent;

        T GetComponent<T>() where T : IComponent;

        void RemoveComponent<T>() where T : IComponent;

    }

}
