using UnityEngine;

namespace Voxels
{
    public class ChunkData
    {
        public WorldPos pos;
        readonly BlockId[,,] blocks;

        public ChunkData(WorldPos pos)
        {
            this.pos = pos;
            this.blocks = new BlockId[MapConstants.ChunkSize, MapConstants.ChunkSize, MapConstants.ChunkSize];
        }

        public ChunkData(int x, int y, int z)
            : this(new WorldPos(x, y, z))
        {
        }

        public BlockId GetBlock(int x, int y, int z)
        {
            return this.blocks[x, y, z];
        }

        public void SetBlock(int x, int y, int z, BlockId blockId)
        {
            this.blocks[x, y, z] = blockId;
        }
    }
}