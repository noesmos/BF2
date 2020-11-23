using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    public struct HexCoordinates
    {
        public int X { get; private set; } 
        public int Y { get; private set; } 
        public int Z { get; private set; }

        private HexCoordinates(int x, int z) //constructor used in fromOffsetCoordinate when you have the gridID of a tile
        {
            X = x; 
            Z = z;
            Y = - x - z;
        }

        public HexCoordinates(Vector3 coord) // constructor to use when you have the hexcoords
        {
            X = (int)coord.x;
            Y = (int)coord.y;
            Z = (int)coord.z;
        }
        public HexCoordinates GetNeighBourHexCoordinates(Vector3 offset)
        {
            return new HexCoordinates(new Vector3(X + offset.x, Y + offset.y, Z + offset.z));
        }

        public static HexCoordinates FromOffsetCoordinate(int x, int z)
        {
            return new HexCoordinates(x, z - x / 2);
        }

        public override string ToString()
        {
            return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
        }

        public static int ManhattanDistance(HexCoordinates from, HexCoordinates to)
        {
            return (Mathf.Abs(from.X - to.X) + Mathf.Abs(from.Y - to.Y) + Mathf.Abs(from.Z - to.Z))/2;
        }



        public static Vector3 HexDifference(HexCoordinates from, HexCoordinates to)
        {
            return new Vector3(from.X - to.X, from.Y - to.Y, from.Z - to.Z);
        }

        public static bool operator == (HexCoordinates a, HexCoordinates b)
        {
            return a.Equals(b);
        }

        public static bool operator != (HexCoordinates a, HexCoordinates b)
        {
            return !a.Equals(b);
        }
    }
}

