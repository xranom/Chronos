using Chronos.Core.Component;
using Chronos.Core.Context;
using Chronos.Core.Resource;
using Newtonsoft.Json.Serialization;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Serialization
{
    public class GameDataResolver : DefaultContractResolver
    {

        private IContext context;

        public GameDataResolver(IContext context)
        {
            this.context = context;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            JsonContract contract = base.CreateContract(objectType);

            if (objectType == typeof(Vector3))
            {
                contract.Converter = new Vector3Converter();
            }

            if (objectType == typeof(Vector4))
            {
                contract.Converter = new Vector4Converter();
            }

            if (objectType == typeof(Quaternion))
            {
                contract.Converter = new QuaternionConverter();
            }

            if (objectType == typeof(Matrix4))
            {
                contract.Converter = new Matrix4Converter();
            }

            if (typeof(IResource).IsAssignableFrom(objectType))
            {
                contract.Converter = new ResourceConverter(context.ResourceManager);
            }


            if (false && typeof(IComponent).IsAssignableFrom(objectType))
            {
                contract.Converter = new ComponentConverter();
            }

            return contract;

        }

    }
}
