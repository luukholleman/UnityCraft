using System.Collections.Generic;
using Assets.Code.World;
using Assets.Code.World.Chunks.Blocks;
using Assets.Code.World.Terrain;

namespace Assets.Code.Thread
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

            _chunkGenerator.Generate();
        }

        protected override void OnFinished()
        {

        }
    }
}
