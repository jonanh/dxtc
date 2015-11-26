using System;
using System.Runtime.InteropServices;

namespace dxtc
{
    public static class BMP
    {
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


            // TODO Clean this constructor
            public BITMAPFILEHEADER(uint offset, uint width, uint height, uint size)
            {
                this.bfType = 0x4D42;
                this.bfReserved1 = 0;
                this.bfReserved2 = 0;

                // TODO
                this.bfOffBits = 0;
                this.bfSize = 0;
            }

            // Calculate the size of the struct using the marshalling API
            public static uint size
            {
                get
                {
                    // Should be 40
                    return (uint)Marshal.SizeOf(typeof(BITMAPFILEHEADER));
                }
            }
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
            byte b;
            byte g;
            byte r;
        }
    }
};
