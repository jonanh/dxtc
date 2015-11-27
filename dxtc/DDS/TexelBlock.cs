using System;

namespace dxtc
{
    public class TexelBlock
    {
        protected Image.Color[] pixels = new Image.Color[16];

        public TexelBlock()
        {
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

