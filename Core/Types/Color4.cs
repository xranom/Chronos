using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Drawing = System.Drawing;
using System.Threading.Tasks;

namespace Chronos.Core.Types
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Color4
    {

        public static Color4 Black = new Color4()
        {
            A = 1.0f
        };

        public static Color4 White = new Color4()
        {
            R = 1.0f,
            G = 1.0f,
            B = 1.0f,
            A = 1.0f
        };

        public static Color4 None = new Color4()
        {
            R = 0.0f,
            G = 0.0f,
            B = 0.0f,
            A = 0.0f
        };

        public float R;

        public float G;

        public float B;

        public float A;

        public Color4(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
            A = 1.0f;
        }

        public Color4(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static explicit operator Vector3(Color4 color)
        {
            return new Vector3(color.R, color.G, color.B);
        }

        public static implicit operator Vector4(Color4 color)
        {
            return new Vector4(color.R, color.G, color.B, color.A);
        }

        public static implicit operator Drawing.Color(Color4 color)
        {
            return Drawing.Color.FromArgb(
                255,
                (int)(color.R * 255),
                (int)(color.G * 255),
                (int)(color.B * 255));
        }

    }
}
