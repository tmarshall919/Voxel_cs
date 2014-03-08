using System;

using Microsoft.Xna.Framework;

namespace VoxLib.Common.Vector
{
	public struct Vector3b
	{
		public readonly byte X;
		public readonly byte Y;
		public readonly byte Z;

		public Vector3b( byte x, byte y, byte z )
		{
			X = x;
			Y = y;
			Z = z;
		}

		public override bool Equals( object obj )
		{
			if( obj is Vector3b )
			{
				Vector3b a = (Vector3b)obj;
				return X == a.X && Y == a.Y && Z == a.Z;
			}

			return base.Equals( obj );
		}

		public static bool operator ==( Vector3b a, Vector3b b )
		{
			return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
		}

		public static bool operator !=( Vector3b a, Vector3b b )
		{
			return !(a.X == b.X && a.Y == b.Y && a.Z == b.Z);
		}

		public static Vector3b operator +( Vector3b a, Vector3b b )
		{
			return new Vector3b( (byte)(a.X + b.X), (byte)(a.Y + b.Y), (byte)(a.Z + b.Z) );
		}
       
		public override int GetHashCode()
		{
			return (int)(X ^ Y ^ Z);
		}

		public override string ToString()
		{
			return ("Vector3b (" + X + "," + Y + "," + Z + ")");
		}
	}
}
