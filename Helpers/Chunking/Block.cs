using System.Buffers;

namespace OceansInFlame.Library.Helpers.Chunking
{
    public readonly record struct Block(IMemoryOwner<byte> Owner, Memory<byte> Memory, long Offset) { }
}
