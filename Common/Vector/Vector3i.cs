using System;

using Microsoft.Xna.Framework;

namespace VoxLib.Common.Vector
{
	public struct Vector3i
	{
		public readonly int X;
		public readonly int Y;
		public readonly int Z;

		public Vector3i( int x, int y, int z )
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector3i( Vector3 vec )
		{
			X = (int)vec.X;
			Y = (int)vec.Y;
			Z = (int)vec.Z;
		}

		public override bool Equals( object obj )
		{
			if( obj is Vector3i )
			{
				Vector3i a = (Vector3i)obj;

				return X == a.X && Y == a.Y && Z == a.Z;
			}

			return base.Equals( obj );
		}

		public static bool operator ==( Vector3i a, Vector3i b )
		{
			return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
		}

		public static bool operator !=( Vector3i a, Vector3i b )
		{
			return !(a.X == b.X && a.Y == b.Y && a.Z == b.Z);
		}

		public static Vector3i operator +( Vector3i a, Vector3i b )
		{
			return new Vector3i(a.X + b.X ,a.Y +b.Y ,a.Z + b.Z);
		}

		public static int DistanceSquared( Vector3i a, Vector3i b )
		{
			int x = a.X - b.X;
			int y = a.Y - b.Y;
			int z = a.Z - b.Z;

			return (x * x) + (y * y) + (z * z);
		}

		public override int GetHashCode()
		{
			return (int)(X ^ Y ^ Z);
		}

		public override string ToString()
		{
			return ("vector3i (" + X + "," + Y + "," + Z + ")");
		}

		public Vector3 AsVector3()
		{
			return new Vector3(X, Y, Z);
		}
	}
}
