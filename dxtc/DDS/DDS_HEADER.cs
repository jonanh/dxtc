using System;
using System.Runtime.InteropServices;

namespace dxtc.DDS
{
    // https://msdn.microsoft.com/en-us/library/bb943982(v=vs.85).aspx

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DDS_HEADER
    {
        #region Fields

        // DWORD
        public UInt32 dwSize;

        // DWORD
        public Flags dwFlags;

        // DWORD
        public UInt32 dwHeight;

        // DWORD
        public UInt32 dwWidth;

        // DWORD
        public UInt32 dwPitchOrLinearSize;

        // DWORD
        public UInt32 dwDepth;

        // DWORD
        public UInt32 dwMipMapCount;

        // DWORD
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 11)]
        public UInt32[] dwReserved1;


        public DDS_PIXELFORMAT ddspf;

        // Specifies the complexity of the surfaces stored.
        public CAPSFlags dwCaps;

        // Additional detail about the surfaces stored.
        // We won't use it
        public UInt32 dwCaps2;

        // Unused
        public UInt32 dwCaps3;
        public UInt32 dwCaps4;
        public UInt32 dwReserved2;

        #endregion


        #region Constructor

        public static DDS_HEADER CreateDXT1Header(uint width, uint height, bool compressed = true)
        {
            return new DDS_HEADER
            {
                dwSize = DDS_HEADER.size,
                dwWidth = width,
                dwHeight = height,
                dwFlags = compressed ? 
                    Flags.DDS_HEADER_FLAGS_TEXTURE | Flags.DDSD_LINEARSIZE :
                    Flags.DDS_HEADER_FLAGS_TEXTURE | Flags.DDSD_PITCH,
                dwMipMapCount = 0,
                dwReserved1 = new uint[11],
                ddspf = DDS_PIXELFORMAT.D3FMT_R5G6B5_Format,
                dwCaps = CAPSFlags.DDSCAPS_TEXTURE,
                dwCaps2 = 0,
                dwCaps3 = 0,
                dwCaps4 = 0,
                dwReserved2 = 0,
                dwPitchOrLinearSize = width * height / 2,
            };
        }

        #endregion


        #region Flags

        [Flags]
        public enum Flags : uint
        {
            DDSD_CAPS = 0x1,
            DDSD_HEIGHT = 0x2,
            DDSD_WIDTH = 0x4,
            DDSD_PITCH = 0x8,
            DDSD_PIXELFORMAT = 0x1000,
            DDSD_MIPMAPCOUNT = 0x20000,
            DDSD_LINEARSIZE = 0x80000,
            DDSD_DEPTH = 0x800000,

            DDS_HEADER_FLAGS_TEXTURE = DDSD_CAPS | DDSD_HEIGHT | DDSD_WIDTH | DDSD_PIXELFORMAT | DDSD_MIPMAPCOUNT,
        };

        [Flags]
        public enum CAPSFlags : uint
        {
            //Optional; must be used on any file that contains more than one surface (a mipmap, a cubic environment map, or mipmapped volume texture).
            DDSCAPS_COMPLEX = 0x8,

            // Optional; should be used for a mipmap.
            DDSCAPS_MIPMAP = 0x400000,

            // Required
            DDSCAPS_TEXTURE = 0x1000,
        }

        #endregion


        #region Testing the size of the struct

        public static uint size
        {
            get
            {
                // should be 124
                return (uint)Marshal.SizeOf(typeof(DDS_HEADER));
            }
        }

        #endregion
    };
}

