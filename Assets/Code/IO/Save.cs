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
        public Dictionary<Position, Block> blocks = new Dictionary<Position, Block>();

        public Save(Chunk chunk)
        {
            for (int x = 0; x < World.World.ChunkSize; x++)
            {
                for (int y = 0; y < World.World.ChunkSize; y++)
                {
                    for (int z = 0; z < World.World.ChunkSize; z++)
                    {
                        if (chunk.Blocks[x, y, z] == null || !chunk.Blocks[x, y, z].Changed)
                            continue;

                        Position position = new Position(x, y, z);
                        blocks.Add(position, chunk.Blocks[x, y, z]);
                    }
                }
            }
        }
    }
}
