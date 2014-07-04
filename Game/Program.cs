using Chronos.Core.Application;
using Game.Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Program
    {
        public static void Main(string[] args)
        {

            ApplicationHost.Start(
                new DemoController(),
                ApplicationConfiguration.Default);

        }
    }
}
