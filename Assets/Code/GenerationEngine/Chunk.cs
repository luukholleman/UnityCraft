using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Static;
using Assets.CoherentNoise;
using Assets.CoherentNoise.Generation;
using UnityEngine;
using Random = System.Random;

namespace Assets.Code.GenerationEngine
{
    public class Chunk
    {
        public Position Position;

        public WorldObject[, ,] Objects = new WorldObject[Generator.ChunkSize, Generator.ChunkSize, Generator.ChunkSize];
        
        public List<KeyValuePair<Position, WorldObject>> WorldObjects = new List<KeyValuePair<Position, WorldObject>>();

        public Chunk(Position position)
        {
            Position = position;
        }

        public void SetObject(Position position, WorldObject staticObject, bool replaceBlocks = false)
        {
            if (Helper.InChunk(position - Position) && (!WorldObjects.Any(w => w.Key == position) || replaceBlocks))
            {
                position -= Position;

                if (!replaceBlocks && Objects[position.x, position.y, position.z] != null)
                    return;

                Objects[position.x, position.y, position.z] = staticObject;
            }
        }
    }
}
