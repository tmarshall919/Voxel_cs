using System;

using Microsoft.Xna.Framework;

namespace VoxLib.Common.Vector
{
	public struct ZVector2i
	{
		public readonly int X;
		public readonly int Z;

		public ZVector2i( int x, int z )
		{
			X = x;
			Z = z;
		}

		public ZVector2i( Vector2 vec )
		{
			X = (int)vec.X;
			Z = (int)vec.Y;
		}

		public override bool Equals( object obj )
		{
			if( obj is ZVector2i )
			{
				ZVector2i vec = (ZVector2i)obj;

				return X == vec.X && Z == vec.Z;
			}

			return base.Equals( obj );
		}

		public static bool operator ==( ZVector2i a, ZVector2i b )
		{
			return a.X == b.X && a.Z == b.Z;
		}

		public static bool operator !=( ZVector2i a, ZVector2i b )
		{
			return !(a.X == b.X && a.Z == b.Z);
		}

		public static Vector2i operator +( ZVector2i a, ZVector2i b )
		{
			return new Vector2i( a.X + b.X, a.Z + b.Z) ;
		}

		public static int DistanceSquared( ZVector2i a, ZVector2i b )
		{
			int x = a.X - b.X;
			int y = a.Z - b.Z;

			return (x * x) + (y * y);
		}

		public override int GetHashCode()
		{         
			return (int)(X ^ Z);
		}

		public override string ToString()
		{
			return ("ZVector2i (" + X + "," + Z + ")");
		}

		public Vector2 AsVector2()
		{
			return new Vector2(X, Z);
		}
	}
}
