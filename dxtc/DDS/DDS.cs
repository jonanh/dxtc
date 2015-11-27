using System;

using System.Runtime.InteropServices;

namespace dxtc
{
    public class DDS
    {
        /*
         * 
         * Programming Guide for DDS
         * https://msdn.microsoft.com/en-us/library/windows/desktop/bb943991(v=vs.85).aspx#File_Layout1
         * 
         * File Structure
         *
         * DWORD               dwMagic;
         * DDS_HEADER          header;
         * 
         * If the DDS_PIXELFORMAT dwFlags is set to DDPF_FOURCC and dwFourCC is set to "DX10" an additional DDS_HEADER_DXT10
         * 
         * DDS_HEADER_DXT10    header10;
         * 
         * A pointer to an array of bytes that contains the main surface data.
         * 
         * BYTE bdata[]
         * 
         * BYTE bdata2[]
         * 
         */

        public const UInt32 Magic = 0x20534444;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DDS_Magic
        {
            public UInt32 dwMagic;
        }


        // https://msdn.microsoft.com/en-us/library/bb943982(v=vs.85).aspx

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DDS_Header
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

            public static DDS_Header CreateDXT1Header(uint width, uint height, bool compressed = true)
            {
                return new DDS_Header
                {
                    dwSize = DDS_Header.size,
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
                };
            }

            #endregion


            #region Flags

            [Flags]
            public enum Flags : UInt32
            {
                DDSD_CAPS = 0x1,
                DDSD_HEIGHT = 0x2,
                DDSD_WIDTH = 0x4,
                DDSD_PITCH = 0x8,
                DDSD_PIXELFORMAT = 0x1000,
                DDSD_MIPMAPCOUNT = 0x20000,
                DDSD_LINEARSIZE = 0x80000,
                DDSD_DEPTH = 0x800000,

                DDS_HEADER_FLAGS_TEXTURE = DDSD_CAPS | DDSD_HEIGHT | DDSD_WIDTH | DDSD_PIXELFORMAT,
            };

            [Flags]
            public enum CAPSFlags : UInt32
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
                    return (uint)Marshal.SizeOf(typeof(DDS_Header));
                }
            }

            #endregion
        };


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
                        dwRBitMask = 0xf800,
                        dwBBitMask = 0x7e0,
                        dwGBitMask = 0x1f,
                        dwABitMask = 0,
                    };
                }
            }

            #endregion


            #region Flags

            [Flags]
            public enum Flags : UInt32
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


        // DDS_HEADER_DXT10 structure
        // https://msdn.microsoft.com/en-us/library/windows/desktop/bb943983(v=vs.85).aspx

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DDS_HEADER_DXT10
        {
            #region Fields

            // The surface pixel format (see DXGI_FORMAT).
            public DXGI_FORMAT dxgiFormat;

            public D3D10_RESOURCE_DIMENSION resourceDimension;

            // UINT.
            public UInt32 miscFlag;

            // UINT. The number of elements in the array.
            public UInt32 arraySize;

            // UINT.
            public UInt32 miscFlags2;

            #endregion


            #region Constructors

            public DDS_HEADER_DXT10 CreateDXT1Header
            {
                get
                {
                    return new DDS_HEADER_DXT10
                    {
                        dxgiFormat = DXGI_FORMAT.DXGI_FORMAT_B5G6R5_UNORM,
                        resourceDimension = D3D10_RESOURCE_DIMENSION.D3D10_RESOURCE_DIMENSION_TEXTURE2D,
                        miscFlag = 0,
                        //arraySize = 0,
                        miscFlags2 = 0, // DDS_ALPHA_MODE_UNKNOWN
                    };
                }
            }

            #endregion


            #region Flags

            // https://msdn.microsoft.com/en-us/library/windows/desktop/bb173059(v=vs.85).aspx

            public enum DXGI_FORMAT : UInt32
            {
                DXGI_FORMAT_UNKNOWN = 0,
                DXGI_FORMAT_R32G32B32A32_TYPELESS = 1,
                DXGI_FORMAT_R32G32B32A32_FLOAT = 2,
                DXGI_FORMAT_R32G32B32A32_UINT = 3,
                DXGI_FORMAT_R32G32B32A32_SINT = 4,
                DXGI_FORMAT_R32G32B32_TYPELESS = 5,
                DXGI_FORMAT_R32G32B32_FLOAT = 6,
                DXGI_FORMAT_R32G32B32_UINT = 7,
                DXGI_FORMAT_R32G32B32_SINT = 8,
                DXGI_FORMAT_R16G16B16A16_TYPELESS = 9,
                DXGI_FORMAT_R16G16B16A16_FLOAT = 10,
                DXGI_FORMAT_R16G16B16A16_UNORM = 11,
                DXGI_FORMAT_R16G16B16A16_UINT = 12,
                DXGI_FORMAT_R16G16B16A16_SNORM = 13,
                DXGI_FORMAT_R16G16B16A16_SINT = 14,
                DXGI_FORMAT_R32G32_TYPELESS = 15,
                DXGI_FORMAT_R32G32_FLOAT = 16,
                DXGI_FORMAT_R32G32_UINT = 17,
                DXGI_FORMAT_R32G32_SINT = 18,
                DXGI_FORMAT_R32G8X24_TYPELESS = 19,
                DXGI_FORMAT_D32_FLOAT_S8X24_UINT = 20,
                DXGI_FORMAT_R32_FLOAT_X8X24_TYPELESS = 21,
                DXGI_FORMAT_X32_TYPELESS_G8X24_UINT = 22,
                DXGI_FORMAT_R10G10B10A2_TYPELESS = 23,
                DXGI_FORMAT_R10G10B10A2_UNORM = 24,
                DXGI_FORMAT_R10G10B10A2_UINT = 25,
                DXGI_FORMAT_R11G11B10_FLOAT = 26,
                DXGI_FORMAT_R8G8B8A8_TYPELESS = 27,
                DXGI_FORMAT_R8G8B8A8_UNORM = 28,
                DXGI_FORMAT_R8G8B8A8_UNORM_SRGB = 29,
                DXGI_FORMAT_R8G8B8A8_UINT = 30,
                DXGI_FORMAT_R8G8B8A8_SNORM = 31,
                DXGI_FORMAT_R8G8B8A8_SINT = 32,
                DXGI_FORMAT_R16G16_TYPELESS = 33,
                DXGI_FORMAT_R16G16_FLOAT = 34,
                DXGI_FORMAT_R16G16_UNORM = 35,
                DXGI_FORMAT_R16G16_UINT = 36,
                DXGI_FORMAT_R16G16_SNORM = 37,
                DXGI_FORMAT_R16G16_SINT = 38,
                DXGI_FORMAT_R32_TYPELESS = 39,
                DXGI_FORMAT_D32_FLOAT = 40,
                DXGI_FORMAT_R32_FLOAT = 41,
                DXGI_FORMAT_R32_UINT = 42,
                DXGI_FORMAT_R32_SINT = 43,
                DXGI_FORMAT_R24G8_TYPELESS = 44,
                DXGI_FORMAT_D24_UNORM_S8_UINT = 45,
                DXGI_FORMAT_R24_UNORM_X8_TYPELESS = 46,
                DXGI_FORMAT_X24_TYPELESS_G8_UINT = 47,
                DXGI_FORMAT_R8G8_TYPELESS = 48,
                DXGI_FORMAT_R8G8_UNORM = 49,
                DXGI_FORMAT_R8G8_UINT = 50,
                DXGI_FORMAT_R8G8_SNORM = 51,
                DXGI_FORMAT_R8G8_SINT = 52,
                DXGI_FORMAT_R16_TYPELESS = 53,
                DXGI_FORMAT_R16_FLOAT = 54,
                DXGI_FORMAT_D16_UNORM = 55,
                DXGI_FORMAT_R16_UNORM = 56,
                DXGI_FORMAT_R16_UINT = 57,
                DXGI_FORMAT_R16_SNORM = 58,
                DXGI_FORMAT_R16_SINT = 59,
                DXGI_FORMAT_R8_TYPELESS = 60,
                DXGI_FORMAT_R8_UNORM = 61,
                DXGI_FORMAT_R8_UINT = 62,
                DXGI_FORMAT_R8_SNORM = 63,
                DXGI_FORMAT_R8_SINT = 64,
                DXGI_FORMAT_A8_UNORM = 65,
                DXGI_FORMAT_R1_UNORM = 66,
                DXGI_FORMAT_R9G9B9E5_SHAREDEXP = 67,
                DXGI_FORMAT_R8G8_B8G8_UNORM = 68,
                DXGI_FORMAT_G8R8_G8B8_UNORM = 69,
                DXGI_FORMAT_BC1_TYPELESS = 70,
                DXGI_FORMAT_BC1_UNORM = 71,
                DXGI_FORMAT_BC1_UNORM_SRGB = 72,
                DXGI_FORMAT_BC2_TYPELESS = 73,
                DXGI_FORMAT_BC2_UNORM = 74,
                DXGI_FORMAT_BC2_UNORM_SRGB = 75,
                DXGI_FORMAT_BC3_TYPELESS = 76,
                DXGI_FORMAT_BC3_UNORM = 77,
                DXGI_FORMAT_BC3_UNORM_SRGB = 78,
                DXGI_FORMAT_BC4_TYPELESS = 79,
                DXGI_FORMAT_BC4_UNORM = 80,
                DXGI_FORMAT_BC4_SNORM = 81,
                DXGI_FORMAT_BC5_TYPELESS = 82,
                DXGI_FORMAT_BC5_UNORM = 83,
                DXGI_FORMAT_BC5_SNORM = 84,
                DXGI_FORMAT_B5G6R5_UNORM = 85,
                DXGI_FORMAT_B5G5R5A1_UNORM = 86,
                DXGI_FORMAT_B8G8R8A8_UNORM = 87,
                DXGI_FORMAT_B8G8R8X8_UNORM = 88,
                DXGI_FORMAT_R10G10B10_XR_BIAS_A2_UNORM = 89,
                DXGI_FORMAT_B8G8R8A8_TYPELESS = 90,
                DXGI_FORMAT_B8G8R8A8_UNORM_SRGB = 91,
                DXGI_FORMAT_B8G8R8X8_TYPELESS = 92,
                DXGI_FORMAT_B8G8R8X8_UNORM_SRGB = 93,
                DXGI_FORMAT_BC6H_TYPELESS = 94,
                DXGI_FORMAT_BC6H_UF16 = 95,
                DXGI_FORMAT_BC6H_SF16 = 96,
                DXGI_FORMAT_BC7_TYPELESS = 97,
                DXGI_FORMAT_BC7_UNORM = 98,
                DXGI_FORMAT_BC7_UNORM_SRGB = 99,
                DXGI_FORMAT_AYUV = 100,
                DXGI_FORMAT_Y410 = 101,
                DXGI_FORMAT_Y416 = 102,
                DXGI_FORMAT_NV12 = 103,
                DXGI_FORMAT_P010 = 104,
                DXGI_FORMAT_P016 = 105,
                DXGI_FORMAT_420_OPAQUE = 106,
                DXGI_FORMAT_YUY2 = 107,
                DXGI_FORMAT_Y210 = 108,
                DXGI_FORMAT_Y216 = 109,
                DXGI_FORMAT_NV11 = 110,
                DXGI_FORMAT_AI44 = 111,
                DXGI_FORMAT_IA44 = 112,
                DXGI_FORMAT_P8 = 113,
                DXGI_FORMAT_A8P8 = 114,
                DXGI_FORMAT_B4G4R4A4_UNORM = 115,
                DXGI_FORMAT_P208 = 130,
                DXGI_FORMAT_V208 = 131,
                DXGI_FORMAT_V408 = 132,
                //DXGI_FORMAT_FORCE_UINT = 0xffffffff
            }

            public enum D3D10_RESOURCE_DIMENSION : UInt32
            {
                D3D10_RESOURCE_DIMENSION_UNKNOWN = 0,
                D3D10_RESOURCE_DIMENSION_BUFFER = 1,
                D3D10_RESOURCE_DIMENSION_TEXTURE1D = 2,
                D3D10_RESOURCE_DIMENSION_TEXTURE2D = 3,
                D3D10_RESOURCE_DIMENSION_TEXTURE3D = 4,
            }

            #endregion
        }


        // DXT1 block and color

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Block
        {
            public Color color0;
            public Color color1;
            public Int32 indices;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Color
        {
            public Int16 value;

            /*
            public static implicit operator Image.Color(Color color)
            {
                return new Image.Color
                {
                    r = color.r,
                    g = color.g,
                    b = color.b,
                };
            }

            public static implicit operator Color(Image.Color color)
            {
                return new Image.Color
                {
                    r = color.r,
                    g = color.g,
                    b = color.b,
                };
            }
            */
        }
    }
}
