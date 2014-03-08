using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VoxLib.Blocks;
using VoxLib.Common.Extensions;
using VoxLib.Common.Vector;
using VoxLib.Configurations;

namespace VoxLib.Chunks
{
	public class Chunk
	{
		#region Members
		public static byte ChunkWidth = Configuration.ChunkConfig.ChunkWidth;
		public static byte ChunkHeight = Configuration.ChunkConfig.ChunkHeight;
		public static byte ChunkDepth = Configuration.ChunkConfig.ChunkDepth;
		public static byte ChunkMaxHeight = Configuration.ChunkConfig.ChunkMaxHeight;

		public ZVector2i WorldPosition	{ get; private set; }
		public ZVector2i Position		{ get; private set; }
		public BoundingBox BoundingBox	{ get; set; }
		public byte LowestEmptyBlock	{ get; set; }
		public byte HighestSolidBlock	{ get; set; }

		public EChunkState State		{ get; set; }

		public VertexBuffer VertexBuffer { get; set; }
		public IndexBuffer	IndexBuffer { get; set; }
		public List<BlockVertexPositionTextureLight>	VertexList;
		public List<short>		IndexList;
		public short nIndex;
		#endregion

		public Chunk( ZVector2i vecPosition )
		{
			State = EChunkState.AwaitingGeneration;
			Position = vecPosition;

			WorldPosition = new ZVector2i( vecPosition.X * ChunkWidth, vecPosition.Z * ChunkDepth );
			BoundingBox = new BoundingBox(
					new Vector3(WorldPosition.X, 0, WorldPosition.Z),
					new Vector3(WorldPosition.X + ChunkWidth, ChunkHeight, WorldPosition.Z + ChunkDepth) );

			VertexList = new List<BlockVertexPositionTextureLight>();
			IndexList = new List<short>();

			LowestEmptyBlock = Chunk.ChunkMaxHeight;
			HighestSolidBlock = 0;
		}

		public static void RerollSettings()
		{
			Chunk.ChunkWidth = Configuration.ChunkConfig.ChunkWidth;
			Chunk.ChunkHeight = Configuration.ChunkConfig.ChunkHeight;
			Chunk.ChunkDepth = Configuration.ChunkConfig.ChunkDepth;
			Chunk.ChunkMaxHeight = Configuration.ChunkConfig.ChunkMaxHeight;
		}

		public bool IsInBounds( float x, float z )
		{
			if( x < BoundingBox.Min.X || z < BoundingBox.Min.Z ||
				x >= BoundingBox.Max.X || z >= BoundingBox.Max.Z )
			{
				return false;
			}

			return true;
		}

		#region Block Accessors
		public Block BlockAt( int x, int y, int z )
		{
			return BlockManager.BlockAt( WorldPosition.X + x, y, WorldPosition.Z + z );
		}
		public Block BlockAt( Vector3 vecIndex )
		{
			return BlockManager.BlockAt( vecIndex );
		}
		public Block BlockAt( Vector3i vecIndex )
		{
			return BlockManager.BlockAt( vecIndex );
		}
		public Block FastBlockAt( int x, int y, int z )
		{
			return BlockManager.FastBlockAt( WorldPosition.X + x, y, WorldPosition.Z + z );
		}
		public Block FastBlockAt( Vector3 vecIndex )
		{
			return BlockManager.FastBlockAt( vecIndex );
		}
		public Block FastBlockAt( Vector3i vecIndex )
		{
			return BlockManager.FastBlockAt( vecIndex );
		}
		public EBlockType BlockTypeAt( int x, int y, int z )
		{
			return BlockManager.BlockTypeAt( WorldPosition.X + x, y, WorldPosition.Z + z );
		}
		public EBlockType BlockTypeAt( Vector3 vecIndex )
		{
			return BlockManager.BlockTypeAt( vecIndex );
		}
		public EBlockType BlockTypeAt( Vector3i vecIndex )
		{
			return BlockManager.BlockTypeAt( vecIndex );
		}
		public EBlockType FastBlockTypeAt( int x, int y, int z )
		{
			return BlockManager.FastBlockTypeAt( WorldPosition.X + x, y, WorldPosition.Z + z );
		}
		public EBlockType FastBlockTypeAt( Vector3 vecIndex )
		{
			return BlockManager.FastBlockTypeAt( vecIndex );
		}
		public EBlockType FastBlockTypeAt( Vector3i vecIndex )
		{
			return BlockManager.FastBlockTypeAt( vecIndex );
		}
		public void SetBlock( Block block, int x, int y, int z )
		{
			BlockManager.SetBlock( block, WorldPosition.X + x, y, WorldPosition.Z + z );
		}
		public void SetBlock( Block block, Vector3 vecIndex )
		{
			BlockManager.SetBlock( block, vecIndex );
		}
		public void SetBlock( Block block, Vector3i vecIndex )
		{
			BlockManager.SetBlock( block, vecIndex );
		}
		public void FastSetBlock( Block block, int x, int y, int z )
		{
			BlockManager.FastSetBlock( block, WorldPosition.X + x, y, WorldPosition.Z + z );
		}
		public void FastSetBlock( Block block, Vector3 vecIndex )
		{
			BlockManager.FastSetBlock( block, vecIndex );
		}
		public void FastSetBlock( Block block, Vector3i vecIndex )
		{
			BlockManager.FastSetBlock( block, vecIndex );
		}
		public void SetBlockType( EBlockType nType, int x, int y, int z )
		{
			BlockManager.SetBlockType( nType, WorldPosition.X + x, y, WorldPosition.Z + z );
		}
		public void SetBlockType( EBlockType nType, Vector3 vecIndex )
		{
			BlockManager.SetBlockType( nType, vecIndex );
		}
		public void SetBlockType( EBlockType nType, Vector3i vecIndex )
		{
			BlockManager.SetBlockType( nType, vecIndex );
		}
		public void FastSetBlockType( EBlockType nType, int x, int y, int z )
		{
			BlockManager.SetBlockType( nType, WorldPosition.X + x, y, WorldPosition.Z + z );
		}
		public void FastSetBlockType( EBlockType nType, Vector3 vecIndex )
		{
			BlockManager.SetBlockType( nType, vecIndex );
		}
		public void FastSetBlockType( EBlockType nType, Vector3i vecIndex )
		{
			BlockManager.SetBlockType( nType, vecIndex );
		}
		#endregion


		public void CalculateHeights()
		{
			for( byte x = 0; x < Chunk.ChunkWidth; x++ )
			{
				int nPosX = WorldPosition.X + x;

				for( byte z = 0; z < Chunk.ChunkDepth; z++ )
				{
					int nPosZ = WorldPosition.Z + z;
					int nOffset = BlockManager.BlockIndexAtWorld( nPosX, nPosZ );

					for( int y = Chunk.ChunkMaxHeight; y >= 0; y-- )
					{
						if( y > HighestSolidBlock && BlockManager.Blocks[nOffset + y].Exists )
							HighestSolidBlock = (byte)y;
						else if( LowestEmptyBlock > y && !BlockManager.Blocks[nOffset + y].Exists )
							LowestEmptyBlock = (byte)y;
					}
				}
			}

			LowestEmptyBlock--;
		}




		private bool m_bDisposed = false;
		public void Dispose( bool bDispose )
		{
			if( m_bDisposed )
				return;

			if( bDispose )
			{
				VertexList.Clear();
				VertexList = null;
				IndexList.Clear();
				IndexList = null;

				if( VertexBuffer != null )
					VertexBuffer.Dispose();
				if( IndexList != null )
					IndexBuffer.Dispose();
			}

			m_bDisposed = true;
			GC.SuppressFinalize( this );
		}
		~Chunk()
		{
			Dispose( false );
		}
	}
}
