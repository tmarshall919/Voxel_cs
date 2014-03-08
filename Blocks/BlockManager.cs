using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using VoxLib.Chunks;
using VoxLib.Common.Vector;
using VoxLib.Configurations;

namespace VoxLib.Blocks
{
	public static class BlockManager
	{
		#region Members
		public static Block[] Blocks;

		public static int ChunkCacheWidth = Configuration.ChunkConfig.ChunkCacheWidth;
		public static int ChunkCacheDepth = Configuration.ChunkConfig.ChunkCacheDepth;
		public static int AdvanceX = Configuration.ChunkConfig.ChunkCacheDepth * Configuration.ChunkConfig.ChunkHeight;
		public static int AdvanceZ = Configuration.ChunkConfig.ChunkHeight;

		public static int ArraySize = Configuration.ChunkConfig.ChunkCacheWidth * Configuration.ChunkConfig.ChunkCacheDepth * Configuration.ChunkConfig.ChunkHeight;
		#endregion

		static BlockManager()
		{
			Debug.WriteLine( "[BlockManager (ctor)] - Creating array. Size : {0}", ArraySize );

			Blocks = new Block[ArraySize];
			for( int i = 0; i < ArraySize; i++ )
				Blocks[i] = new Block( EBlockType.Air, 16 );
		}

		public static void RerollSettings()
		{
			ChunkCacheWidth = Configuration.ChunkConfig.ChunkCacheWidth;
			ChunkCacheDepth = Configuration.ChunkConfig.ChunkCacheDepth;
			AdvanceX = Configuration.ChunkConfig.ChunkCacheDepth * Configuration.ChunkConfig.ChunkHeight;
			AdvanceZ = Configuration.ChunkConfig.ChunkHeight;
			ArraySize = Configuration.ChunkConfig.ChunkCacheWidth * Configuration.ChunkConfig.ChunkCacheDepth * Configuration.ChunkConfig.ChunkHeight;

			//TODO : Block Array
		}

		#region Block GET Accessors
		public static Block BlockAt( int x, int y, int z )
		{
			if( !ChunkCache.IsInBounds( x, y, z ) )
				return Block.Empty;

			int nX = x % ChunkCacheWidth;
			int nZ = z % ChunkCacheDepth;

			if( nX < 0 )
				nX += ChunkCacheWidth;
			if( nZ < 0 )
				nZ += ChunkCacheDepth;

			int nIndex = nX * AdvanceX + nZ * AdvanceZ + y;
#if DEBUG
//			Debug.Assert( nIndex >= ArraySize || nIndex < 0 );
#endif
			return Blocks[nIndex];
		}
		public static Block BlockAt( Vector3 vecIndex )
		{
			return BlockAt( (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		public static Block BlockAt( Vector3i vecIndex )
		{
			return BlockAt( (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		public static Block FastBlockAt( int x, int y, int z )
		{
			int nX = x % ChunkCacheWidth;
			int nZ = z % ChunkCacheDepth;

			if( nX < 0 )
				nX += ChunkCacheWidth;
			if( nZ < 0 )
				nZ += ChunkCacheDepth;

			int nIndex = nX * AdvanceX + nZ * AdvanceZ + y;
#if DEBUG
			Debug.Assert( nIndex >= ArraySize || nIndex < 0 );
#endif
			return Blocks[nIndex];
		}
		public static Block FastBlockAt( Vector3 vecIndex )
		{
			return FastBlockAt( (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		public static Block FastBlockAt( Vector3i vecIndex )
		{
			return FastBlockAt( (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}

		public static EBlockType BlockTypeAt( int x, int y, int z )
		{
			if( !ChunkCache.IsInBounds( x, y, z ) )
				return EBlockType.Air;

			int nX = x % ChunkCacheWidth;
			int nZ = z % ChunkCacheDepth;

			if( nX < 0 )
				nX += ChunkCacheWidth;
			if( nZ < 0 )
				nZ += ChunkCacheDepth;

			int nIndex = nX * AdvanceX + nZ * AdvanceZ + y;
#if DEBUG
			Debug.Assert( nIndex >= ArraySize || nIndex < 0 );
#endif
			return Blocks[nIndex].nType;
		}
		public static EBlockType BlockTypeAt( Vector3 vecIndex )
		{
			return BlockTypeAt( (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		public static EBlockType BlockTypeAt( Vector3i vecIndex )
		{
			return BlockTypeAt( (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		public static EBlockType FastBlockTypeAt( int x, int y, int z )
		{
			int nX = x % ChunkCacheWidth;
			int nZ = z % ChunkCacheDepth;

			if( nX < 0 )
				nX += ChunkCacheWidth;
			if( nZ < 0 )
				nZ += ChunkCacheDepth;

			int nIndex = nX * AdvanceX + nZ * AdvanceZ + y;
#if DEBUG
			Debug.Assert( nIndex >= ArraySize || nIndex < 0 );
#endif
			return Blocks[nIndex].nType;
		}
		public static EBlockType FastBlockTypeAt( Vector3 vecIndex )
		{
			return FastBlockTypeAt( (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		public static EBlockType FastBlockTypeAt( Vector3i vecIndex )
		{
			return FastBlockTypeAt( (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		#endregion

		#region Block SET Accessors
		public static void SetBlock( Block block, int x, int y, int z )
		{
			if( !ChunkCache.IsInBounds( x, y, z ) )
				return;

			int nX = x % ChunkCacheWidth;
			int nZ = z % ChunkCacheDepth;

			if( nX < 0 )
				nX += ChunkCacheWidth;
			if( nZ < 0 )
				nZ += ChunkCacheDepth;

			int nIndex = nX * AdvanceX + nZ * AdvanceZ + y;
#if DEBUG
			Debug.Assert( nIndex >= ArraySize || nIndex < 0 );
#endif
			Blocks[nIndex] = block;
		}
		public static void SetBlock( Block block, Vector3 vecIndex )
		{
			SetBlock( block, (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		public static void SetBlock( Block block, Vector3i vecIndex )
		{
			SetBlock( block, (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		public static void FastSetBlock( Block block, int x, int y, int z )
		{
			int nX = x % ChunkCacheWidth;
			int nZ = z % ChunkCacheDepth;

			if( nX < 0 )
				nX += ChunkCacheWidth;
			if( nZ < 0 )
				nZ += ChunkCacheDepth;

			int nIndex = nX * AdvanceX + nZ * AdvanceZ + y;
#if DEBUG
			Debug.Assert( nIndex >= ArraySize || nIndex < 0 );
#endif
			Blocks[nIndex] = block;
		}
		public static void FastSetBlock( Block block, Vector3 vecIndex )
		{
			FastSetBlock( block, (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		public static void FastSetBlock( Block block, Vector3i vecIndex )
		{
			FastSetBlock( block, (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}

		public static void SetBlockType( EBlockType nType, int x, int y, int z )
		{
			if( !ChunkCache.IsInBounds( x, y, z ) )
				return;

			int nX = x % ChunkCacheWidth;
			int nZ = z % ChunkCacheDepth;

			if( nX < 0 )
				nX += ChunkCacheWidth;
			if( nZ < 0 )
				nZ += ChunkCacheDepth;

			int nIndex = nX * AdvanceX + nZ * AdvanceZ + y;
#if DEBUG
			Debug.Assert( nIndex >= ArraySize || nIndex < 0 );
#endif
			Blocks[nIndex].nType = nType;
		}
		public static void SetBlockType( EBlockType nType, Vector3 vecIndex )
		{
			SetBlockType( nType, (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		public static void SetBlockType( EBlockType nType, Vector3i vecIndex )
		{
			SetBlockType( nType, (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		public static void FastSetBlockType( EBlockType nType, int x, int y, int z )
		{
			int nX = x % ChunkCacheWidth;
			int nZ = z % ChunkCacheDepth;

			if( nX < 0 )
				nX += ChunkCacheWidth;
			if( nZ < 0 )
				nZ += ChunkCacheDepth;

			int nIndex = nX * AdvanceX + nZ * AdvanceZ + y;
#if DEBUG
			Debug.Assert( nIndex >= ArraySize || nIndex < 0 );
#endif
			Blocks[nIndex].nType = nType;
		}
		public static void FastSetBlockType( EBlockType nType, Vector3 vecIndex )
		{
			FastSetBlockType( nType, (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		public static void FastSetBlockType( EBlockType nType, Vector3i vecIndex )
		{
			FastSetBlockType( nType, (int)vecIndex.X, (int)vecIndex.Y, (int)vecIndex.Z );
		}
		#endregion

		public static int BlockIndexAtWorld( int x, int z )
		{
			int nX = x % ChunkCacheWidth;
			int nZ = z % ChunkCacheDepth;

			if( nX < 0 )
				nX += ChunkCacheWidth;
			if( nZ < 0 )
				nZ += ChunkCacheDepth;

			int nIndex = nX * AdvanceX + nZ * AdvanceZ;
#if DEBUG
		//	Debug.Assert( nIndex >= ArraySize || nIndex < 0 );
#endif
			return nIndex;
		}
		public static int BlockIndexAtWorld( int x, byte y, int z )
		{
			int nX = x % ChunkCacheWidth;
			int nZ = z % ChunkCacheDepth;

			if( nX < 0 )
				nX += ChunkCacheWidth;
			if( nZ < 0 )
				nZ += ChunkCacheDepth;

			int nIndex = nX * AdvanceX + nZ * AdvanceZ + y;
#if DEBUG
//			Debug.Assert( nIndex >= ArraySize || nIndex < 0 );
#endif
			return nIndex;
		}
		public static int BlockIndexAt( Chunk chunk, byte x, byte z )
		{
			int nIndexX = chunk.WorldPosition.X + x;
			int nIndexZ = chunk.WorldPosition.Z + z;
			int nX = nIndexX % ChunkCacheWidth;
			int nZ = nIndexZ % ChunkCacheDepth;

			if( nX < 0 )
				nX += ChunkCacheWidth;
			if( nZ < 0 )
				nZ += ChunkCacheDepth;

			int nIndex = nX * AdvanceX + nZ * AdvanceZ;
#if DEBUG
//			Debug.Assert( nIndex >= ArraySize || nIndex < 0 );
#endif
			return nIndex;
		}
		public static int BlockIndexAt( Chunk chunk, byte x, byte y, byte z )
		{
			int nIndexX = chunk.WorldPosition.X + x;
			int nIndexZ = chunk.WorldPosition.Z + z;
			int nX = nIndexX % ChunkCacheWidth;
			int nZ = nIndexZ % ChunkCacheDepth;

			if( nX < 0 )
				nX += ChunkCacheWidth;
			if( nZ < 0 )
				nZ += ChunkCacheDepth;

			int nIndex = nX * AdvanceX + nZ * AdvanceZ + y;
#if DEBUG
			Debug.Assert( nIndex >= ArraySize || nIndex < 0 );
#endif
			return nIndex;
		}
	}
}
