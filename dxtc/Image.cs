using System;

namespace dxtc
{
    public class Image
    {
        public uint height;
        public uint width;
        public Color[] pixels;

        /// <summary>
        /// Creates new image without initializing the color matrix
        /// </summary>
        /// 
        /// <param name="height">Height.</param>
        /// <param name="width">Width.</param>
        public Image(uint width, uint height)
        {
            this.height = height;
            this.width = width;
            this.pixels = new Color[width * height];
        }

        /// <summary>
        /// Creates new image with a uniform color
        /// </summary>
        /// 
        /// <param name="height">Height.</param>
        /// <param name="width">Width.</param>
        /// <param name="color">Color.</param>
        public Image(uint width, uint height, Color color) : this(width, height)
        {
            for (var i = 0; i < width * height; i++)
            {
                pixels[i] = color;
            }
        }

        /// <summary>
        /// Creates new image with an array of colors
        /// </summary>
        /// 
        /// <param name="height">Height.</param>
        /// <param name="width">Width.</param>
        /// <param name="colors">Color array.</param>
        public Image(uint width, uint height, Color[] colors)
        {
            this.height = height;
            this.width = width;
            this.pixels = colors;
        }

        /// <summary>
        /// Color.
        /// </summary>
        public struct Color
        {
            public byte r;
            public byte g;
            public byte b;

            public Color(byte r, byte g, byte b)
            {
                this.r = r;
                this.g = g;
                this.b = b;
            }

            public Color(int r, int g, int b) : this(clamp(r), clamp(g), clamp(b))
            {
            }

            public Color(uint r, uint g, uint b) : this(clamp(r), clamp(g), clamp(b))
            {
            }

            public Color(double r, double g, double b) : this(clamp(r), clamp(g), clamp(b))
            {
            }

            #region Operators

            public static Color operator+(Color c1, Color c2)
            {
                return new Color(
                    c1.r + c2.r, 
                    c1.g + c2.g, 
                    c1.b + c2.b);
            }

            public static Color operator-(Color c1, Color c2)
            {
                return new Color(
                    c1.r - c2.r, 
                    c1.g - c2.g, 
                    c1.b - c2.b);
            }

            public static Color operator*(Color c, float number)
            {
                return new Color(
                    (int)(c.r * number),
                    (int)(c.g * number),
                    (int)(c.b * number));
            }

            public static Color operator/(Color c, float number)
            {
                return c * (1f / number);
            }

            public static bool operator<(Color c1, Color c2)
            {
                return (c1.r + c1.g + c1.b) < (c2.r + c2.g + c2.b);
            }

            public static bool operator>(Color c1, Color c2)
            {
                return (c1.r + c1.g + c1.b) > (c2.r + c2.g + c2.b);
            }

            /// <summary>
            /// Manhattan Distance to another color.
            /// </summary>
            /// <param name="color">Color.</param>
            public int distance(Color color)
            {
                var rd = Math.Abs(this.r - color.r);
                var bd = Math.Abs(this.b - color.b);
                var gd = Math.Abs(this.g - color.g);

                return rd + gd + bd;
            }

            #endregion


            /// <summary>
            /// Clamp the specified value to avoid exceptions by overflows.
            /// </summary>
            /// <param name="value">Value.</param>
            private static byte clamp(int value)
            {
                if (value < 0)
                    return 0;

                if (value > 255)
                    return 255;

                return (byte)value;
            }

            /// <summary>
            /// Clamp the specified value to avoid exceptions by overflows.
            /// </summary>
            /// <param name="value">Value.</param>
            private static byte clamp(uint value)
            {
                if (value > 255)
                    return 255;

                return (byte)value;
            }

            /// <summary>
            /// Clamp the specified value to avoid exceptions by overflows.
            /// </summary>
            /// <param name="value">Value.</param>
            private static byte clamp(double value)
            {
                if (value < 0f)
                    return 0;

                if (value > 255f)
                    return 255;

                return (byte)Math.Round(value);
            }

            #region Some default values

            public static Color Red
            {
                get
                {
                    return new Color(255, 0, 0);
                }
            }

            public static Color Black
            {
                get
                {
                    return new Color(0, 0, 0);
                }
            }

            public static Color White
            {
                get
                {
                    return new Color(255, 255, 255);
                }
            }

            #endregion
        }

        /// <summary>
        /// Gets or sets the <see cref="dxtc.Image"/> with the specified x y.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Color this[uint x, uint y]
        {
            set
            {
                pixels[y * width + x] = value;
            }

            get
            {
                return pixels[y * width + x];
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="dxtc.Image"/> with the specified i.
        /// </summary>
        /// <param name="i">The index.</param>
        public Color this[uint i]
        {
            set
            {
                pixels[i] = value;
            }

            get
            {
                return pixels[i];
            }
        }
    }
}
