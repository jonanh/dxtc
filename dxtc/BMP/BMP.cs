using System;
using System.Runtime.InteropServices;

namespace dxtc
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
            infoHeader = new BITMAPINFOHEADER((int)width, (int)height, bitPerPixel);
            pixels = new BGR[width * height];
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
                return new Image.Color
                {
                    r = color.r,
                    g = color.g,
                    b = color.b,
                };
            }
        }

        #region utils

        public uint width
        {
            get
            {
                return (uint)infoHeader.biWidth;
            }
        }

        public int height
        {
            get
            {
                return infoHeader.biHeight;
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

        public uint bitPerPixel
        {
            get
            {
                return infoHeader.biBitCount;
            }
        }

        public uint padding
        {
            get
            {
                return ((bitPerPixel * width) % 32) / 8;
            }
        }

        // https://en.wikipedia.org/wiki/BMP_file_format#Pixel_storage

        public uint rowSize(uint bitPerPixel, uint width)
        {
            return (bitPerPixel * width + 31) / 4;
        }

        public uint pixelArraySize(uint width, int height, uint bitPerPixel)
        {
            return rowSize(bitPerPixel, width) * (uint)Math.Abs(height);
        }

        public static implicit operator BMP(Image image)
        {
            var bmp = new BMP(image.width, -(int)image.height);

            for(uint i = 0; i < image.height * image.width; i++)
            {
                bmp[i] = image[i];
            }

            return bmp;
        }

        public static implicit operator Image(BMP bmp)
        {
            var image = new Image(bmp.width, (uint)Math.Abs(bmp.height));

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
                uint index = (image.height - 1) * image.width;
                for(uint i = 0; i < image.height; i++)
                {
                    for(uint j = 0; j < image.width; j++)
                    {
                        image[index + j] = bmp[i];
                    }

                    index -= image.width;
                }
            }

            return image;
        }

        #endregion
    }
};
