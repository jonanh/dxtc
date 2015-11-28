using System;

namespace dxtc.DDS
{
    public partial class DDS
    {
        public static implicit operator DDS(Image image)
        {
            uint _imgheight = image.height;
            uint _imgwidth = image.width;

            var dds = DDS.CreateDXT1(_imgwidth, _imgheight);

            uint _height = dds.height;
            uint _width = dds.width;

            for(uint i = 0; i < _height; i++)
            {
                for(uint j = 0; j < _width; j++)
                {
                    // Check the boundaries of the image, repeat the last valid value per axis
                    var ii = i >= _imgheight ? _imgheight - 1 : i;
                    var jj = j >= _imgwidth ? _imgwidth - 1 : j;

                    // Get the 4x4 block coordinate and the block
                    var iii = i / 4;
                    var jjj = j / 4;

                    var block = dds[jjj, iii];

                    // Set the color in the block
                    block[j % 4, i % 4] = image[jj, ii];
                }
            }

            return dds;
        }

        public static implicit operator Image(DDS dds)
        {
            var image = new Image(dds.width, dds.height);

            uint _imgheight = image.height;
            uint _imgwidth = image.width;

            for(uint i = 0; i < _imgheight; i++)
            {
                for(uint j = 0; j < _imgwidth; j++)
                {
                    // Get the 4x4 block coordinate
                    var ii = i / 4;
                    var jj = j / 4;

                    // Get the block
                    var block = dds[jj, ii];

                    // Get the color
                    image[j, i] = block[j % 4, i % 4];
                }
            }

            return image;
        }
    }
}

