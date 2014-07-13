using Chronos.Core.Context;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Service
{
    public class Display : IService
    {

        public event EventHandler<EventArgs> OnRender;

        public event EventHandler<EventArgs> OnResize;

        public Core.Types.Color4 ClearColor { get; set; }

        public int Width
        {
            get { return window.Width; }
        }

        public int Height
        {
            get { return window.Height; }
        }

        private NativeWindow window;

        public Display(NativeWindow window)
        {
            this.window = window;
            ClearColor = new Core.Types.Color4(0.3f, 0.3f, 0.6f);
        }

        public void Initialise(IContext context)
        {

        }

        public void Render()
        {

            // Clear the buffer:            
            GL.ClearColor(ClearColor.R, ClearColor.G, ClearColor.B, ClearColor.A);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var evt = OnRender;
            if (evt != null)
            {
                evt(this, null);
            }
        }

        public void Resize()
        {
            var evt = OnResize;
            if (evt != null)
            {
                evt(this, null);
            }
        }

    }

}
