using System;

namespace dxtc
{
    public class Image
    {
        public int height;
        public int width;
        public Color[] pixels;

        /// <summary>
        /// Creates new image with a uniform color
        /// </summary>
        /// 
        /// <param name="height">Height.</param>
        /// <param name="width">Width.</param>
        /// <param name="color">Color.</param>
        public Image(int height, int width, Color color)
        {
            this.height = height;
            this.width = width;
            this.pixels = new Color[width * height];

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
        public Image(int height, int width, Color[] colors)
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
        }
    }
}
