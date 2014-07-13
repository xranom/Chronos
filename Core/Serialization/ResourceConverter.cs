using Chronos.Core.Resource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Serialization
{
    public class ResourceConverter : JsonConverter
    {

        private ResourceManager resourceManager;

        private MethodInfo getResource;

        public ResourceConverter(ResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
            getResource = typeof(ResourceManager).GetMethod("GetResource");
        }

        public override bool CanConvert(Type objectType)
        {
            if (typeof(IResource).IsAssignableFrom(objectType))
            {
                IResource resource = (IResource)objectType;
                if (resource.Source == ResourceSource.Static)
                {
                    return true;
                }
            }

            return false;

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            ResourceHandle handle = serializer.Deserialize<ResourceHandle>(reader);

            switch (handle.Source)
            {
                case ResourceSource.Static:
                    Console.WriteLine("Binding resource {0}", handle.Name);
                    MethodInfo getGenericResource = getResource.MakeGenericMethod(handle.Type);
                    return getGenericResource.Invoke(resourceManager, new object[] { handle.Name });

                case ResourceSource.Generated:
                    return Activator.CreateInstance(handle.Type);

                default:
                    throw new ArgumentException("Unexepcted resource source");

            }

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            IResource resource = (IResource)value;

            // Need to force serialisation of Data without this converter!

            ResourceHandle handle = new ResourceHandle()
            {
                Name = resource.Name,
                Source = resource.Source,
                Type = value.GetType()
            };

            serializer.Serialize(writer, handle);

        }

        [DataContract]
        private class ResourceHandle
        {

            [DataMember]
            public string Name { get; set; }

            [DataMember]
            public ResourceSource Source { get; set; }

            [DataMember]
            public Type Type { get; set; }

        }

    }

}
