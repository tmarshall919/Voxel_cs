using System;

using Microsoft.Xna.Framework;

namespace VoxLib.Blocks
{
	public struct Block
	{
		#region Members
		public EBlockType nType;
		public byte nSun;
		public byte R;
		public byte G;
		public byte B;
		#endregion

		#region Constructors
		public Block( EBlockType nType )
		{
			this.nType = nType;
			nSun = 16;
			R = G = B = 0;
		}
		public Block( EBlockType nType, byte nSun )
		{
			this.nType = nType;
			this.nSun = nSun;
			R = G = B = 0;
		}
		public Block( EBlockType nType, byte nSun, byte R, byte G, byte B )
		{
			this.nType = nType;
			this.nSun = nSun;
			this.R = R;
			this.G = G;
			this.B = B;
		}
		public Block( EBlockType nType, byte nSun, Color color )
		{
			this.nType = nType;
			this.nSun = nSun;
			this.R = color.R;
			this.G = color.G;
			this.B = color.B;
		}
		#endregion

		#region Helper Methods
		public static Block Empty
		{
			get{ return new Block(); }
		}
		public bool Exists
		{
			get{ return nType != EBlockType.Air; }
		}
		#endregion


		public static EBlockTexture GetTexture( EBlockType nType )
		{
			return GetTexture( nType, EBlockFaceDirection.ENDOFENUM, EBlockType.Air );
		}

		public static EBlockTexture GetTexture( EBlockType nType, EBlockFaceDirection nFaceDir )
		{
			return GetTexture( nType, nFaceDir, EBlockType.Air );
		}

		public static EBlockTexture GetTexture( EBlockType nBlockType, EBlockFaceDirection nDir, EBlockType nBlockAbove )
		{
			switch( nBlockType )
			{
				case EBlockType.Grass:
				{
					switch( nDir )
					{
						case EBlockFaceDirection.XIncreasing:
						case EBlockFaceDirection.XDecreasing:
						case EBlockFaceDirection.ZIncreasing:
						case EBlockFaceDirection.ZDecreasing:
							return EBlockTexture.GrassSide;
						case EBlockFaceDirection.YIncreasing:
							return EBlockTexture.GrassTop;
						case EBlockFaceDirection.YDecreasing:
							return EBlockTexture.Dirt;
						default:
							return EBlockTexture.Stone1;
					}
				}

				case EBlockType.Dirt:
					return EBlockTexture.Dirt;
				case EBlockType.Cobblestone:
					return EBlockTexture.Cobblestone;
				case EBlockType.Stone1:
					return EBlockTexture.Stone1;
				case EBlockType.Sand:
					return EBlockTexture.Sand;
				case EBlockType.WoodenPlank:
					return EBlockTexture.WoodenPlank;
				case EBlockType.Tree:
				{
					switch( nDir )
					{
						case EBlockFaceDirection.XIncreasing:
						case EBlockFaceDirection.XDecreasing:
						case EBlockFaceDirection.ZIncreasing:
						case EBlockFaceDirection.ZDecreasing:
							return EBlockTexture.TreeSide;
						case EBlockFaceDirection.YIncreasing:
						case EBlockFaceDirection.YDecreasing:
							return EBlockTexture.TreeTop;
						default:
							return EBlockTexture.Stone1;
					}
				}
				case EBlockType.RedFlower:
					return EBlockTexture.RedFlower;
				case EBlockType.TreeLeaves:
					return EBlockTexture.TreeLeaves;
				case EBlockType.LongGrass:
					return EBlockTexture.LongGrass;
	
				case EBlockType.Crack1:
					return EBlockTexture.Crack1;
				case EBlockType.Crack2:
					return EBlockTexture.Crack2;
				case EBlockType.Crack3:
					return EBlockTexture.Crack3;
				case EBlockType.Crack4:
					return EBlockTexture.Crack4;
				case EBlockType.Water:
					return EBlockTexture.Water;
				default:
					return EBlockTexture.Stone1;
		}
	}
	}
}
