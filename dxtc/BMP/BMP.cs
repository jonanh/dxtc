using System;
using System.Runtime.InteropServices;

namespace dxtc.BMP
{
    public sealed partial class BMP
    {
        #region Fields

        internal BITMAPFILEHEADER fileHeader;

        internal BITMAPINFOHEADER infoHeader;

        // Hardcoded pixel array to R8G8B8
        internal BGR[] pixels;

        #endregion

        private BMP()
        {
        }

        public BMP(uint width, int height, uint bitPerPixel = 24)
        {
            fileHeader = new BITMAPFILEHEADER(pixelArraySize(width, height, bitPerPixel));
            infoHeader = new BITMAPINFOHEADER((int)width, height, bitPerPixel);
            pixels = new BGR[width * Math.Abs(height)];
        }

        // Internal pixel representation
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BGR
        {
            public byte b;
            public byte g;
            public byte r;

            public static implicit operator Image.Color(BGR color)
            {
                return new Image.Color
                {
                    r = color.r,
                    g = color.g,
                    b = color.b,
                };
            }

            public static implicit operator BGR(Image.Color color)
            {
                return new BGR
                {
                    r = color.r,
                    g = color.g,
                    b = color.b,
                };
            }
        }

        #region utils

        /// <summary>
        /// Gets the width from the BMP header.
        /// </summary>
        /// <value>The width.</value>
        public uint width
        {
            get
            {
                return (uint)infoHeader.biWidth;
            }
        }

        /// <summary>
        /// Gets the height from the BMP header.
        /// </summary>
        /// <value>The height.</value>
        public int height
        {
            get
            {
                return infoHeader.biHeight;
            }
        }

        /// <summary>
        /// Gets the unsigned height from the BMP header.
        /// </summary>
        /// <value>The height.</value>
        public uint uheight
        {
            get
            {
                return (uint)Math.Abs(height);
            }
        }

        /// <summary>
        /// Gets the bit per pixel from the BMP header.
        /// </summary>
        /// <value>The bit per pixel.</value>
        public uint bitPerPixel
        {
            get
            {
                return infoHeader.biBitCount;
            }
        }

        public BGR this[uint x, uint y]
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

        public BGR this[uint i]
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

        public uint padding
        {
            get
            {
                return rowSize(bitPerPixel, width) - (bitPerPixel * width / 8);
            }
        }

        // https://en.wikipedia.org/wiki/BMP_file_format#Pixel_storage

        public uint rowSize(uint bitPerPixel, uint width)
        {
            return ((bitPerPixel * width + 31) / 32) * 4;
        }

        public uint pixelArraySize(uint width, int height, uint bitPerPixel)
        {
            return rowSize(bitPerPixel, width) * (uint)Math.Abs(height);
        }

        public static implicit operator BMP(Image image)
        {
            var bmp = new BMP(image.width, -(int)image.height);
            var size = image.height * image.width;

            for(uint i = 0; i < size; i++)
            {
                bmp[i] = image[i];
            }

            return bmp;
        }

        public static implicit operator Image(BMP bmp)
        {
            var image = new Image(bmp.width, bmp.uheight);

            // Bottom up
            if (bmp.height < 0)
            {
                for(uint i = 0; i < image.height * image.width; i++)
                {
                    image[i] = bmp[i];
                }
            }
            else
            {
                // Index to reverse the matrix from bottom to up
                uint index = (image.height - 1) * image.width;
                uint ii = 0;

                for(uint i = 0; i < image.height; i++)
                {
                    for(uint j = 0; j < image.width; j++, ii++)
                    {
                        image[index + j] = bmp[ii];
                    }

                    index -= image.width;
                }
            }

            return image;
        }

        #endregion
    }
};
