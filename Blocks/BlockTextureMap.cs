using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace VoxLib.Blocks
{
	public static class BlockTextureMap
	{
		public static int BlockTextureAtlasSize = 8;
		public static float BlockTextureOffset = 1f / BlockTextureAtlasSize;
		public static readonly Dictionary<int, Vector2[]> BlockTextures = new Dictionary<int,Vector2[]>();

		static BlockTextureMap()
		{
			BuildMap();
		}

		private static void BuildMap()
		{
			for( int i = 0; i <= (int)EBlockTexture.ENDOFENUM; i++ )
			{
				BlockTextures.Add( (i * 6), GetBlockTexture(i, EBlockFaceDirection.XIncreasing) );
				BlockTextures.Add( (i * 6) + 1, GetBlockTexture(i, EBlockFaceDirection.XDecreasing) );
				BlockTextures.Add( (i * 6) + 2, GetBlockTexture(i, EBlockFaceDirection.YIncreasing) );
				BlockTextures.Add( (i * 6) + 3, GetBlockTexture(i, EBlockFaceDirection.YDecreasing) );
				BlockTextures.Add( (i * 6) + 4, GetBlockTexture(i, EBlockFaceDirection.ZIncreasing) );
				BlockTextures.Add( (i * 6) + 5, GetBlockTexture(i, EBlockFaceDirection.ZDecreasing) );
			}
		}

		public static Vector2[] GetBlockTexture( int nIndex, EBlockFaceDirection nFaceDir )
		{
			int nTexIndex = nIndex;
			int x = nIndex % 16;
			int y = nIndex / 16;
			float fOffset = 1f / ((float)16);
			float fOffsetX = x * fOffset;
			float fOffsetY = y * fOffset;
			Vector2[] vecUVList = new Vector2[6];

			switch( nFaceDir )
			{
				case EBlockFaceDirection.XIncreasing:
					vecUVList[0] = new Vector2(fOffsetX, fOffsetY);
					vecUVList[1] = new Vector2(fOffsetX + fOffset, fOffsetY);
					vecUVList[2] = new Vector2(fOffsetX, fOffsetY + fOffset);
					vecUVList[3] = new Vector2(fOffsetX, fOffsetY + fOffset);
					vecUVList[4] = new Vector2(fOffsetX + fOffset, fOffsetY);
					vecUVList[5] = new Vector2(fOffsetX + fOffset, fOffsetY + fOffset);
					break;
				case EBlockFaceDirection.XDecreasing:
					vecUVList[0] = new Vector2(fOffsetX, fOffsetY);
					vecUVList[1] = new Vector2(fOffsetX + fOffset, fOffsetY);
					vecUVList[2] = new Vector2(fOffsetX + fOffset, fOffsetY + fOffset);
					vecUVList[3] = new Vector2(fOffsetX, fOffsetY);
					vecUVList[4] = new Vector2(fOffsetX + fOffset, fOffsetY + fOffset);
					vecUVList[5] = new Vector2(fOffsetX, fOffsetY + fOffset);
					break;
				case EBlockFaceDirection.YIncreasing:
					vecUVList[0] = new Vector2(fOffsetX, fOffsetY + fOffset);
					vecUVList[1] = new Vector2(fOffsetX, fOffsetY);
					vecUVList[2] = new Vector2(fOffsetX + fOffset, fOffsetY);
					vecUVList[3] = new Vector2(fOffsetX, fOffsetY + fOffset);
					vecUVList[4] = new Vector2(fOffsetX + fOffset, fOffsetY);
					vecUVList[5] = new Vector2(fOffsetX + fOffset, fOffsetY + fOffset);
					break;
				case EBlockFaceDirection.YDecreasing:
					vecUVList[0] = new Vector2(fOffsetX, fOffsetY);
					vecUVList[1] = new Vector2(fOffsetX + fOffset, fOffsetY);
					vecUVList[2] = new Vector2(fOffsetX, fOffsetY + fOffset);
					vecUVList[3] = new Vector2(fOffsetX, fOffsetY + fOffset);
					vecUVList[4] = new Vector2(fOffsetX + fOffset, fOffsetY);
					vecUVList[5] = new Vector2(fOffsetX + fOffset, fOffsetY + fOffset);
					break;
				case EBlockFaceDirection.ZIncreasing:
					vecUVList[0] = new Vector2(fOffsetX, fOffsetY);
					vecUVList[1] = new Vector2(fOffsetX + fOffset, fOffsetY);
					vecUVList[2] = new Vector2(fOffsetX + fOffset, fOffsetY + fOffset);
					vecUVList[3] = new Vector2(fOffsetX, fOffsetY);
					vecUVList[4] = new Vector2(fOffsetX + fOffset, fOffsetY + fOffset);
					vecUVList[5] = new Vector2(fOffsetX, fOffsetY + fOffset);
					break;
				case EBlockFaceDirection.ZDecreasing:
					vecUVList[0] = new Vector2(fOffsetX, fOffsetY);
					vecUVList[1] = new Vector2(fOffsetX + fOffset, fOffsetY);
					vecUVList[2] = new Vector2(fOffsetX, fOffsetY + fOffset);
					vecUVList[3] = new Vector2(fOffsetX, fOffsetY + fOffset);
					vecUVList[4] = new Vector2(fOffsetX + fOffset, fOffsetY);
					vecUVList[5] = new Vector2(fOffsetX + fOffset, fOffsetY + fOffset);
					break;
			}

			return vecUVList;
        }
	}
}
