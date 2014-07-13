using Chronos.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Entity
{

    public delegate bool RelevantDelegate(IEntity entity);

    public class EntitySet
    {

        public event EventHandler<EntityEventArgs> EntityAdded;

        public event EventHandler<EntityEventArgs> EntityRemoved;

        private RelevantDelegate isRelevant;

        public IReadOnlyList<IEntity> EntityKeys
        {
            get { return entities.AsReadOnly(); }
        }

        private List<IEntity> entities = new List<IEntity>();

        public EntitySet(IContext context, RelevantDelegate isRelevant)
        {
            this.isRelevant = isRelevant;
            context.EntityManager.EntityCreated += EntityCreated;
            context.EntityManager.EntityUpdated += EntityUpdated;
            context.EntityManager.EntityDestroyed += EntityDestroyed;
        }

        private void EntityCreated(object sender, EntityEventArgs e)
        {
            if (isRelevant(e.Entity))
            {
                entities.Add(e.Entity);
                var added = EntityAdded;
                if (added != null)
                {
                    added(this, e);
                }
            }
        }

        private void EntityUpdated(object sender, EntityEventArgs e)
        {
            if (!isRelevant(e.Entity) && entities.Contains(e.Entity))
            {
                entities.Remove(e.Entity);
                var removed = EntityRemoved;
                if (removed != null)
                {
                    removed(this, e);
                }
            }
        }

        private void EntityDestroyed(object sender, EntityEventArgs e)
        {
            if (!isRelevant(e.Entity))
            {
                entities.Remove(e.Entity);
                var removed = EntityRemoved;
                if (removed != null)
                {
                    removed(this, e);
                }
            }
        }

    }

}
