using Assets.Code.World.Chunks;
using Assets.Code.World.Chunks.Blocks;
using UnityEngine;

namespace Assets.Code.World.Terrain
{
    public static class TerrainHelper
    {
        public static WorldPosition GetBlockPos(Vector3 pos)
        {
            WorldPosition blockPosition = new WorldPosition(
                Mathf.RoundToInt(pos.x),
                Mathf.RoundToInt(pos.y),
                Mathf.RoundToInt(pos.z)
            );

            return blockPosition;
        }

        public static WorldPosition GetBlockPos(RaycastHit hit, bool adjacent = false)
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

        public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
        {
            Chunk chunk = hit.collider.GetComponent<Chunk>();

            if (chunk == null)
                return false;

            WorldPosition position = GetBlockPos(hit, adjacent);
            
            chunk.World.SetBlock(position, block);

            return true;
        }
        public static Block GetBlock(RaycastHit hit, bool adjacent = false)
        {
            Chunk chunk = hit.collider.GetComponent<Chunk>();
            if (chunk == null)
                return null;

            WorldPosition position = GetBlockPos(hit, adjacent);

            Block block = chunk.World.GetBlock(position);

            return block;
        }
    }
}