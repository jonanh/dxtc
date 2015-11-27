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

        public static DDS CreateDXT1(uint width, uint height)
        {
            // DXT1 textures should multiple of 4
            uint _width = ((width + 3) / 4) * 4;
            uint _height = ((height + 3) / 4) * 4;

            return new DDS
            {
                ddsHeader = DDS_HEADER.CreateDXT1Header(_width, _height),

                ddsHeaderDXT10 = DDS_HEADER_DXT10.CreateDXT1Header,

                blocks = new DXT1Block[_width * _height / 16],
            };
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

        public DXT1Block this[uint x]
        {
            get
            {
                var block = blocks[x];
                if (block == null)
                {
                    blocks[x] = block = new DXT1Block();
                }
                return block;
            }
        }

        public DXT1Block this[uint x, uint y]
        {
            get
            {
                return this[x + y * horizontalBlocks];
            }
        }

        #endregion
    }
}
