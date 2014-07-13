using Chronos.Core.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Component
{
    
    public class Person : IComponent
    {

        public string Name { get; set; }

        public int Age { get; set; }

    }

}
