using System;
using System.Runtime.InteropServices;

namespace dxtc.DDS
{
    using dxtc;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DXT1BlockStruct
    {
        public ColorR5G6B5 color0;
        public ColorR5G6B5 color1;
        public Int32 indices;

        #region Testing the size of the struct

        public static uint size
        {
            get
            {
                return (uint)Marshal.SizeOf(typeof(DXT1Block));
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
        public Int16 value;

        public const UInt32 RMask = 0xf800;
        public const UInt32 GMask = 0x7e0;
        public const UInt32 BMask = 0x1f;

        public static implicit operator Image.Color(ColorR5G6B5 color)
        {
            return new Image.Color
            {
//                r = color.r,
//                g = color.g,
//                b = color.b,
            };
        }

        public static implicit operator ColorR5G6B5(Image.Color color)
        {
//            var value = 
//                ((color.r << 11) & RMask) |
//                ((color.r << ) & GMask) | 
//                (color.g & 0x1f) << 5) |
////                          ((color.g & 0x1f) << 5) |
////                          (color.b & 0x1f);
//
            return new ColorR5G6B5
            {
                value = 0,
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

