using System;
using UnityEngine;

namespace Assets.Code.World
{
    [Serializable]
    public struct WorldPosition
    {
        public int x;
        public int y;
        public int z;

        public WorldPosition(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public WorldPosition(WorldPosition position)
            : this(position.x, position.y, position.z)
        {
        }

        public WorldPosition(Vector3 position)
            : this((int)position.x, (int)position.y, (int)position.z)
        {
        }

        public override bool Equals(object obj)
        {
            if (GetHashCode() == obj.GetHashCode())
                return true;

            return false;
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

        public static WorldPosition operator +(WorldPosition pos1, WorldPosition pos2)
        {
            WorldPosition newPosition = new WorldPosition();

            newPosition.x = pos1.x + pos2.x;
            newPosition.y = pos1.y + pos2.y;
            newPosition.z = pos1.z + pos2.z;

            return newPosition;
        }

        public static WorldPosition operator -(WorldPosition pos1, WorldPosition pos2)
        {
            WorldPosition newPosition = new WorldPosition();

            newPosition.x = pos1.x - pos2.x;
            newPosition.y = pos1.y - pos2.y;
            newPosition.z = pos1.z - pos2.z;

            return newPosition;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public static int ManhattanDistance(WorldPosition pos1, WorldPosition pos2)
        {
            return Math.Max(Math.Abs(pos1.x - pos2.x), Math.Max(Math.Abs(pos1.y - pos2.y), (Math.Abs(pos1.z - pos2.z))));
        }

        public int ManhattanDistance(WorldPosition pos)
        {
            return ManhattanDistance(this, pos);
        }

        public override string ToString()
        {
            return "WorldPosition: " + x + "," + y + "," + z;
        }
    }
}