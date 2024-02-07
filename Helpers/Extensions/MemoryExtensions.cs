using OceansInFlame.Library.Helpers.Chunking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Helpers.Extensions
{
    public static class MemoryExtensions
    {
        public static IEnumerable<(Memory<byte> Memory, int Offset)> Chunk(this Memory<byte> buffer)
        {
            var offset = 0;
            while (offset < buffer.Length)
            {
                var chunkSize = SearchChunk(buffer.Slice(offset, Math.Min(buffer.Length - offset, ChunkConstants.MAX_CHUNK_SIZE)));
                yield return (buffer.Slice(offset, chunkSize), offset);
                offset += chunkSize;
            }
        }
        static int SearchChunk(Memory<byte> buffer)
        {
            var digest = new ChunkDigest();
            var span = buffer.Span;
            for (var i = ChunkConstants.MIN_CHUNK_SIZE; i < buffer.Length; i++)
            {
                var b = span[i];
                if (digest.Push(b))
                {
                    return i + 1;
                }
            }
            return buffer.Length;
        }
        public static void HashTo(this Memory<byte> src, Memory<byte> dst)
        {
            SHA512.Create().TryComputeHash(src.Span, dst.Span, out _);
        }
    }
}
