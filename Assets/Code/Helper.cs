﻿using Assets.Code.GenerationEngine;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Dynamic;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code
{
    class Helper
    {
        public static Position SnapToGrid(Position origPosition)
        {
            Position playerChunk = new Position(
                origPosition.x / World.World.ChunkSize * World.World.ChunkSize,
                origPosition.y / World.World.ChunkSize * World.World.ChunkSize,
                origPosition.z / World.World.ChunkSize * World.World.ChunkSize
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

        public static Position GetBlockPos(Vector3 pos)
        {
            Position blockPosition = new Position(
                Mathf.RoundToInt(pos.x),
                Mathf.RoundToInt(pos.y),
                Mathf.RoundToInt(pos.z)
            );

            return blockPosition;
        }

        public static Position GetBlockPos(RaycastHit hit, bool adjacent = false)
        {
            Vector3 pos = new Vector3(
                MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
                MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
                MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
            );

            return GetBlockPos(pos);
        }

        static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
        {
            if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
            {
                if (adjacent)
                {
                    pos += (norm / 2);
                }
                else
                {
                    pos -= (norm / 2);
                }
            }

            return pos;
        }

        public static bool SetBlock(RaycastHit hit, StaticObject staticObject, bool adjacent = false)
        {
            ChunkComponent chunkComponent = hit.collider.GetComponent<ChunkComponent>();

            if (chunkComponent == null)
                return false;

            Position position = GetBlockPos(hit, adjacent);

            chunkComponent.World.SetObject(position, staticObject);

            return true;
        }

        public static MonoBehaviour GetMonoBehaviour(RaycastHit hit, bool adjacent = false)
        {
            if (hit.collider.CompareTag("Chunk"))
            {
                ChunkComponent chunkComponent = hit.collider.GetComponent<ChunkComponent>();

                return chunkComponent;
            }
            else if (hit.collider.CompareTag("DynamicObject"))
            {
                DynamicObjectComponent objectComponent = hit.collider.GetComponent<DynamicObjectComponent>();

                return objectComponent;
            }

            return null;
        }

    }
}
