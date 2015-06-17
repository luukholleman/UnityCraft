using System.Collections.Generic;
using Assets.Code.World;
using Assets.Code.World.Chunks.Blocks;
using Assets.Code.World.Terrain;

namespace Assets.Code.Thread
{
    class FillChunkJob : ThreadedJob
    {
        public Position Position;
        public List<KeyValuePair<Position, Block>> Blocks { get { return _chunkGenerator.Blocks; } }

        private ChunkGenerator _chunkGenerator;

        public FillChunkJob(Position position)
        {
            Position = position;
        }

        protected override void ThreadFunction()
        {
            _chunkGenerator = new ChunkGenerator(Position);

            _chunkGenerator.Generate();
        }

        protected override void OnFinished()
        {

        }
    }
}
