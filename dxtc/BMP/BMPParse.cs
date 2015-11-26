
using System.IO;

namespace dxtc
{
    public partial class BMP
    {
        public static BMP read(Stream stream)
        {
            var image = new BMP();

            int readStream = 0;

            // Read headers
            readStream += stream.ReadStruct(out image.fileHeader);

            readStream += stream.ReadStruct(out image.infoHeader);

            // Ensure we jump to the offset

            int seek = (int)image.fileHeader.bfOffBits - readStream;

            stream.Seek(seek, SeekOrigin.Current);

            // Initialize pixel matrix
            image.pixels = new BGR[image.height * image.width];

            uint imagePadding = image.padding;

            readStream = 0;

            // Optimize loop
            if (imagePadding > 0)
            {
                uint index = 0;
                for (uint i = 0; i < image.height; i++)
                {
                    for (uint j = 0; j < image.width; j++, index++)
                    {
                        BGR color;

                        readStream += stream.ReadStruct(out color);

                        image[index] = color;
                    }

                    if (imagePadding > 0)
                    {
                        stream.Seek(imagePadding, SeekOrigin.Current);
                    }
                }
            }
            else
            {
                for (uint i = 0; i < image.height * image.width; i++)
                {
                    BGR color;

                    readStream += stream.ReadStruct(out color);

                    image[i] = color;
                }
            }

            return image;
        }

        public void write(Stream stream)
        {

            int writeIndex = 0;

            writeIndex += stream.WriteStruct(fileHeader);

            writeIndex += stream.WriteStruct(infoHeader);

            byte[] paddingBuffer = new byte[padding];

            // Optimize loop
            if (paddingBuffer.Length > 0)
            {
                uint index = 0;
                for (uint i = 0; i < infoHeader.biHeight; i++)
                {
                    for (uint j = 0; j < infoHeader.biWidth; j++, index++)
                    {
                        BGR color = this[j, i];
                        writeIndex += stream.WriteStruct(color);
                    }

                    stream.Write(paddingBuffer, 0, paddingBuffer.Length);
                }
            }
            else
            {
                for (uint i = 0; i < height * width; i++)
                {
                    BGR color = this[i];
                    writeIndex += stream.WriteStruct(color);
                }
            }
        }
    }
}

