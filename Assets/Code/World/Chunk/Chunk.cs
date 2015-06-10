using UnityEngine;

namespace Assets.Code.World.Chunk
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]

    public class Chunk : MonoBehaviour
    {
        public Block.Block[, ,] Blocks = new Block.Block[ChunkSize, ChunkSize, ChunkSize];
        public static int ChunkSize = 32;
        public bool update = false;

        public bool rendered;

        //Use this for initialization
        MeshFilter _filter;
        MeshCollider _coll;

        public World World;
        public WorldPos WorldPos;

        // Use this for initialization
        void Start()
        {
            _filter = gameObject.GetComponent<MeshFilter>();
            _coll = gameObject.GetComponent<MeshCollider>();
        }
        
        //Update is called once per frame
        void Update()
        {
            if (update)
            {
                update = false;
                UpdateChunk();
            }
        }

        void OnDestroy()
        {
            Serialization.SaveChunk(this);
        }

        public Block.Block GetBlock(int x, int y, int z)
        {
            if (InRange(x) && InRange(y) && InRange(z))
                return Blocks[x, y, z];
            return World.GetBlock(WorldPos.x + x, WorldPos.y + y, WorldPos.z + z);
        }

        public void SetBlock(int x, int y, int z, Block.Block block)
        {
            if (InRange(x) && InRange(y) && InRange(z))
            {
                Blocks[x, y, z] = block;
            }
            else
            {
                World.SetBlock(WorldPos.x + x, WorldPos.y + y, WorldPos.z + z, block);
            }
        }

        // Updates the chunk based on its contents
        void UpdateChunk()
        {
            rendered = true;

            MeshData meshData = new MeshData();

            for (int x = 0; x < ChunkSize; x++)
            {
                for (int y = 0; y < ChunkSize; y++)
                {
                    for (int z = 0; z < ChunkSize; z++)
                    {
                        meshData = Blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                    }
                }
            }

            RenderMesh(meshData);
        }
        // Sends the calculated mesh information
        // to the mesh and collision components
        void RenderMesh(MeshData meshData)
        {
            _filter.mesh.Clear();
            _filter.mesh.vertices = meshData.Vertices.ToArray();
            _filter.mesh.triangles = meshData.Triangles.ToArray();
            _filter.mesh.uv = meshData.Uv.ToArray();
            _filter.mesh.RecalculateNormals();

            _coll.sharedMesh = null;
            Mesh mesh = new Mesh();
            mesh.vertices = meshData.ColVertices.ToArray();
            mesh.triangles = meshData.ColTriangles.ToArray();
            mesh.RecalculateNormals();

            _coll.sharedMesh = mesh;
        }

        public static bool InRange(int index)
        {
            if (index < 0 || index >= ChunkSize)
                return false;

            return true;
        }

        public void SetBlocksUnmodified()
        {
            foreach (Block.Block block in Blocks)
            {
                block.changed = false;
            }
        }
    }
}