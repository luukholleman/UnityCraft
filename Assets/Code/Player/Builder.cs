using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.World.Chunks.Blocks;
using Assets.Code.World.Terrain;
using UnityEngine;

namespace Assets.Code.Player
{
    public class Builder : MonoBehaviour
    {
        Vector2 rot;

        void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 100, LayerMask.GetMask("Chunks")))
                {
                    Block block = TerrainHelper.GetBlock(hit);
                    WorldPosition pos = TerrainHelper.GetBlockPos(hit);

                    TerrainHelper.SetBlock(hit, new BlockAir());

                    GameObject floatingBlock = Instantiate(Resources.Load<GameObject>("Prefabs/DroppedBlock"), pos.ToVector3(), new Quaternion()) as GameObject;

                    floatingBlock.GetComponent<DroppedBlock>().Position = pos;
                    floatingBlock.GetComponent<DroppedBlock>().OrigBlock = block;
                }
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 100))
                {
                    TerrainHelper.SetBlock(hit, new Block(), true);
                }
            }

        }
    }
}
