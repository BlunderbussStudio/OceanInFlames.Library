using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Helpers.Chunking
{
    public class BlockWriteStream : Stream, IEnumerable<Block>
    {
        public List<Block> blocks = new List<Block>();
        long position = 0;
        readonly int blockSize;
        IMemoryOwner<byte> owner;
        Memory<byte> buffer;
        int cursor;

        public BlockWriteStream(int blockSize)
        {
            this.blockSize = blockSize;
            Malloc();
        }

        void Malloc(int size = -1)
        {
            owner = MemoryPool<byte>.Shared.Rent(size < 0 ? blockSize : size);
            buffer = owner.Memory;
            cursor = 0;
        }

        void Push()
        {
            blocks.Add(new Block(owner, buffer[..cursor], position));
        }

        public override void Write(byte[] bytes, int offset, int count)
        {
            var cursor = 0;
            var buffer = new Memory<byte>(bytes);
            while (cursor < count)
            {
                var writeLength = Math.Min(count - cursor, blockSize - this.cursor);
                buffer.Slice(cursor, writeLength).CopyTo(this.buffer.Slice(this.cursor, writeLength));
                cursor += writeLength;
                this.cursor += writeLength;
                position += writeLength;
                if (this.cursor == blockSize)
                {
                    Push();
                    Malloc();
                }
            }
        }

        public void Close()
        {
            if (cursor > 0)
            {
                Push();
                Malloc(0);
            }
        }

        public IEnumerator<Block> GetEnumerator() => blocks.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => blocks.GetEnumerator();

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => blocks.Sum(block => (long)block.Memory.Length) + buffer.Length;

        public override long Position { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }

        public override void Flush()
        {
            throw new InvalidOperationException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException();
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException();
        }
    }
}
