using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World.Chunks;
using Assets.Code.World.Chunks.Blocks;
using Assets.Code.World.Terrain;
using UnityEngine;

namespace Assets.Code.World.Thread
{
    class FillChunkJob : ThreadedJob
    {
        public WorldPosition WorldPosition;  // arbitary job data
        public List<KeyValuePair<WorldPosition, Block>> Blocks { get { return _chunkGenerator.Blocks; } }

        private ChunkGenerator _chunkGenerator;

        public FillChunkJob(WorldPosition worldPosition)
        {
            WorldPosition = worldPosition;
        }

        protected override void ThreadFunction()
        {
            _chunkGenerator = new ChunkGenerator(WorldPosition);

            _chunkGenerator.FillChunk();
        }
        protected override void OnFinished()
        {

        }
    }
}
