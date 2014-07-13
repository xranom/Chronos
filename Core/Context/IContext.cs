using Chronos.Core.Component;
using Chronos.Core.Entity;
using Chronos.Core.Resource;
using Chronos.Core.Service;
using Chronos.Core.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Context
{

    /// <summary>
    /// Represents the context (data) of a game instance.
    /// </summary>
    public interface IContext
    {

        EntityManager EntityManager { get; }

        ComponentManager ComponentManager { get; }

        SystemManager SystemManager { get; }

        ResourceManager ResourceManager { get; }

        ServiceManager ServiceManager { get; }

        void Initialise();

        void Reset();

        #region Helper Methods

        IEntity CreateEntity();

        void DestroyEntity(IEntity entity);

        void AddSystem(ISystem system);

        void RemoveSystem(ISystem system);

        T GetSystem<T>() where T : ISystem;

        T GetService<T>() where T : IService;

        #endregion
        
        #region Serialization

        void Save(string filename);

        void Load(string filename);

        #endregion

    }

}
