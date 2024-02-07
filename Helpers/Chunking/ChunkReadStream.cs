namespace OceansInFlame.Library.Helpers.Chunking
{
    public class ChunkReadStream : Stream
    {
        readonly IEnumerator<byte[]> chunks;
        Memory<byte> chunk;
        int cursor;

        public ChunkReadStream(IEnumerable<byte[]> blocks)
        {
            chunks = blocks.GetEnumerator();
            Next();
        }

        void Next()
        {
            if (chunks.MoveNext())
            {
                chunk = new Memory<byte>(chunks.Current);
                cursor = 0;
            }
            else
            {
                chunk = Memory<byte>.Empty;
                cursor = 0;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var bufferSpan = new Memory<byte>(buffer).Span.Slice(offset, count);
            var cursor = 0;
            while (cursor < count)
            {
                var readLength = Math.Min(chunk.Length - this.cursor, count - cursor);
                chunk.Span.Slice(this.cursor, readLength).CopyTo(bufferSpan.Slice(cursor, readLength));
                this.cursor += readLength;
                cursor += readLength;
                if (this.cursor == chunk.Length)
                {
                    Next();
                }
            }
            return count;
        }
        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new InvalidOperationException();

        public override long Position { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }

        public override void Flush()
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

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException();
        }
    }
}
