using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.GenerationEngine;
using Assets.Code.World;
using UnityEngine;

namespace Assets.Code
{
    class Helper
    {
        public static Position SnapToGrid(Position origPosition)
        {
            Position playerChunk = new Position(
                Mathf.FloorToInt(origPosition.x / World.World.ChunkSize) * World.World.ChunkSize,
                Mathf.FloorToInt(origPosition.y / World.World.ChunkSize) * World.World.ChunkSize,
                Mathf.FloorToInt(origPosition.z / World.World.ChunkSize) * World.World.ChunkSize
                );

            return playerChunk;
        }

        private static int Mod(int a, int n)
        {
            int result = a % n;
            if ((a < 0 && n > 0) || (a > 0 && n < 0))
                result += n;
            return result;
        }

        public static Position InnerChunkPosition(Position pos)
        {
            return new Position(Mod(pos.x, World.World.ChunkSize), Mod(pos.y, World.World.ChunkSize), Mod(pos.z, World.World.ChunkSize));
        }


        public static bool InChunk(Position position)
        {
            return InChunk(position.x) && InChunk(position.y) && InChunk(position.z);
        }

        public static bool InChunk(int index)
        {
            if (index < 0 || index >= Generator.ChunkSize)
                return false;

            return true;
        }

    }
}
