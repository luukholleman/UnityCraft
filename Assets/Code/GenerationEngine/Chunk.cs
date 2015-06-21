using System;
using System.Collections.Generic;
using Assets.Code.Blocks;
using Assets.Code.Blocks.Plants;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.CoherentNoise;
using Assets.CoherentNoise.Generation;
using UnityEngine;
using Random = System.Random;

namespace Assets.Code.GenerationEngine
{
    public class Chunk
    {
        public Position Position;

        public Block[, ,] Blocks = new Block[Generator.ChunkSize, Generator.ChunkSize, Generator.ChunkSize];

        public List<KeyValuePair<Position, Block>> BeyondChunkBlocks = new List<KeyValuePair<Position, Block>>();

        public Chunk(Position position)
        {
            Position = position;
        }

        public void SetBlock(Position position, Block block, bool replaceBlocks = false)
        {
            if (Helper.InChunk(position - Position))
            {
                position -= Position;

                if (!replaceBlocks && Blocks[position.x, position.y, position.z] != null)
                    return;

                Blocks[position.x, position.y, position.z] = block;
            }
            else
            {
                BeyondChunkBlocks.Add(new KeyValuePair<Position, Block>(position, block));
            }
        }
    }
}
