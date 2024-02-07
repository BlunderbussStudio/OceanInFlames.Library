namespace OceansInFlame.Library.Helpers.Chunking
{
    public record struct ChunkDigest()
    {
        ulong gearHash = 0;

        public bool Push(byte b)
        {
            gearHash = (gearHash << 1) + ChunkConstants.GearTable[b];
            return (gearHash & ChunkConstants.MASK) == 0;
        }
    }
}
