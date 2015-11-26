using System;
using System.Runtime.InteropServices;
using System.IO;

namespace dxtc
{
    // Based on :
    // - http://www.matthew-long.com/2005/10/18/memory-pinning/
    // - http://stackoverflow.com/questions/25808429/is-there-a-generic-way-to-write-a-struct-to-bytes-in-big-endian-format

    public static class FileExtensions
    {
        /// <summary>
        /// Utility for reading structs from streams
        /// </summary>
        /// <returns>The number of bytes read from the stream.</returns>
        /// <param name="stream">Stream.</param>
        /// <param name="value">The struct.</param>
        /// <typeparam name="T">Struct type.</typeparam>
        public static int ReadStruct<T>(this Stream stream, out T value) where T : struct
        {
            var type = typeof(T);
            var size = Marshal.SizeOf(type);

            // Create a buffer
            var buffer = new byte[size];

            // Read the buffer
            var read = stream.Read(buffer, 0, size);

            // Make sure that the Garbage Collector doesn't touch the buffer 
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                // Marshal the buffer
                value = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
                return read;
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Utility for writting structs to streams.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="instance">Instance.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void WriteStruct<T>(this Stream stream, T instance) where T : struct
        {
            var type = typeof(T);
            var size = Marshal.SizeOf(type);

            // Create a buffer
            var buffer = new byte[size];

            // Allocates memory from the unmanaged memory of the process.
            IntPtr ptr = Marshal.AllocHGlobal(size);

            try
            {
                // Marshals data from a managed object to an unmanaged block of memory.
                Marshal.StructureToPtr(instance, ptr, true);

                // Copies data from a managed array to an unmanaged memory pointer, or from an unmanaged memory pointer to a managed array
                Marshal.Copy(ptr, buffer, 0, size);

                stream.Write(buffer, 0, buffer.Length);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
