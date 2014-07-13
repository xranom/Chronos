using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Component
{
    [DataContract]
    public class ComponentMap<T> where T : IComponent
    {

        [DataMember]
        public IDictionary<int, T> ComponentById { get; set; }

        public ComponentMap()
        {
            ComponentById = new Dictionary<int, T>();
        }

        public T Create(int id)
        {
            T component = (T)Activator.CreateInstance<T>();
            ComponentById[id] = component;
            return component;
        }

        public void Destroy(int id)
        {
            ComponentById.Remove(id);
        }

    }
}
