using System;

using Microsoft.Xna.Framework;

namespace VoxLib.Common.Vector
{
	public struct ZVector2b
	{
		public readonly byte X;
		public readonly byte Z;

		public ZVector2b( byte x, byte z )
		{
			X = x;
			Z = z;
		}

		public ZVector2b( Vector2 vec )
		{
			X = (byte)vec.X;
			Z = (byte)vec.Y;
		}

		public override bool Equals( object obj )
		{
			if( obj is ZVector2b )
			{
				ZVector2b vec = (ZVector2b)obj;

				return X == vec.X && Z == vec.Z;
			}

			return base.Equals( obj );
		}

		public static bool operator ==( ZVector2b a, ZVector2b b )
		{
			return a.X == b.X && a.Z == b.Z;
		}

		public static bool operator !=( ZVector2b a, ZVector2b b )
		{
			return !(a.X == b.X && a.Z == b.Z);
		}

		public static Vector2b operator +( ZVector2b a, ZVector2b b )
		{
			return new Vector2b( (byte)(a.X + b.X), (byte)(a.Z + b.Z) );
		}

		public static int DistanceSquared( ZVector2b a, ZVector2b b )
		{
			int x = (int)(a.X - b.X);
			int y = (int)(a.Z - b.Z);

			return (x * x) + (y * y);
		}

		public override int GetHashCode()
		{         
			return (int)(X ^ Z);
		}

		public override string ToString()
		{
			return ("ZVector2b (" + X + "," + Z + ")");
		}

		public Vector2 AsVector2()
		{
			return new Vector2( X, Z );
		}
	}
}
