using Assets.Code.World;
using Assets.Code.World.Chunk.Block;
using UnityEngine;

namespace Assets.Code.Player
{
    public class Builder : MonoBehaviour
    {
        Vector2 rot;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 100))
                {
                    TerrainHelper.SetBlock(hit, new BlockAir());
                }
            }

        }
    }
}
