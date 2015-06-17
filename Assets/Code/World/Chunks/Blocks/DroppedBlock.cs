using UnityEngine;

namespace Assets.Code.World.Chunks.Blocks
{
    class DroppedBlock : MonoBehaviour
    {
        public Position Position;

        public Block OrigBlock;

        public float SelfDestroyTime = 300;

        void Start()
        {
            MeshFilter filter = GetComponent<MeshFilter>();

            MeshData meshData = new MeshData();

            meshData = OrigBlock.StandaloneBlockData(meshData);

            filter.mesh.Clear();
            filter.mesh.vertices = meshData.Vertices.ToArray();
            filter.mesh.triangles = meshData.Triangles.ToArray();
            filter.mesh.uv = meshData.Uv.ToArray();
            filter.mesh.RecalculateNormals();

            GetComponent<Rigidbody>().velocity = new Vector3(Random.value * 2 - 1, Random.value * 2 - 1, Random.value * 2 - 1);

            Destroy(this, SelfDestroyTime);
        }
    }
}
