using System;
using System.Collections.Generic;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.World.Chunks.Blocks;

namespace Assets.Code.IO
{
    [Serializable]
    class Save
    {
        public Dictionary<WorldPosition, Block> blocks = new Dictionary<WorldPosition, Block>();

        public Save(Chunk chunk)
        {
            for (int x = 0; x < Chunk.ChunkSize; x++)
            {
                for (int y = 0; y < Chunk.ChunkSize; y++)
                {
                    for (int z = 0; z < Chunk.ChunkSize; z++)
                    {
                        if (!chunk.Blocks[x, y, z].Changed)
                            continue;

                        WorldPosition position = new WorldPosition(x, y, z);
                        blocks.Add(position, chunk.Blocks[x, y, z]);
                    }
                }
            }
        }
    }
}
