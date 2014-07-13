using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Entity
{
    public class EntityEventArgs : EventArgs
    {

        public IEntity Entity { get; private set; }

        public EntityEventArgs(IEntity entity)
        {
            Entity = entity;
        }

    }
}
