using System;

namespace dxtc.DDS
{
    using dxtc;

    public class DXT1WorkBlock
    {
        public Image.Color color0;
        public Image.Color color1;

        public DXT1WorkBlock(Image.Color[] colorBlock)
        {
            color0 = colorBlock[0];
            color1 = colorBlock[1];
            indices = 0;

            // Select min/max colors
            foreach (var color in colorBlock)
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
                //
            }
        }

        public Image.Color this[uint i]
        {
            get
            {
                return Image.Color.Black;
            }
        }
    }
}

