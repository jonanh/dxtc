using System;

namespace dxtc.DDS
{
    using dxtc;

    public class DXT1Block : TexelBlock
    {
        #region Fields

        public Image.Color color0
        { 
            get; 
            private set;
        }

        public Image.Color color1
        {
            get;
            private set;
        }

        private byte[] indexes = new byte[16];

        #endregion

        //private void interpolateColor;

        public void apply()
        {
            color0 = pixels[0];
            color1 = pixels[1];

            // Select min/max colors
            foreach (var color in pixels)
            {
                if (color0 > color)
                {
                    color0 = color;
                }

                if (color1 < color)
                {
                    color1 = color;
                }
            }

            // Select indices
            foreach (var color in colorBlock)
            {
                var closer = 
            }
        }

        public static implicit operator DDS_DXT1Block(DXT1Block block)
        {
            UInt32 indices = 0;

            // Code into a bitarray
            uint i = 16;
            foreach(byte index in block.indexes)
            {
                indices |= (index & 0x3u);
                if (i > 0)
                {
                    indices = indices << 2;
                }
            }

            return new DDS_DXT1Block
            {
                color0 = block.color0,
                color1 = block.color1,
                indices = indices,
            };
        }

        public static implicit operator DXT1Block(DDS_DXT1Block block)
        {
            var newBlock = new DXT1Block
            {
                color0 = block.color0,
                color1 = block.color1,
            };

            // Decode from bitarray
            var indices = block.indices;

            for(uint i = 16; i > 0; i++)
            {
                newBlock.indexes[i] = (byte)(indices & 0x3);
                indices = indices >> 2;
            }

            return newBlock;
        }
    }
}
