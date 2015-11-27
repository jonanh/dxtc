using System;
using System.Runtime.InteropServices;

namespace dxtc.DDS
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/bb943984(v=vs.85).aspx

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DDS_PIXELFORMAT
    {
        #region Fields

        // DWORD. Structure size; set to 32 (bytes).
        public UInt32 dwSize;

        public Flags dwFlags;

        public FOURCC dwFourCC;

        // Number of bits in an RGB (possibly including alpha) format. Valid when dwFlags includes DDPF_RGB, DDPF_LUMINANCE, or DDPF_YUV.
        public UInt32 dwRGBBitCount;

        // Red (or lumiannce or Y) mask for reading color data. For instance, given the A8R8G8B8 format, the red mask would be 0x00ff0000.
        public UInt32 dwRBitMask;

        // Green (or U) mask for reading color data. For instance, given the A8R8G8B8 format, the green mask would be 0x0000ff00.
        public UInt32 dwGBitMask;

        // Blue (or V) mask for reading color data. For instance, given the A8R8G8B8 format, the blue mask would be 0x000000ff.
        public UInt32 dwBBitMask;

        public UInt32 dwABitMask;

        #endregion


        #region Constructors

        public static DDS_PIXELFORMAT D3FMT_R5G6B5_Format
        {
            get
            {
                return new DDS_PIXELFORMAT
                {
                    dwSize = DDS_PIXELFORMAT.size,
                    dwFlags = Flags.DDPF_RGB,
                    dwFourCC = FOURCC.DXT1,
                    dwRGBBitCount = 16,
                    dwRBitMask = ColorR5G6B5.RMask,
                    dwGBitMask = ColorR5G6B5.GMask,
                    dwBBitMask = ColorR5G6B5.BMask,
                    dwABitMask = 0,
                };
            }
        }

        #endregion


        #region Flags

        [Flags]
        public enum Flags : uint
        {
            // Texture contains alpha data; dwRGBAlphaBitMask contains valid data.
            DDPF_ALPHAPIXELS = 0x1,

            // Used in some older DDS files for alpha channel only uncompressed data (dwRGBBitCount contains the alpha channel bitcount; dwABitMask contains valid data)
            DDPF_ALPHA = 0x2,

            // Texture contains compressed RGB data; dwFourCC contains valid data.
            DDPF_FOURCC = 0x4,

            // Texture contains uncompressed RGB data; dwRGBBitCount and the RGB masks (dwRBitMask, dwGBitMask, dwBBitMask) contain valid data.
            DDPF_RGB = 0x40,

            // Used in some older DDS files for YUV uncompressed data (dwRGBBitCount contains the YUV bit count; dwRBitMask contains the Y mask, dwGBitMask contains the U mask, dwBBitMask contains the V mask)
            DDPF_YUV = 0x200,

            // Used in some older DDS files for single channel color uncompressed data (dwRGBBitCount contains the luminance channel bit count; dwRBitMask contains the channel mask). Can be combined with DDPF_ALPHAPIXELS for a two channel DDS file.
            DDPF_LUMINANCE = 0x20000,

            DEFAULT = DDPF_FOURCC | DDPF_RGB,
        }

        public enum FOURCC
        {
            DXT1 = 827611204,
        }

        // MakeFourCC allows to generate the value the FourCCFlags
        //
        // R8G8B8
        // https://msdn.microsoft.com/en-us/library/windows/desktop/bb153349(v=vs.85).aspx
        //
        // D3DFORMAT
        // https://msdn.microsoft.com/en-us/library/windows/desktop/bb172558(v=vs.85).aspx
        //
        // public static int MakeFourCC(int ch0, int ch1, int ch2, int ch3)
        // {
        //    return ((int)(byte)(ch0) | ((int)(byte)(ch1) << 8) | ((int)(byte)(ch2) << 16) | ((int)(byte)(ch3) << 24));
        // }

        #endregion


        #region Testing the size of the struct

        public static uint size
        {
            get
            {
                // should be 32
                return (uint)Marshal.SizeOf(typeof(DDS_PIXELFORMAT));
            }
        }

        #endregion
    };
}

