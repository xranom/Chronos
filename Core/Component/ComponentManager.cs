using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Component
{
    [DataContract]
    public class ComponentManager
    {

        public ComponentData data;

        private IList<Type> componentTypes
            = new List<Type>();

        public ComponentManager()
        {
            data = new ComponentData();
            FindComponentTypes();
            CreateComponentMaps();
        }

        public bool HasComponent<T>(int id) where T : IComponent
        {
            var componentMap = (ComponentMap<T>)
                data.ComponentMapByType[typeof(T)];
            return componentMap.ComponentById.ContainsKey(id);
        }

        public T AddComponent<T>(int id) where T : IComponent
        {
            var componentMap = (ComponentMap<T>)
                data.ComponentMapByType[typeof(T)];
            return componentMap.Create(id);
        }

        public T GetComponent<T>(int id) where T : IComponent
        {
            var componentMap = (ComponentMap<T>)
                data.ComponentMapByType[typeof(T)];
            return componentMap.ComponentById[id];
        }

        public void RemoveComponent<T>(int id) where T : IComponent
        {
            var componentMap = (ComponentMap<T>)
                data.ComponentMapByType[typeof(T)];
            componentMap.Destroy(id);
        }

        private void FindComponentTypes()
        {
            Type componentType = typeof(IComponent);
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.IsDynamic)
                {
                    foreach (Type type in assembly.GetExportedTypes())
                    {
                        if (!type.IsInterface && componentType.IsAssignableFrom(type))
                        {
                            componentTypes.Add(type);
                        }
                    }
                }
            }
        }

        private void CreateComponentMaps()
        {
            foreach (Type type in componentTypes)
            {
                Type genericType = typeof(ComponentMap<>).MakeGenericType(type);
                object instance = Activator.CreateInstance(genericType);
                if (!data.ComponentMapByType.ContainsKey(type))
                {
                    data.ComponentMapByType[type] = instance;
                }
            }
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(data);
        }

        public void Deserialize(string saveData)
        {
            data = JsonConvert.DeserializeObject<ComponentData>(saveData);
        }

    }
}
