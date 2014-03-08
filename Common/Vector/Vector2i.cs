using System;

using Microsoft.Xna.Framework;

namespace VoxLib.Common.Vector
{
	public struct Vector2i
	{
		public readonly int X;
		public readonly int Y;

		public Vector2i( int x, int y )
		{
			X = x;
			Y = y;
		}

		public Vector2i( Vector2 vec )
		{
			X = (int)vec.X;
			Y = (int)vec.Y;
		}

		public override bool Equals( object obj )
		{
			if( obj is Vector2i )
			{
				Vector2i vec = (Vector2i)obj;

				return X == vec.X && Y == vec.Y;
			}

			return base.Equals( obj );
		}

		public static bool operator ==( Vector2i a, Vector2i b )
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator !=( Vector2i a, Vector2i b )
		{
			return !(a.X == b.X && a.Y == b.Y);
		}

		public static Vector2i operator +( Vector2i a, Vector2i b )
		{
			return new Vector2i( a.X + b.X, a.Y + b.Y) ;
		}

		public static int DistanceSquared( Vector2i a, Vector2i b )
		{
			int x = a.X - b.X;
			int y = a.Y - b.Y;

			return (x * x) + (y * y);
		}

		public override int GetHashCode()
		{         
			return (int)(X ^ Y);
		}

		public override string ToString()
		{
			return ("Vector2i (" + X + "," + Y + ")");
		}

		public Vector2 AsVector2()
		{
			return new Vector2(X, Y);
		}
	}
}
