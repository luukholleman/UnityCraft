using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.World.Chunks.Blocks;

namespace Assets.Code.Thread
{
    class GenerateChunkMesh : ThreadedJob
    {
        private Chunk _chunk;
        private Block[,,] _blocks;

        public MeshData MeshData;

        public GenerateChunkMesh(Chunk chunk, Block[, ,] blocks)
        {
            _chunk = chunk;
            _blocks = blocks;

            MeshData = new MeshData();
        }

        protected override void ThreadFunction()
        {
            for (int x = 0; x < Chunk.ChunkSize; x++)
                for (int y = 0; y < Chunk.ChunkSize; y++)
                    for (int z = 0; z < Chunk.ChunkSize; z++)
                        MeshData = _blocks[x, y, z].Blockdata(_chunk, x, y, z, MeshData);
        }

        protected override void OnFinished()
        {
            throw new NotImplementedException();
        }
    }
}
