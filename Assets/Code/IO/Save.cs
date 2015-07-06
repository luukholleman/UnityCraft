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

        public Save(Chunk chunk)
        {
			if (chunk.Built) {
				for (int x = 0; x < World.WorldSettings.ChunkSize; x++)
				{
					for (int y = 0; y < World.WorldSettings.ChunkSize; y++)
					{
						for (int z = 0; z < World.WorldSettings.ChunkSize; z++)
						{
							if (!chunk.GetObject(new Position(x, y, z)).Changed)
								continue;
							
							Position position = new Position(x, y, z) + chunk.Position;
							blocks.Add(position, chunk.GetObject(position));
						}
					}
				}
			}
        }
    }
}
