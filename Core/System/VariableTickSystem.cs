using Chronos.Core.Context;
using Chronos.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.System
{
    public abstract class VariableTickSystem : ISystem
    {


        protected IContext Context { get; private set; }

        protected Clock Clock { get; private set; }

        protected Input Input { get; private set; }

        public void Initialise(IContext context)
        {
            Context = context;
            Clock = context.GetService<Clock>();
            Input = context.GetService<Input>();
            Clock.VariableTick += Tick;
            Initialise();
        }

        public void Tick(object sender, EventArgs e)
        {
            Update();
        }

        public abstract void Initialise();

        public abstract void Update();

    }
}
