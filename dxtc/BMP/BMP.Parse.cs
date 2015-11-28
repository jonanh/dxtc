using System;
using System.IO;

namespace dxtc.BMP
{
    public partial class BMP
    {
        public static BMP read(Stream stream)
        {
            var image = new BMP();

            int readIndex = 0;

            // Read headers
            readIndex += stream.ReadStruct(out image.fileHeader);

            readIndex += stream.ReadStruct(out image.infoHeader);

            if (image.infoHeader.biBitCount != 24 || 
                image.infoHeader.biCompression != BITMAPINFOHEADER.CompressionMode.BI_RGB)
            {
                throw new NotImplementedException("Format not implemented");
            }

            // Ensure we jump to the offset
            int seek = (int)image.fileHeader.bfOffBits - readIndex;

            stream.Seek(seek, SeekOrigin.Current);

            uint _height = image.uheight;
            uint _width = image.width;
            uint _size = _height * _width;

            // Initialize pixel matrix
            image.pixels = new BGR[_size];

            uint imagePadding = image.padding;

            // Optimize loop if there is no padding
            if (imagePadding > 0)
            {
                uint index = 0;
                for (uint i = 0; i < _height; i++)
                {
                    for (uint j = 0; j < _width; j++, index++)
                    {
                        BGR color;

                        readIndex += stream.ReadStruct(out color);

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
                for (uint i = 0; i < _size; i++)
                {
                    BGR color;

                    readIndex += stream.ReadStruct(out color);

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

            // Avoid accessing the size properties too much
            uint _height = uheight;
            uint _width = width;
            uint _size = _height * _width;

            // Optimize loop if there is no adding
            if (paddingBuffer.Length > 0)
            {
                uint index = 0;
                for (uint i = 0; i < _height; i++)
                {
                    for (uint j = 0; j < _width; j++, index++)
                    {
                        BGR color = this[j, i];
                        writeIndex += stream.WriteStruct(color);
                    }

                    stream.Write(paddingBuffer, 0, paddingBuffer.Length);
                }
            }
            else
            {
                for (uint i = 0; i < _size; i++)
                {
                    BGR color = this[i];
                    writeIndex += stream.WriteStruct(color);
                }
            }
        }
    }
}

