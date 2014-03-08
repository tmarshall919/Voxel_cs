using System;

using Microsoft.Xna.Framework;

namespace VoxLib.Common.Vector
{
	public struct Vector2b
	{
		public readonly byte X;
		public readonly byte Y;

		public Vector2b( byte x, byte y )
		{
			X = x;
			Y = y;
		}

		public Vector2b( Vector2 vec )
		{
			X = (byte)vec.X;
			Y = (byte)vec.Y;
		}

		public override bool Equals( object obj )
		{
			if( obj is Vector2b )
			{
				Vector2b vec = (Vector2b)obj;

				return X == vec.X && Y == vec.Y;
			}

			return base.Equals( obj );
		}

		public static bool operator ==( Vector2b a, Vector2b b )
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator !=( Vector2b a, Vector2b b )
		{
			return !(a.X == b.X && a.Y == b.Y);
		}

		public static Vector2b operator +( Vector2b a, Vector2b b )
		{
			return new Vector2b( (byte)(a.X + b.X), (byte)(a.Y + b.Y) );
		}

		public static int DistanceSquared( Vector2b a, Vector2b b )
		{
			int x = (int)(a.X - b.X);
			int y = (int)(a.Y - b.Y);

			return (x * x) + (y * y);
		}

		public override int GetHashCode()
		{         
			return (int)(X ^ Y);
		}

		public override string ToString()
		{
			return ("Vector2b (" + X + "," + Y + ")");
		}

		public Vector2 AsVector2()
		{
			return new Vector2( X, Y );
		}
	}
}
