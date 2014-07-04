using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Application
{
    public class ApplicationConfiguration
    {

        /// <summary>
        /// The default application configuration values.
        /// </summary>
        public static ApplicationConfiguration Default = new ApplicationConfiguration()
        {
            Width = 800,
            Height = 600,
            GraphicsMode = new GraphicsMode(new ColorFormat(32), 24, 8, 4),
            Title = "Chronos Game"
        };

        public int Width { get; set; }

        public int Height { get; set; }

        public GraphicsMode GraphicsMode { get; set; }

        public string Title { get; set; }

    }

}
