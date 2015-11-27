using System;
using System.Runtime.InteropServices;

namespace dxtc.BMP
{
    // BITMAPV5HEADER
    // https://msdn.microsoft.com/en-us/library/windows/desktop/dd183381(v=vs.85).aspx

    //  It is an extended version of the BITMAPINFOHEADER
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct BITMAPV5HEADER {
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

        public CIEXYZTRIPLE  bV5Endpoints;

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
    }
}
