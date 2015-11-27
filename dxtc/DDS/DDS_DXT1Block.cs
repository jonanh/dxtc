using System;
using System.Runtime.InteropServices;

namespace dxtc.DDS
{
    using dxtc;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DDS_DXT1Block
    {
        public ColorR5G6B5 color0;
        public ColorR5G6B5 color1;
        public UInt32 indices;

        #region Testing the size of the struct

        public static uint size
        {
            get
            {
                return (uint)Marshal.SizeOf(typeof(DDS_DXT1Block));
            }
        }

        public static bool correctSize
        {
            get
            {
                return size == 8;
            }
        }

        #endregion
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorR5G6B5
    {
        public UInt16 value;

        // The masks can also be validated with the documentation
        // https://msdn.microsoft.com/en-us/library/windows/desktop/bb943991(v=vs.85).aspx#common_dds_file_resource_formats_and_associated_header_content

        public const UInt32 RMask = 0xf800;
        public const UInt32 GMask = 0x7e0;
        public const UInt32 BMask = 0x1f;

        public static implicit operator Image.Color(ColorR5G6B5 color)
        {
            // Example of the red channel
            // 
            // RRRR RGGG GGGB BBBB 16bits
            // 1111 1111 1111 1111 <- original value
            // 1111 1000 0000 0000
            //                >> 8 
            // --------------------
            //           1111 1111 <- after shift
            //         & 1111 1000 (0xf8)
            // -------------------
            //           1111 1000 <- after and
            //           RRRR RRRR 8 bits

            UInt32 value = color.value;

            var r = (value >> (6 + 5 - 3)) & 0xf8u;
            var g = (value >> (5 - 2))     & 0xfcu;
            var b = (value << 3)           & 0xf8u;

            return new Image.Color(r, g, b);
        }

        public static implicit operator ColorR5G6B5(Image.Color color)
        {
            // Example of the red channel
            // 
            //           RRRR RRRR 8 bits
            //           1111 1111 <- original value
            //         & 1111 1000 (0xf8)
            // -------------------
            //           1111 1000 <- after and
            //                << 8 
            // -------------------
            // 1111 1000 0000 0000 <- after shift
            // RRRR RGGG GGGB BBBB 16 bits

            var value = ((color.r & 0xf8u) << (6 + 5 - 3)) |
                        ((color.g & 0xfcu) << (5 - 2)) |
                        ((color.g & 0xf8u) >> (3));
            
            return new ColorR5G6B5
            {
                value = (UInt16)value,
            };
        }

        #region Testing the size of the struct

        public static uint size
        {
            get
            {
                // should be 2
                return (uint)Marshal.SizeOf(typeof(ColorR5G6B5));
            }
        }

        public static bool correctSize
        {
            get
            {
                return size == 2;
            }
        }
        #endregion
    }
}

