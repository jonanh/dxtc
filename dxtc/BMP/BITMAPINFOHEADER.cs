using System;
using System.Runtime.InteropServices;

namespace dxtc.BMP
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/dd183376(v=vs.85).aspx

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct BITMAPINFOHEADER
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

        public enum CompressionMode : uint
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

        public BITMAPINFOHEADER(int width, int height, uint bitPerPixel)
        {
            this.biSize = size;
            this.biPlanes = 1;
            this.biBitCount = (UInt16)bitPerPixel;
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
}

