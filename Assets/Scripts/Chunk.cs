using System.Collections;
using UnityEngine;

namespace Voxels
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        public bool update = false;
        public bool rendered;

        MeshFilter meshFilter;
        MeshCollider meshCollider;

        public WorldPos pos;
        public World world;
        public ChunkData data;

        void Start()
        {
            meshFilter = gameObject.GetComponent<MeshFilter>();
            meshCollider = gameObject.GetComponent<MeshCollider>();
        }

        void Update()
        {
            if (update)
            {
                update = false;
                UpdateChunk();
            }
        }

        public void Render()
        {
            update = true;
        }

        void UpdateChunk()
        {
            BuildMesh();
        }

        private void BuildMesh()
        {
            rendered = true;
            MeshData meshData = new MeshData(world.data);

            for (int y = 0; y < MapConstants.ChunkSize; y++)
            {
                for (int z = 0; z < MapConstants.ChunkSize; z++)
                {
                    for (int x = 0; x < MapConstants.ChunkSize; x++)
                    {
                        var block = data.GetBlock(x, y, z);
                        meshData.AddBlock(block, pos.x + x, pos.y + y, pos.z + z);
                    }
                }
            }

            // TOOD: ThreadNinja might help
            // yield return Ninja.JumpToUnity;
            RenderMesh(meshData);
        }

        // Sends the calculated mesh information to the mesh and collision components
        void RenderMesh(MeshData meshData)
        {
            meshFilter.mesh.Clear();
            meshFilter.mesh.vertices = meshData.vertices.ToArray();
            meshFilter.mesh.triangles = meshData.triangles.ToArray();

            meshFilter.mesh.uv = meshData.uv.ToArray();
            meshFilter.mesh.RecalculateNormals();

            meshCollider.sharedMesh = null;

            Mesh mesh = new Mesh();
            mesh.vertices = meshData.colliderVertices.ToArray();
            mesh.triangles = meshData.colliderTriangles.ToArray();
            mesh.RecalculateNormals();

            meshCollider.sharedMesh = mesh;
        }
    }
}
