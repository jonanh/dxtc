using System;
using System.IO;

namespace dxtc
{
    using BMP;
    using DDS;

    public static class Tests
    {
        public static void Run()
        {
            Tests.TestStructSizes();
            Tests.TestReadBmp();
            Tests.TestWriteBmp();
            Tests.TestDDS();
            Tests.TestDDSBlock();
            Tests.TestDDS2();
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
            Image gradient = gradientImage();

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
                // Convert to image and back to bmp
                Image image = bmp;
                bmp = image;

                bmp.write(fileStream2);
                fileStream2.Close();
            }

            // Check the padding and content generating a gradient
            File.Delete("gradient.bmp");
            using (var fileStream = new FileStream("gradient.bmp", FileMode.OpenOrCreate))
            {
                // Convert to BMP
                bmp = gradient;

                // Write it to the disk
                bmp.write(fileStream);

                fileStream.Close();
            }

            // Check the saved grandient
            using (var fileStream = new FileStream("gradient.bmp", FileMode.Open))
            {
                bmp = BMP.BMP.read(fileStream);

                if (bmp.width != gradient.width)
                {
                    Console.WriteLine("Saved and read gradient image contain different width!");
                }

                if (bmp.uheight != gradient.height)
                {
                    Console.WriteLine("Saved and read gradient image contain different height!");
                }

                Image.Color color0 = gradient[5, 4];
                Image.Color color1 = bmp[5, 4];

                if (!color0.Equals(color1))
                {
                    Console.WriteLine("Saved and read gradient image contain different pixels!");
                }

                fileStream.Close();
            }
        }

        public static void TestDDS()
        {
            Image gradient = gradientImage();

            DDS.DDS dds = gradient;

            if (dds.width != (uint)(Math.Ceiling(gradient.width / 4f) * 4))
            {
                Console.WriteLine("DDS file has an incorrent width!");
            }

            if (dds.height != (uint)(Math.Ceiling(gradient.height / 4f) * 4))
            {
                Console.WriteLine("DDS file has an incorrent height!");
            }

            Image image2 = dds;

            Image.Color color0 = image2[5, 4];
            Image.Color color1 = gradient[5, 4];

            if (!color0.Equals(color1))
            {
                Console.WriteLine("Image -> DDS -> Image fails pixel data!");
            }
        }

        public static void TestDDSBlock()
        {
            // R5G6B5 lossless color
            var color = new Image.Color(248, 252, 248);

            // Image with a single block
            Image image = new Image(4, 4, color);

            // Image -> DDS
            DDS.DDS dds = image;

            if (dds.horizontalBlocks != 1 || dds.verticalBlocks != 1)
            {
                Console.WriteLine("DDS file has an incorrent number of blocks!");
            }

            if (!dds[0][0].Equals(color) || !dds[0][0].Equals(color))
            {
                Console.WriteLine("DDS block has an incorrent color!");
            }

            // Get serialized block struct
            DDS_DXT1Block structBlock = dds[0];
            Image.Color color2 = structBlock.color0;

            if (!color2.Equals(color) || (structBlock.indices & 0x3) != 0)
            {
                Console.WriteLine("StructBlock contains incorrect colors!");
            }

            // Set some colors to black and get the block again
            image[7] = Image.Color.Black;
            image[15] = Image.Color.Black;
            // image -> dds -> block -> struct block -> block
            dds = image;
            structBlock = dds[0];
            DXT1Block block = structBlock;

            var color3 = block[0];
            var color4 = block[7];
            var color5 = block[15];

            if (!color3.Equals(color) ||
                !color4.Equals(Image.Color.Black) ||
                !color5.Equals(Image.Color.Black))
            {
                Console.WriteLine("DDS failed saving/restoring 2 colors!");
            }

            // Let's save the file and see what happens

            File.Delete("block.dds");
            using (var fileStream = new FileStream("block.dds", FileMode.OpenOrCreate))
            {
                dds.write(fileStream);

                fileStream.Close();
            }

            DDS.DDS dds2;
            using (var fileStream = new FileStream("block.dds", FileMode.OpenOrCreate))
            {
                dds2 = DDS.DDS.read(fileStream);
                fileStream.Close();
            }

            block = dds2[0];

            color3 = block[0];
            color4 = block[7];
            color5 = block[15];

            if (!color3.Equals(color) ||
                !color4.Equals(Image.Color.Black) ||
                !color5.Equals(Image.Color.Black))
            {
                Console.WriteLine("DDS failed saving/restoring 2 colors!");
            }
        }

        public static void TestDDS2()
        {
            Image image = null;
            DDS.DDS dss = null;
            BMP.BMP bmp = null;

            // Read BMP
            using (var fileStream = new FileStream("forest4.bmp", FileMode.Open))
            {
                image = BMP.BMP.read(fileStream);

                fileStream.Close();
            }

            // Write DDS
            File.Delete("forest4.dds");
            using (var fileStream = new FileStream("forest4.dds", FileMode.OpenOrCreate))
            {
                dss = image;
                dss.write(fileStream);

                fileStream.Close();
            }

            // Read DDS
            using (var fileStream = new FileStream("forest4.dds", FileMode.OpenOrCreate))
            {
                image = DDS.DDS.read(fileStream);

                fileStream.Close();
            }

            // See what happened in a BMP
            File.Delete("forest5.bmp");
            using (var fileStream = new FileStream("forest5.bmp", FileMode.OpenOrCreate))
            {
                bmp = image;
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

        public static Image gradientImage()
        {
            var image = new Image(23, 17);

            uint index = 0;
            for(uint i = 0; i < image.height; i++)
            {
                for(uint j = 0; j < image.width; j++, index++)
                {
                    image[index] = new Image.Color(i * 10u, j * 10u, 0);
                }
            }

            return image;
        }
    }
}
