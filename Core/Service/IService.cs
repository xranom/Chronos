using Chronos.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Service
{
    public interface IService
    {

        void Initialise(IContext context);

    }
}
