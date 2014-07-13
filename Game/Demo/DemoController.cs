using Chronos.Core.Context;
using Chronos.Core.Controller;
using Chronos.Core.Entity;
using Game.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Demo
{
    public class DemoController : IController
    {

        public void Initialise(IContext context)
        {

            string[] names = { "Bob", "John" };
            int[] ages = { 20, 35 };

            int count = Math.Max(names.Length, ages.Length);
            for (int i = 0; i < count; ++i)
            {
                IEntity entity = context.CreateEntity();
                Person person = entity.AddComponent<Person>();
                person.Name = names[i];
                person.Age = ages[i];
            }


            context.Save("save.json");

        }

    }
}

