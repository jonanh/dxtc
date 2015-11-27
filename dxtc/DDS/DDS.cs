using System;

namespace dxtc.DDS
{
    public partial class DDS
    {
        #region Fields

        internal DDS_HEADER ddsHeader;

        internal DDS_HEADER_DXT10 ddsHeaderDXT10;

        internal DXT1Block[] blocks;

        #endregion

        private DDS()
        {
        }

        public DDS(uint width, uint height)
        {
            ddsHeader = DDS_HEADER.CreateDXT1Header(width, height);

            ddsHeaderDXT10 = DDS_HEADER_DXT10.CreateDXT1Header;

            blocks = new DXT1Block[width * height];
        }


        #region utils

        /// <summary>
        /// Gets the width from the DDS header.
        /// </summary>
        /// <value>The width.</value>
        public uint width
        {
            get
            {
                return ddsHeader.dwWidth;
            }
        }

        /// <summary>
        /// Gets the height from the DDS header.
        /// </summary>
        /// <value>The height.</value>
        public uint height
        {
            get
            {
                return ddsHeader.dwHeight;
            }
        }

        public uint horizontalBlocks
        {
            get
            {
                return (uint)Math.Ceiling(width / 4f);
            }
        }

        public uint verticalBlocks
        {
            get
            {
                return (uint)Math.Ceiling(height / 4f);
            }
        }

        #endregion

        public static implicit operator DDS(Image image)
        {
            var dds = new DDS(image.width, image.height);

            for(uint i = 0; i < image.height; i++)
            {
                for(uint j = 0; j < image.width; j++)
                {
                    
                }
            }

            return dds;
        }

        public static implicit operator Image(DDS bmp)
        {
            return null;
        }
    }
}
