using System;

namespace dxtc.DDS
{
    using dxtc;

    public class DXT1Block : TexelBlock
    {
        #region Fields

        public Image.Color[] colors = new Image.Color[4];
        private byte[] indexes = new byte[16];

        #endregion

        public void decode(Image.Color color0, Image.Color color1, UInt32 indices)
        {
            if (color0 > color1)
            {
                colors[2] = new Image.Color(
                    (2f * color0.r + color1.r) / 3,
                    (2f * color0.g + color1.g) / 3,
                    (2f * color0.b + color1.b) / 3);

                colors[3] = new Image.Color(
                    (color0.r + 2f * color1.r) / 3,
                    (color0.g + 2f * color1.g) / 3,
                    (color0.b + 2f * color1.b) / 3);
            }
            else
            {
                colors[2] = new Image.Color(
                    (color0.r + color1.r) / 2,
                    (color0.g + color1.g) / 2,
                    (color0.b + color1.b) / 2);

                colors[3] = Image.Color.Black;
            }

            // Decode from bitarray
            for(int i = 15; i >= 0; i--)
            {
                var index = (byte)(indices & 0x3);
                pixels[i] = colors[index];
                indices = indices >> 2;
            }
        }

        public void encode()
        {
            // Select min/max colors
            foreach (var color in pixels)
            {
                if (colors[0] < color)
                {
                    colors[0] = color;
                }

                if (colors[1] > color)
                {
                    colors[1] = color;
                }
            }

            // For each pixel in the texel block
            for(int i = 15; i >= 0; i--)
            {
                // Select the closer color from the 4 colors
                var currentColor = pixels[i];
                var currentDistance = Math.Abs(currentColor.distance(colors[0]));
                byte candidateColor = 0;

                for(byte j = 1; j < 4; j++)
                {
                    var newDistance = Math.Abs(currentColor.distance(colors[j]));
                    if (newDistance < currentDistance)
                    {
                        currentDistance = newDistance;
                        candidateColor = j;
                    }
                }

                // Save it in the index array
                indexes[i] = candidateColor;
            }
        }

        public static implicit operator DDS_DXT1Block(DXT1Block block)
        {
            // Encode selecting the colors and generating the index
            block.encode();

            // Convert into a bitarray
            UInt32 indices = 0;

            uint i = 15;
            foreach(byte index in block.indexes)
            {
                indices |= (index & 0x3u);
                if (i > 0)
                {
                    i--;
                    indices = indices << 2;
                }
            }

            return new DDS_DXT1Block
            {
                color0 = block.colors[0],
                color1 = block.colors[1],
                indices = indices,
            };
        }

        public static implicit operator DXT1Block(DDS_DXT1Block block)
        {
            var newBlock = new DXT1Block();

            // Decode from bitarray
            newBlock.decode(block.color0, block.color1, block.indices);

            return newBlock;
        }
    }
}
