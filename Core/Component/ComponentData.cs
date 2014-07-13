using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Component
{
    [DataContract]
    public class ComponentData
    {

        [DataMember]
        public IDictionary<Type, object> ComponentMapByType { get; set; }

        public ComponentData()
        {
            ComponentMapByType = new Dictionary<Type, object>();
        }

    }
}
