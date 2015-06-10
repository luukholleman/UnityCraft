using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World;
using Assets.Code.World.Chunk;
using Assets.Code.World.Chunk.Block;

namespace Assets.Code.IO
{
    [Serializable]
    class Save
    {
        public Dictionary<WorldPos, Block> blocks = new Dictionary<WorldPos, Block>();

        public Save(Chunk chunk)
        {
            for (int x = 0; x < Chunk.ChunkSize; x++)
            {
                for (int y = 0; y < Chunk.ChunkSize; y++)
                {
                    for (int z = 0; z < Chunk.ChunkSize; z++)
                    {
                        if (!chunk.Blocks[x, y, z].changed)
                            continue;

                        WorldPos pos = new WorldPos(x, y, z);
                        blocks.Add(pos, chunk.Blocks[x, y, z]);
                    }
                }
            }
        }
    }
}
