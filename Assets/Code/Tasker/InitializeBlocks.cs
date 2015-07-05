using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.GenerationEngine;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Dynamic;
using Assets.Code.WorldObjects.Static;

namespace Assets.Code.Tasker
{
    class InitializeBlocks : Task
    {
        private Chunk _chunk;
        private ChunkData _chunkData;

        public InitializeBlocks(Chunk chunk, ChunkData chunkData)
        {
            _chunk = chunk;
            _chunkData = chunkData;
        }
        public override IEnumerator Execute(Action taskdone)
        {
            yield return null;

            int i = 0;

            foreach (KeyValuePair<Position, StaticObject> worldObject in _chunkData.GetStaticObjects())
            {
                worldObject.Value.Chunk = _chunk;
                worldObject.Value.Position = worldObject.Key + _chunk.Position;

                if (++i % 20 == 0)
                    yield return null;
            }

            foreach (KeyValuePair<Position, DynamicObject> worldObject in _chunkData.GetDynamicObjects())
            {
                worldObject.Value.Chunk = _chunk;
                worldObject.Value.Position = worldObject.Key + _chunk.Position;

                if (++i % 20 == 0)
                    yield return null;
            }

            taskdone();
        }
    }
}
