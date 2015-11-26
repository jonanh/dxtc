using System;

namespace dxtc
{
    public class Image
    {
        public uint height;
        public uint width;
        public Color[] pixels;

        /// <summary>
        /// Creates new image with a uniform color
        /// </summary>
        /// 
        /// <param name="height">Height.</param>
        /// <param name="width">Width.</param>
        /// <param name="color">Color.</param>
        public Image(uint width, uint height) : this(height, width)
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
        }


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
