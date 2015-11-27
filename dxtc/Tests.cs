using System;
using System.IO;

namespace dxtc
{
    using BMP;

    public static class Tests
    {

        public static void Run()
        {
            Tests.TestStructSizes();
            Tests.TestReadBmp();
            Tests.TestWriteBmp();
            Tests.TestColorChange();
        }

        public static void TestStructSizes()
        {
            if (DDS.DDS_HEADER.size != 124)
            {
                Console.WriteLine("Wrong DDS.DDS_Header size");
            }

            if (DDS.DDS_PIXELFORMAT.size != 32)
            {
                Console.WriteLine("Wrong DDS.DDS_PIXELFORMAT size");
            }

            if (DDS.ColorR5G6B5.size != 2)
            {
                Console.WriteLine("Wrong DDS.ColorR5G6B5 size");
            }

            if (DDS.DDS_DXT1Block.size != 8)
            {
                Console.WriteLine("Wrong DDS.DXT1Block size");
            }

            if (BMP.BITMAPINFOHEADER.size != 40)
            {
                Console.WriteLine("Wrong BMP.BITMAPINFOHEADER size");
            }

            if (BMP.BITMAPFILEHEADER.size != 14)
            {
                Console.WriteLine("Wrong BMP.BITMAPFILEHEADER size");
            }
        }

        public static void TestReadBmp()
        {
            using (var fileStream = new FileStream("test.bmp", FileMode.Open))
            {
                var bmp = BMP.BMP.read(fileStream);

                if (bmp.width != 16)
                {
                    Console.WriteLine("Wrong BMP size");                    
                }

                if (bmp.height != 16)
                {
                    Console.WriteLine("Wrong BMP size");
                }

                Image image = bmp;

                Image.Color firstColor = image[0, 0];
                Image.Color lastColor = image[15, 15];

                if (!firstColor.Equals(Image.Color.Red))
                {
                    Console.WriteLine("First pixel should be red");
                }

                if (!lastColor.Equals(Image.Color.Black))
                {
                    Console.WriteLine("Last pixel should be black");
                }

                fileStream.Close();
            }
        }

        public static void TestWriteBmp()
        {
            BMP.BMP bmp;

            // Load the example test.bmp
            using (var fileStream = new FileStream("test.bmp", FileMode.Open))
            {
                bmp = BMP.BMP.read(fileStream);
                fileStream.Close();
            }


            // Check replicating the test file test.bmp
            File.Delete("test2.bmp");
            using (var fileStream2 = new FileStream("test2.bmp", FileMode.OpenOrCreate))
            {
                Image image = bmp;
                BMP.BMP bmp2 = image;

                bmp2.write(fileStream2);
                fileStream2.Close();
            }

            // Check the padding and content generating a gradient
            File.Delete("test3.bmp");
            using (var fileStream = new FileStream("test3.bmp", FileMode.OpenOrCreate))
            {
                var image = new Image(17, 17);

                uint index = 0;
                for(uint i = 0; i < image.height; i++)
                {
                    for(uint j = 0; j < image.width; j++, index++)
                    {
                        image[index] = new Image.Color(i * 10u, j * 10u, 0);
                    }
                }

                // Generate the bmp
                bmp = image;

                // Write it to the disk
                bmp.write(fileStream);

                fileStream.Close();
            }
        }

        public static void TestColorChange()
        {
            // Check a color with all the components to 0
            DDS.ColorR5G6B5 black = Image.Color.Black;
            if (!Image.Color.Black.Equals((Image.Color)black))
            {
                Console.WriteLine("Black pixel convertion failed!");
            }

            // Check a color without color loss
            var gray = new Image.Color(248, 252, 248);
            DDS.ColorR5G6B5 convertedGray = gray;

            if (!gray.Equals((Image.Color)convertedGray))
            {
                Console.WriteLine("Gray pixel convertion failed!");
            }

            // Check the loss of the white information
            DDS.ColorR5G6B5 convertedWhite = Image.Color.White;

            if (!gray.Equals((Image.Color)convertedWhite))
            {
                Console.WriteLine("White pixel convertion failed!");
            }
        }
    }
}
