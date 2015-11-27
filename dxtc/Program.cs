using System;
using System.IO;

namespace dxtc
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Run(args);

            //Tests.Run();
        }

        public enum FORMAT
        {
            BMP, DDS
        }

        public static void Run(string[] args)
        {
            if (args.Length < 2)
            {
                var appName = System.AppDomain.CurrentDomain.FriendlyName;

                Console.WriteLine("Invalid command. Syntax: " + appName + " <orig file> <dest file>");
            }
            else
            {
                // Intermediate format
                Image image = null;

                string file1 = args[0];
                string file2 = args[1];

                // Check source and destionation formats
                FORMAT fromFormat;
                FORMAT toFormat;

                if (file1.ToLower().Contains(".dds"))
                {
                    fromFormat = FORMAT.DDS;
                }
                else if (file1.ToLower().Contains(".bmp"))
                {
                    fromFormat = FORMAT.BMP;
                }
                else
                {
                    Console.WriteLine("Unknown file extension: " + file1);
                    return;
                }

                if (file2.ToLower().Contains(".dds"))
                {
                    toFormat = FORMAT.DDS;
                }
                else if (file2.ToLower().Contains(".bmp"))
                {
                    toFormat = FORMAT.BMP;
                }
                else
                {
                    Console.WriteLine("Unknown file extension: " + file2);
                    return;
                }

                try
                {
                    // Read into Intermediate format
                    using (var fileStream = new FileStream(file1, FileMode.Open))
                    {
                        if (fromFormat == FORMAT.DDS)
                        {
                            image = DDS.DDS.read(fileStream);
                        }
                        else if (fromFormat == FORMAT.BMP)
                        {
                            image = BMP.BMP.read(fileStream);
                        }

                        fileStream.Close();
                    }

                    // Save into final format
                    File.Delete(file2);
                    using (var fileStream = new FileStream(file2, FileMode.OpenOrCreate))
                    {
                        if (toFormat == FORMAT.DDS)
                        {
                            DDS.DDS dds = image;
                            dds.write(fileStream);
                        }
                        else if (toFormat == FORMAT.BMP)
                        {
                            BMP.BMP bmp = image;
                            bmp.write(fileStream);
                        }

                        fileStream.Close();
                    }
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine("File not found! " + file1);
                }
            }
        }
    }
}
