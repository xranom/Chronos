using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Service
{
    public class MouseEventArgs : EventArgs
    {
        public MouseButton Button { get; private set; }

        public MouseEventArgs(MouseButton button)
        {
            Button = button;
        }
    }
}
