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

        public static void Run(string[] args)
        {
            if (args.Length < 2)
            {
                var appName = System.AppDomain.CurrentDomain.FriendlyName;

                Console.WriteLine("Invalid command. Syntax: " + appName + " <orig file> <dest file>");
            }
            else
            {
                Image image = null;
                try
                {
                    string file1 = args[0];
                    string file2 = args[1];

                    using (var fileStream = new FileStream(file1, FileMode.Open))
                    {
                        if (file1.ToLower().Contains(".dds"))
                        {
                            image = DDS.DDS.read(fileStream);
                        }
                        else if (file1.ToLower().Contains(".bmp"))
                        {
                            image = BMP.BMP.read(fileStream);
                        }
                        else
                        {
                            Console.WriteLine("Unknown file extension: " + file1);
                            return;
                        }

                        fileStream.Close();
                    }

                    File.Delete(file2);
                    using (var fileStream = new FileStream(file2, FileMode.OpenOrCreate))
                    {
                        if (file2.ToLower().Contains(".dds"))
                        {
                            DDS.DDS dds = image;
                            dds.write(fileStream);
                        }
                        else if (file2.ToLower().Contains(".bmp"))
                        {
                            BMP.BMP bmp = image;
                            bmp.write(fileStream);
                        }
                        else
                        {
                            Console.WriteLine("Unknown file extension: " + file2);
                            return;
                        }

                        fileStream.Close();
                    }
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine("File not found! " + e.Message);
                }
            }
        }
    }
}
