using System;
using UnityEngine;

namespace Assets.Code.World
{
    [Serializable]
    public class Position
    {
        public int x;
        public int y;
        public int z;

        public Position()
        {
            
        }

        public Position(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Position(Position position)
            : this(position.x, position.y, position.z)
        {
        }

        public Position(Vector3 position)
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

        public static Position operator +(Position pos1, Position pos2)
        {
            Position newPosition = new Position();

            newPosition.x = pos1.x + pos2.x;
            newPosition.y = pos1.y + pos2.y;
            newPosition.z = pos1.z + pos2.z;

            return newPosition;
        }

        public static Position operator -(Position pos1, Position pos2)
        {
            Position newPosition = new Position();

            newPosition.x = pos1.x - pos2.x;
            newPosition.y = pos1.y - pos2.y;
            newPosition.z = pos1.z - pos2.z;

            return newPosition;
        }

        public static Position operator /(Position pos, Position pos2)
        {
            Position newPosition = new Position();

            newPosition.x = pos.x / pos2.x;
            newPosition.y = pos.y / pos2.y;
            newPosition.z = pos.z / pos2.z;

            return newPosition;
        }

        public static Position operator /(Position pos, int divider)
        {
            Position newPosition = new Position();

            newPosition.x = pos.x / divider;
            newPosition.y = pos.y / divider;
            newPosition.z = pos.z / divider;

            return newPosition;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public static int ManhattanDistance(Position pos1, Position pos2)
        {
            return Math.Max(Math.Abs(pos1.x - pos2.x), Math.Max(Math.Abs(pos1.y - pos2.y), (Math.Abs(pos1.z - pos2.z))));
        }

        public int ManhattanDistance(Position pos)
        {
            return ManhattanDistance(this, pos);
        }

        public override string ToString()
        {
            return "Position: " + x + "," + y + "," + z;
        }
    }
}