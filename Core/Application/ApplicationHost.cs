using Chronos.Core.Context;
using Chronos.Core.Controller;
using Chronos.Core.Service;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Application
{
    
    /// <summary>
    /// The ApplicationHost class extends GameWindow to
    /// support our own engine architecture.
    /// </summary>
    public class ApplicationHost : GameWindow
    {


        private IController controller;

        private IContext context;

        private Input input;

        private Clock clock;

        private Display display;

        /// <summary>
        /// Start an ApplicationHost with the specified controller
        /// and configuration.
        /// </summary>
        /// <param name="controller">The game logic controller.</param>
        /// <param name="configuration">The game configuration.</param>
        public static void Start(
            IController controller,
            ApplicationConfiguration configuration)
        {
            using ( ApplicationHost host = new ApplicationHost(
                controller,
                configuration))
            {
                host.Run(100.0);
            }
        }

        /// <summary>
        /// Create an instance of the ApplicationHost class.
        /// </summary>
        /// <param name="controller">The game logic controller.</param>
        /// <param name="configuration">The game configuration.</param>
        private ApplicationHost(
            IController controller,
            ApplicationConfiguration configuration)
        : base(configuration.Width, configuration.Height, configuration.GraphicsMode, configuration.Title)
        {
           
            this.controller = controller;
            this.context = new StandardContext();

            input = new Input(this, Keyboard, Mouse);
            context.AddService(input);

            clock = new Clock();
            context.AddService(clock);

            display = new Display(this);
            context.AddService(display);

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            context.Initialise();
            controller.Initialise(context);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            display.Render();
            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            display.Resize();
        }

    }

}
