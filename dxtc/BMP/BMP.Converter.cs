using System;

namespace dxtc.BMP
{
    public partial class BMP
    {
        public static implicit operator BMP(Image image)
        {
            var bmp = new BMP(image.width, -(int)image.height);
            var size = image.height * image.width;

            for(uint i = 0; i < size; i++)
            {
                bmp[i] = image[i];
            }

            return bmp;
        }

        public static implicit operator Image(BMP bmp)
        {
            var image = new Image(bmp.width, bmp.uheight);

            // Bottom up
            if (bmp.height < 0)
            {
                for(uint i = 0; i < image.height * image.width; i++)
                {
                    image[i] = bmp[i];
                }
            }
            else
            {
                // Index to reverse the matrix from bottom to up
                uint index = (image.height - 1) * image.width;
                uint ii = 0;

                for(uint i = 0; i < image.height; i++)
                {
                    for(uint j = 0; j < image.width; j++, ii++)
                    {
                        image[index + j] = bmp[ii];
                    }

                    index -= image.width;
                }
            }

            return image;
        }
    }
}

