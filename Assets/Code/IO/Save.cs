using System;
using System.Collections.Generic;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Static;

namespace Assets.Code.IO
{
    [Serializable]
    class Save
    {
        public Dictionary<Position, WorldObject> blocks = new Dictionary<Position, WorldObject>();

        public Save(ChunkComponent chunkComponent)
        {
            for (int x = 0; x < World.World.ChunkSize; x++)
            {
                for (int y = 0; y < World.World.ChunkSize; y++)
                {
                    for (int z = 0; z < World.World.ChunkSize; z++)
                    {
                        if (chunkComponent.Blocks[x, y, z] == null || !chunkComponent.Blocks[x, y, z].Changed)
                            continue;

                        Position position = new Position(x, y, z);
                        blocks.Add(position, chunkComponent.Blocks[x, y, z]);
                    }
                }
            }
        }
    }
}
