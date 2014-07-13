using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Resource
{
    public interface IResource
    {

        string Name { get; }

        ResourceSource Source { get; }

    }
}
