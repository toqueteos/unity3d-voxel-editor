using UnityEngine;
using System;

namespace Voxels
{
    [Serializable]
    public struct WorldPos
    {
        public int x, y, z;

        public WorldPos(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 47;
                hash = hash * 227 + x.GetHashCode();
                hash = hash * 227 + y.GetHashCode();
                hash = hash * 227 + z.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WorldPos))
            {
                return false;
            }

            return x == ((WorldPos)obj).x && y == ((WorldPos)obj).y && z == ((WorldPos)obj).z;
        }
    }
}