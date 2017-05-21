using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Voxels
{
    public class MeshData
    {
        public List<Vector3> vertices;
        public List<int> triangles;
        public List<Vector2> uv;

        public List<Vector3> colliderVertices;
        public List<int> colliderTriangles;

        WorldData world;

        public MeshData(WorldData world)
        {
            this.vertices = new List<Vector3>();
            this.triangles = new List<int>();
            this.uv = new List<Vector2>();

            this.colliderVertices = new List<Vector3>();
            this.colliderTriangles = new List<int>();

            this.world = world;
        }

        public void Reset()
        {
            this.vertices.Clear();
            this.triangles.Clear();
            this.uv.Clear();

            this.colliderVertices.Clear();
            this.colliderTriangles.Clear();

            // this.world.Reset();
        }

        public void AddQuadTriangles()
        {
            this.triangles.Add(vertices.Count - 4);
            this.triangles.Add(vertices.Count - 3);
            this.triangles.Add(vertices.Count - 2);
            this.triangles.Add(vertices.Count - 4);
            this.triangles.Add(vertices.Count - 2);
            this.triangles.Add(vertices.Count - 1);

            this.colliderTriangles.Add(colliderVertices.Count - 4);
            this.colliderTriangles.Add(colliderVertices.Count - 3);
            this.colliderTriangles.Add(colliderVertices.Count - 2);
            this.colliderTriangles.Add(colliderVertices.Count - 4);
            this.colliderTriangles.Add(colliderVertices.Count - 2);
            this.colliderTriangles.Add(colliderVertices.Count - 1);
        }

        public void AddVertex(Vector3 vertex)
        {
            this.vertices.Add(vertex);
            this.colliderVertices.Add(vertex);
        }

        public void AddTriangle(int tri)
        {
            this.triangles.Add(tri);
            this.colliderTriangles.Add(tri - (vertices.Count - colliderVertices.Count));
        }

        private BlockInfo GetWorldBlockInfo(int x, int y, int z)
        {
            var block = world.GetBlock(x, y, z);

            return BlockData.Get(block);
        }

        public void AddBlock(BlockId block, int x, int y, int z)
        {
            if (block == BlockId.Air)
            {
                return;
            }

            var thisBlock = BlockData.Get(block);
            BlockInfo blockInfo;

            blockInfo = GetWorldBlockInfo(x, y + 1, z);
            if (!IsSolid(blockInfo, Direction.down))
            {
                AddFaceDataUp(thisBlock, x, y, z);
            }

            blockInfo = GetWorldBlockInfo(x, y - 1, z);
            if (!IsSolid(blockInfo, Direction.up))
            {
                AddFaceDataDown(thisBlock, x, y, z);
            }

            blockInfo = GetWorldBlockInfo(x, y, z + 1);
            if (!IsSolid(blockInfo, Direction.south))
            {
                AddFaceDataNorth(thisBlock, x, y, z);
            }

            blockInfo = GetWorldBlockInfo(x, y, z - 1);
            if (!IsSolid(blockInfo, Direction.north))
            {
                AddFaceDataSouth(thisBlock, x, y, z);
            }

            blockInfo = GetWorldBlockInfo(x + 1, y, z);
            if (!IsSolid(blockInfo, Direction.west))
            {
                AddFaceDataEast(thisBlock, x, y, z);
            }

            blockInfo = GetWorldBlockInfo(x - 1, y, z);
            if (!IsSolid(blockInfo, Direction.east))
            {
                AddFaceDataWest(thisBlock, x, y, z);
            }
        }

        public void AddFaceDataUp(BlockInfo blockInfo, int x, int y, int z)
        {
            AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            AddQuadTriangles();
            AddUVs(blockInfo, Direction.up);
        }

        public void AddFaceDataDown(BlockInfo blockInfo, int x, int y, int z)
        {
            AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            AddQuadTriangles();
            AddUVs(blockInfo, Direction.down);
        }

        public void AddFaceDataNorth(BlockInfo blockInfo, int x, int y, int z)
        {
            AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            AddQuadTriangles();
            AddUVs(blockInfo, Direction.north);
        }

        public void AddFaceDataEast(BlockInfo blockInfo, int x, int y, int z)
        {
            AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            AddQuadTriangles();
            AddUVs(blockInfo, Direction.east);
        }

        public void AddFaceDataSouth(BlockInfo blockInfo, int x, int y, int z)
        {
            AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            AddQuadTriangles();
            AddUVs(blockInfo, Direction.south);
        }

        public void AddFaceDataWest(BlockInfo blockInfo, int x, int y, int z)
        {
            AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            AddQuadTriangles();
            AddUVs(blockInfo, Direction.west);
        }

        public bool IsSolid(BlockInfo blockInfo, Direction direction)
        {
            return blockInfo.solid[(int)direction];
        }

        public void AddUVs(BlockInfo blockInfo, Direction direction)
        {
            var offset = (int)direction;
            Assert.AreEqual(true, offset >= 0);
            Assert.AreEqual(true, offset < 6);

            var blockUV = blockInfo.uvs[offset];
            var size = MapConstants.TileSize;

            uv.Add(new Vector2(size * blockUV.x + size, size * blockUV.y));
            uv.Add(new Vector2(size * blockUV.x + size, size * blockUV.y + size));
            uv.Add(new Vector2(size * blockUV.x, size * blockUV.y + size));
            uv.Add(new Vector2(size * blockUV.x, size * blockUV.y));
        }
    }

    public enum Direction
    {
        north,
        east,
        south,
        west,
        up,
        down
    };
}