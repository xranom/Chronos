using Chronos.Core.Context;
using Chronos.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Entity
{

    public class EntityManager
    {

        public IReadOnlyList<IEntity> EntityKeys
        {
            get { return entities.AsReadOnly(); }
        }

        public event EventHandler<EntityEventArgs> EntityCreated;

        public event EventHandler<EntityEventArgs> EntityUpdated;

        public event EventHandler<EntityEventArgs> EntityDestroyed;

        private int nextEntityKeyId = 0;

        private List<IEntity> entities = new List<IEntity>();

        private IContext context;

        private IList<IEntity> pendingCreate
            = new List<IEntity>();

        private IList<IEntity> pendingDestroy
            = new List<IEntity>();

        private IList<IEntity> pendingUpdate
            = new List<IEntity>();

        public EntityManager()
        {

        }

        public void Initialise(IContext context)
        {
            this.context = context;
            context.GetService<Clock>().FixedTick += Update;
        }

        public IEntity CreateEntity()
        {
            int id = nextEntityKeyId++;
            IEntity entity = new StandardEntity(context, id);
            pendingCreate.Add(entity);
            return entity;
        }

        public void DestroyEntity(IEntity entity)
        {
            pendingDestroy.Add(entity);
        }

        public void Update(object sender, EventArgs e)
        {

            // Create Entities:
            var created = EntityCreated;
            if (created != null)
            {
                foreach (IEntity entity in pendingCreate)
                {
                    entities.Add(entity);
                    created(this, new EntityEventArgs(entity));
                }
            }
            else
            {
                entities.AddRange(pendingCreate);
            }
            pendingCreate.Clear();

            // Destroy Entities:
            var destroyed = EntityDestroyed;
            if (destroyed != null)
            {
                foreach (IEntity entity in pendingDestroy)
                {
                    entities.Remove(entity);
                    destroyed(this, new EntityEventArgs(entity));
                }
            }
            else
            {
                foreach (IEntity entity in pendingDestroy)
                {
                    entities.Remove(entity);
                }
            }
            pendingDestroy.Clear();

            // Updated Entities:
            var updated = EntityUpdated;
            if (updated != null)
            {
                foreach (IEntity entity in pendingUpdate)
                {
                    updated(this, new EntityEventArgs(entity));
                }
            }
            pendingUpdate.Clear();

        }

        private void ComponentsChanged(object sender, EntityEventArgs e)
        {
            if (entities.Contains(e.Entity))
            {
                pendingUpdate.Add(e.Entity);
            }
        }

    }

}
