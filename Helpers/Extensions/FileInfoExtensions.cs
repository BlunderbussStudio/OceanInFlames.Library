using OceansInFlame.Library.Helpers.Chunking;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Helpers.Extensions
{
    public static class FileInfoExtensions
    {
        public static Stream CreateTempStream(out string path, bool deleteOnClose = false)
        {
            path = Path.Combine(Path.GetTempPath(), "OR_CACHE_FILE_" + Guid.NewGuid().ToString());
            var options = FileOptions.Asynchronous
                        | FileOptions.RandomAccess
                        | (deleteOnClose ? FileOptions.DeleteOnClose : FileOptions.None);
            var stream = File.Create(path, 1 << 16, options);
            return stream;
        }
    }

    public static class StreamExtensions
    {
        public static IEnumerable<Block> ReadBlocks(this Stream stream, int size)
        {
            while (true)
            {
                var owner = MemoryPool<byte>.Shared.Rent(size);
                var memory = owner.Memory;
                var offset = stream.Position;
                var readLength = stream.Read(memory.Span);
                if (readLength == 0) break;
                yield return new Block(owner, memory[..readLength], offset);
            }
        }
    }
}
