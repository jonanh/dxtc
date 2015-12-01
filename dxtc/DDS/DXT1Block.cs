using System;

namespace dxtc.DDS
{
    using dxtc;

    public class DXT1Block : TexelBlock
    {
        #region Fields

        public Image.Color[] colors = new Image.Color[4];
        private UInt32 indices;

        #endregion

        public void decode(ColorR5G6B5 colorOrig0, ColorR5G6B5 colorOrig1, UInt32 indices)
        {
            var color0 = colors[0] = colorOrig0;
            var color1 = colors[1] = colorOrig1;

            // Select color2 and color3 following the COMPRESSED_RGB_S3TC_DXT1_EXT spec
            // 
            // https://www.opengl.org/registry/specs/EXT/texture_compression_s3tc.txt
            // 

            if (colorOrig0.value >= colorOrig1.value)
            {
                colors[2] = new Image.Color(
                    (2f * color0.r + color1.r) / 3f,
                    (2f * color0.g + color1.g) / 3f,
                    (2f * color0.b + color1.b) / 3f);

                colors[3] = new Image.Color(
                    (color0.r + 2f * color1.r) / 3f,
                    (color0.g + 2f * color1.g) / 3f,
                    (color0.b + 2f * color1.b) / 3f);
            }
            else
            {
                colors[2] = new Image.Color(
                    (color0.r + color1.r) / 2f,
                    (color0.g + color1.g) / 2f,
                    (color0.b + color1.b) / 2f);

                colors[3] = Image.Color.Black;
            }

            // Decode from bitarray
            for(int i = 0; i < 16; i++)
            {
                var index = (byte)(indices & 0x3);
                pixels[i] = colors[index];
                indices = indices >> 2;
            }
        }

        private const double startDistance = 100000;

        public void encode()
        {
            // The idea is to get the most distant complementary colors in a simple way
            // A simple algorithm with an O(n) function cost

            var nearRed     = pixels[0];
            var nearGreen   = pixels[0];
            var nearBlue    = pixels[0];
            var nearYellow  = pixels[0];
            var nearCyan    = pixels[0];
            var nearMarge   = pixels[0];
            var nearWhite   = pixels[0];
            var nearBlack   = pixels[0];

            var distRed = startDistance;
            var distGreen = startDistance;
            var distBlue = startDistance;
            var distYellow = startDistance;
            var distCyan = startDistance;
            var distMarge = startDistance;
            var distWhite = startDistance;
            var distBlack = startDistance;

            var newDistance = startDistance;

            // First we will check the closest color to each axis
            foreach (var color in pixels)
            {
                newDistance = Image.Color.Red.distance(color);
                if (newDistance < distRed)
                {
                    nearRed = color;
                    distRed = newDistance;
                }

                newDistance = Image.Color.Green.distance(color);
                if (newDistance < distGreen)
                {
                    nearGreen = color;
                    distGreen = newDistance;
                }

                newDistance = Image.Color.Blue.distance(color);
                if (newDistance < distBlue)
                {
                    nearBlue = color;
                    distBlue = newDistance;
                }

                newDistance = Image.Color.Yellow.distance(color);
                if (newDistance < distYellow)
                {
                    nearYellow = color;
                    distYellow = newDistance;
                }

                newDistance = Image.Color.Cyan.distance(color);
                if (newDistance < distCyan)
                {
                    nearCyan = color;
                    distCyan = newDistance;;
                }

                newDistance = Image.Color.Margenta.distance(color);
                if (newDistance < distMarge)
                {
                    nearMarge = color;
                    distMarge = newDistance;
                }

                newDistance = Image.Color.White.distance(color);
                if (newDistance < distWhite)
                {
                    nearWhite = color;
                    distWhite = newDistance;
                }

                newDistance = Image.Color.Black.distance(color);
                if (newDistance < distBlack)
                {
                    nearBlack = color;
                    distBlack = newDistance;
                }
            }

            // Get the most distant complementary colors
            var lastDistance = nearRed.distance(nearCyan);
            colors[0] = nearRed;
            colors[1] = nearCyan;

            newDistance = nearBlue.distance(nearYellow);
            if (lastDistance < newDistance)
            {
                colors[0] = nearBlue;
                colors[1] = nearYellow;
                lastDistance = newDistance;
            }

            newDistance = nearGreen.distance(nearMarge);
            if (lastDistance < newDistance)
            {
                colors[0] = nearGreen;
                colors[1] = nearMarge;
            }

            newDistance = nearWhite.distance(nearBlack);
            if (lastDistance < newDistance)
            {
                colors[0] = nearWhite;
                colors[1] = nearBlack;
            }

            // By default make color0 bigger than color1, so we get
            // more interpolation values

            ColorR5G6B5 color_0_R5G6B5 = colors[0];
            ColorR5G6B5 color_1_R5G6B5 = colors[1];

            // Compare the R5G6B5 colors
            if (color_0_R5G6B5.value < color_1_R5G6B5.value)
            {
                var temp = colors[0];
                colors[0] = colors[1];
                colors[1] = temp;
            }

            // Calculate the colors after the selection

            colors[2] = new Image.Color(
                (2f * colors[0].r + colors[1].r) / 3f,
                (2f * colors[0].g + colors[1].g) / 3f,
                (2f * colors[0].b + colors[1].b) / 3f);

            colors[3] = new Image.Color(
                (colors[0].r + 2f * colors[1].r) / 3f,
                (colors[0].g + 2f * colors[1].g) / 3f,
                (colors[0].b + 2f * colors[1].b) / 3f);
            
            // Use third color?
            int last_color = color_0_R5G6B5.value > color_1_R5G6B5.value ? 4 : 3;

            // For each pixel in the texel block
            for(int i = 0; i < 16; i++)
            {
                // Select the closer color from the 4 colors
                var currentColor = pixels[i];
                lastDistance = currentColor.distance(colors[0]);
                uint candidateColor = 0;

				for(uint j = 1; j < last_color; j++)
                {
                    newDistance = currentColor.distance(colors[j]);
                    if (newDistance < lastDistance)
                    {
                        lastDistance = newDistance;
                        candidateColor = j;
                    }
                }

                // Save it in the index array
                indices |= ((candidateColor & 0x3) << (i * 2));
            }
        }

        public static implicit operator DDS_DXT1Block(DXT1Block block)
        {
            // Encode selecting the colors and generating the index
            block.encode();

            return new DDS_DXT1Block
            {
                color0 = block.colors[0],
                color1 = block.colors[1],
                indices = block.indices,
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
