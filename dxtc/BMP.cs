using System;
using System.Runtime.InteropServices;
using System.IO;

namespace dxtc
{
    public class BMP
    {
        #region Fields

        public BITMAPFILEHEADER fileHeader;

        public BITMAPINFOHEADER infoHeader;

        public BGR[] pixels;

        #endregion


        // https://msdn.microsoft.com/en-us/library/windows/desktop/dd183374(v=vs.85).aspx

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPFILEHEADER
        {
            #region Fields
            // WORD. Signature ("BM")
            public UInt16 bfType;

            // DWORD. File size
            public UInt32 bfSize;

            // WORD.
            public UInt16 bfReserved1;
            public UInt16 bfReserved2;


            // DWORD.
            // The offset, in bytes, from the beginning of the BITMAPFILEHEADER structure to the bitmap bits.
            // offset to start of image data in bytes
            public UInt32 bfOffBits;

            #endregion


            #region Constructor

            public BITMAPFILEHEADER(uint pixelSize)
            {
                this.bfType = 0x4D42;
                this.bfReserved1 = 0;
                this.bfReserved2 = 0;

                // Generate by default BMPs with the BITMAPINFOHEADER
                this.bfOffBits = BITMAPFILEHEADER.size + BITMAPINFOHEADER.size;
                this.bfSize = BITMAPFILEHEADER.size + BITMAPINFOHEADER.size + pixelSize;
            }

            #endregion


            #region Calculate the size of the struct using the marshalling API

            public static uint size
            {
                get
                {
                    // Should be 40
                    return (uint)Marshal.SizeOf(typeof(BITMAPFILEHEADER));
                }
            }

            #endregion
        };


        // https://msdn.microsoft.com/en-us/library/windows/desktop/dd183376(v=vs.85).aspx

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPINFOHEADER
        {
            #region Fields

            // DWORD. The number of bytes required by the structure.
            public UInt32 biSize;

            // LONG; The width of the bitmap, in pixels.
            public Int32 biWidth;

            // LONG.
            // If the value of bV5Height is positive, the bitmap is a bottom-up
            // If bV5Height value is negative, the bitmap is a top-down DIB and its origin is the upper-left
            public Int32 biHeight;

            // WORD. The number of planes for the target device. This value must be 1.
            public UInt16 biPlanes;

            // WORD. The number of bits-per-pixel. This value must be 1, 4, 8, or 24.
            public UInt16 biBitCount;

            // DWORD.
            public CompressionMode biCompression;

            // DWORD. The size, in bytes, of the image. This may be set to zero for BI_RGB bitmaps.
            public UInt32 biSizeImage;

            // LONG. The horizontal resolution, in pixels-per-meter
            public Int32 biXPelsPerMeter;

            // LONG. The vertical resolution, in pixels-per-meter
            public Int32 biYPelsPerMeter;

            // DWORD. The number of color indexes in the color table that are actually used by the bitmap
            public UInt32 biClrUsed;

            // DWORD. The number of color indexes that are required for displaying the bitmap. If this value is zero, all colors are required.
            public UInt32 biClrImportant;

            #endregion


            #region Flags

            public enum CompressionMode : UInt32
            {
                BI_RGB = 0,
                BI_RLE8 = 1,
                BI_RLE4 = 2,
                BI_BITFIELDS = 3,
                BI_JPEG = 4,
                BI_PNG = 5
            }

            #endregion


            #region constructor

            public BITMAPINFOHEADER(Int32 width, Int32 height)
            {
                this.biSize = size;
                this.biPlanes = 1;
                this.biBitCount = 24;
                this.biCompression = CompressionMode.BI_RGB;
                this.biSizeImage = 0;
                this.biWidth = width;
                this.biHeight = height;
                this.biXPelsPerMeter = 120;
                this.biYPelsPerMeter = 120;
                this.biClrUsed = 0;
                this.biClrImportant = 0;
            }

            #endregion


            // Calculate the size of the struct using the marshalling API
            public static uint size
            {
                get
                {
                    // Should be 40
                    return (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
                }
            }
        }

        // BITMAPV5HEADER
        // https://msdn.microsoft.com/en-us/library/windows/desktop/dd183381(v=vs.85).aspx

        //  It is an extended version of the BITMAPINFOHEADER
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPV5HEADER {
            // DWORD
            public UInt32        bV5RedMask;

            // DWORD
            public UInt32        bV5GreenMask;

            // DWORD
            public UInt32        bV5BlueMask;

            // DWORD
            public UInt32        bV5AlphaMask;

            // DWORD
            public UInt32        bV5CSType;

            public CIEXYZTRIPLE bV5Endpoints;

            // DWORD
            public UInt32        bV5GammaRed;

            // DWORD
            public UInt32        bV5GammaGreen;

            // DWORD
            public UInt32        bV5GammaBlue;

            // DWORD
            public UInt32        bV5Intent;

            // DWORD
            public UInt32        bV5ProfileData;

            // DWORD
            public UInt32        bV5ProfileSize;

            // DWORD
            public UInt32        bV5Reserved;

            // Calculate the size of the struct using the marshalling API
            public static uint size
            {
                get
                {
                    // Should be 40
                    return (uint)Marshal.SizeOf(typeof(BITMAPV5HEADER));
                }
            }
        };

        // CIEXYZTRIPLE
        // https://msdn.microsoft.com/en-us/library/windows/desktop/dd371833(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CIEXYZTRIPLE {
            public CIEXYZ ciexyzRed;
            public CIEXYZ ciexyzGreen;
            public CIEXYZ ciexyzBlue;
        }

        // CIEXYZ
        // https://msdn.microsoft.com/en-us/library/windows/desktop/dd371828(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CIEXYZ {
            public Int32 ciexyzX;
            public Int32 ciexyzY;
            public Int32 ciexyzZ;
        }

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

        private uint width
        {
            get
            {
                return (uint)infoHeader.biWidth;
            }
        }

        private int height
        {
            get
            {
                return infoHeader.biHeight;
            }
        }

        private BGR this[int x, int y]
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
            
        public uint bitPerPixel
        {
            get
            {
                // Hardcoded to 24
                return 24;
            }
        }

        // https://en.wikipedia.org/wiki/BMP_file_format#Pixel_storage

        public uint rowSize
        {
            get
            {
                return (bitPerPixel * width + 31) / 4;
            }
        }

        public uint pixelArraySize
        {
            get
            {
                return rowSize * (uint)Math.Abs(height);
            }
        }

        public static implicit operator BMP(Image image)
        {
            return new BMP
            {
                fileHeader = new BITMAPFILEHEADER(),
                infoHeader = new BITMAPINFOHEADER(),
                //pixels = image.pixels,
            };

//            var image = new BMP();
//
//            image.fileHeader = new BITMAPFILEHEADER(image.width * image.height * 24 / 4);
//
//            infoHeader = new BITMAPINFOHEADER(image.width, image.height);
//
//            pixels = new BGR[image.width * image.height];
        }

        #endregion


        #region parsing

        public static BMP read(Stream stream)
        {
            var image = new BMP();

            int readStream = 0;

            // Read headers
            readStream += stream.ReadStruct(out image.fileHeader);

            readStream += stream.ReadStruct(out image.infoHeader);

            // Ensure we jump to the offset

            int seek = (int)image.fileHeader.bfOffBits - readStream;

            stream.Seek(seek, SeekOrigin.Current);

            // Initialize pixel matrix
            image.pixels = new BGR[image.height * image.width];

            readStream = 0;

            for(var i = 0; i < image.infoHeader.biHeight; i++)
            {
                for(var j = 0; j < image.infoHeader.biWidth; j++)
                {
                    BGR color;

                    readStream += stream.ReadStruct(out color);
                }

                // TODO padding
            }

            return image;
        }

        public void write(Stream stream)
        {
        }

        #endregion
    }
};
