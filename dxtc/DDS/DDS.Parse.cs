using System;
using System.IO;
using System.Runtime.InteropServices;

namespace dxtc.DDS
{
    public partial class DDS
    {
        // Header
        public const UInt32 Magic = 0x20534444;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DDS_Magic
        {
            public UInt32 dwMagic;
        }

        /*
         * 
         * Programming Guide for DDS
         * https://msdn.microsoft.com/en-us/library/windows/desktop/bb943991(v=vs.85).aspx#File_Layout1
         * 
         * File Structure
         *
         * DWORD               dwMagic;
         * DDS_HEADER          header;
         * 
         * If the DDS_PIXELFORMAT dwFlags is set to DDPF_FOURCC and dwFourCC is set to "DX10" an additional DDS_HEADER_DXT10
         * 
         * DDS_HEADER_DXT10    header10;
         * 
         * A pointer to an array of bytes that contains the main surface data.
         * 
         * BYTE bdata[]
         * 
         * BYTE bdata2[]
         * 
         */

        public static DDS read(Stream stream)
        {
            var dds = new DDS();

            int readIndex = 0;

            // Read headers

            DDS_Magic fileMagicData;

            readIndex += stream.ReadStruct(out fileMagicData);

            readIndex += stream.ReadStruct(out dds.ddsHeader);

            readIndex += stream.ReadStruct(out dds.ddsHeaderDXT10);

            // Initialize and read DXT1 blocks

            var size = dds.height * dds.width;

            dds.blocks = new DXT1Block[size];

            for (uint i = 0; i < size; i++)
            {
                DDS_DXT1Block block;
                readIndex += stream.ReadStruct(out block);

                dds.blocks[i] = block;
            }

            return dds;
        }

        public void write(Stream stream)
        {
            int writeIndex = 0;

            writeIndex += stream.WriteStruct(new DDS_Magic());

            writeIndex += stream.WriteStruct(ddsHeader);

            // Only read the ddsHeaderDXT10 when the FOURCC.DX10 is set
            if (ddsHeader.ddspf.dwFourCC == DDS_PIXELFORMAT.FOURCC.DX10)
            {
                writeIndex += stream.WriteStruct(ddsHeaderDXT10);
            }

            foreach (var block in blocks)
            {
                DDS_DXT1Block structBlock = block;
                stream.WriteStruct(structBlock);
            }
        }
    }
}
