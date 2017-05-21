using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Voxels
{
    public enum BlockId
    {
        Air,
        Error,
        Cobblestone,
        Stone,
        Dirt,
        Grass,
        Sand,
    }

    public static class BlockData
    {
        public static BlockInfo[] Blocks = new BlockInfo[]{
            new BlockInfo(BlockId.Air, BlockInfo.NotSolid(), BlockInfo.SameUVs(0, 0)),
            new BlockInfo(BlockId.Error, BlockInfo.Solid(), BlockInfo.SameUVs(3, 3)),
            new BlockInfo(BlockId.Cobblestone, BlockInfo.Solid(), BlockInfo.SameUVs(0, 0)),
            new BlockInfo(BlockId.Stone, BlockInfo.Solid(), BlockInfo.SameUVs(0, 0)),
            new BlockInfo(BlockId.Dirt, BlockInfo.Solid(), BlockInfo.SameUVs(1, 0)),
            new BlockInfo(BlockId.Grass, BlockInfo.Solid(), BlockInfo.UVs(3, 0, 3, 0, 3, 0, 3, 0, 2, 0, 1, 0)),
            new BlockInfo(BlockId.Sand, BlockInfo.Solid(), BlockInfo.SameUVs(3, 1)),
        };

        public static BlockInfo Get(BlockId block)
        {
            return Blocks[(int)block];
        }

        public static BlockId Get(int block)
        {
            if (block < 0 || block >= Blocks.Length)
            {
                return BlockId.Error;
            }

            return Blocks[block].id;
        }
    }

    public struct BlockInfo
    {
        public BlockId id;
        public bool[] solid;
        public Vector2[] uvs;

        public BlockInfo(BlockId id, bool[] solid, Vector2[] uvs)
        {
            this.id = id;
            this.solid = solid;
            this.uvs = uvs;
        }

        public static bool[] SameSolid(bool solid)
        {
            return new bool[6] { solid, solid, solid, solid, solid, solid };
        }

        public static bool[] NotSolid()
        {
            return SameSolid(false);
        }

        public static bool[] Solid()
        {
            return SameSolid(true);
        }

        public static bool[] Solid(params bool[] solid)
        {
            Assert.AreEqual(12, solid.Length);

            return solid;
        }

        public static Vector2[] SameUVs(int x, int y)
        {
            var uvs = new Vector2[12];
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(x, y);
            }
            return uvs;
        }

        public static Vector2[] UVs(params int[] pos)
        {
            Assert.AreEqual(12, pos.Length);

            var uvs = new Vector2[6];
            for (int i = 0; i < uvs.Length; i++)
            {
                int p = i * 2;
                uvs[i] = new Vector2(pos[p], pos[p + 1]);
            }

            return uvs;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("BlockInfo{");

            sb.AppendFormat("id={0}", id);

            sb.AppendFormat(", solid=[ ", id);
            foreach (var s in solid)
            {
                sb.AppendFormat("{0} ", s);
            }
            sb.Append("]");

            sb.AppendFormat(", uvs=[ ", id);
            foreach (var uv in this.uvs)
            {
                sb.AppendFormat("{0} ", uv);
            }
            sb.Append("]");

            sb.Append("}");
            return sb.ToString();
        }
    }
}