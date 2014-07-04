using Chronos.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Controller
{
    public interface IController
    {

        void Initialise(IContext context);

    }
}
