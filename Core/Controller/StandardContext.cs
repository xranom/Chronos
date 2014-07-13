using Chronos.Core.Component;
using Chronos.Core.Context;
using Chronos.Core.Entity;
using Chronos.Core.Resource;
using Chronos.Core.Serialization;
using Chronos.Core.Service;
using Chronos.Core.System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Controller
{

    /// <summary>
    /// A standard implementation of IContext that
    /// supports most types of game.
    /// </summary>
    public class StandardContext : IContext
    {

        public EntityManager EntityManager { get; private set; }

        public ComponentManager ComponentManager { get; private set; }

        public SystemManager SystemManager { get; private set; }

        public ResourceManager ResourceManager { get; private set; }

        public ServiceManager ServiceManager { get; private set; }

        public StandardContext()
        {
            PrepareSerializer();
            Reset();
        }

        public void Initialise()
        {
            EntityManager.Initialise(this);
            ServiceManager.Initialise(this);
            SystemManager.Initialise(this);
        }

        public void Reset()
        {
            EntityManager = new EntityManager();
            ComponentManager = new ComponentManager();
            SystemManager = new SystemManager();
            ResourceManager = new ResourceManager();
            ServiceManager = new ServiceManager();
        }

        #region Helper Methods

        public IEntity CreateEntity()
        {
            return EntityManager.CreateEntity();
        }

        public void DestroyEntity(IEntity entityKey)
        {
            EntityManager.DestroyEntity(entityKey);
        }

        public void AddSystem(ISystem system)
        {
            SystemManager.AddSystem(system);
        }

        public T GetSystem<T>() where T : ISystem
        {
            return SystemManager.GetSystem<T>();
        }

        public void RemoveSystem(ISystem system)
        {
            SystemManager.RemoveSystem(system);
        }

        public void AddService(IService service)
        {
            ServiceManager.AddService(service);
        }

        public T GetService<T>() where T : IService
        {
            return (T)ServiceManager.GetService<T>();
        }

        public void RemoveService<T>()
        {
            ServiceManager.RemoveService<T>();
        }

        #endregion

        #region Serialization

        public void Save(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(ComponentManager.Serialize());
            }
        }

        public void Load(string filename)
        {

            using (StreamReader reader = new StreamReader(filename))
            {
                string data = reader.ReadToEnd();
                ComponentManager.Deserialize(data);
            }

            SystemManager.Initialise(this);

        }

        private void PrepareSerializer()
        {

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented,
                ContractResolver = new GameDataResolver(this)
            };

        }

        #endregion

    }
}
