﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Code.GenerationEngine;
using Assets.Code.World;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Assets.Code.Player
{
    class ChunkLoader : MonoBehaviour
    {
        public World.World World;

        public GameObject Player;

        private static List<Position> _chunkPositions = new List<Position>();
        
        void Start()
        {
            int range = (int)Math.Floor((float)(Code.World.World.ViewingRange/Code.World.World.ChunkSize));

            for (int x = -range; x < range; x++)
                for (int y = -range / 2; y < range / 2; y++)
                    for (int z = -range; z < range; z++)
                        _chunkPositions.Add(new Position(x * Code.World.World.ChunkSize, y * Code.World.World.ChunkSize, z * Code.World.World.ChunkSize));   

            _chunkPositions = _chunkPositions.OrderBy(w => w.ToVector3().magnitude).ToList();

            StartCoroutine("GetChunksFromGenerationEngine");
            StartCoroutine("DeleteChunks");
        }
        
        void Update()
        {
            if (Code.World.World.Generator == null)
                return;

            Code.World.World.Generator.SetPlayerPosition(new Position(Player.transform.position));
        }

        IEnumerator GetChunksFromGenerationEngine()
        {
            for (;;)
            {
                if (Code.World.World.Generator == null)
                    yield return new WaitForSeconds(Random.value);

                int i = 0;

                foreach (KeyValuePair<Position, ChunkData> chunk in Code.World.World.Generator.GetNewChunks())
                {
                    World.CreateNewChunkPrefab(chunk);
                }

                yield return new WaitForSeconds(Random.value);
            }
        }
        
        IEnumerator DeleteChunks()
        {
            for (;;)
            {
                int j = 0;

                foreach (Position position in Code.World.World.Generator.GetOutOfRangeChunks())
                {
                    World.DestroyChunk(position);
                }

                yield return new WaitForSeconds(Random.value);
            }
        }
    }
}
