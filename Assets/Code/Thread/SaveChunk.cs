using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.IO;
using Assets.Code.World;
using Assets.Code.World.Chunks;

namespace Assets.Code.Thread
{
    class SaveChunk : ThreadedJob
    {
        private ChunkComponent _chunkComponent;

        public SaveChunk(ChunkComponent chunkComponent)
        {
            _chunkComponent = chunkComponent;
        }
        protected override void ThreadFunction()
        {
            Serialization.SaveChunk(_chunkComponent);
        }

        protected override void OnFinished()
        {

        }
    }
}
