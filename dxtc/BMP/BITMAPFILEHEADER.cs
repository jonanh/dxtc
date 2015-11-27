using System;
using System.Runtime.InteropServices;

namespace dxtc.BMP
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/dd183374(v=vs.85).aspx

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct BITMAPFILEHEADER
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
    }
}
