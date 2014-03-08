//! Heavily follows the TechCraft format.

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VoxLib.Blocks;
using VoxLib.Common.Vector;
using VoxLib.Configurations;

namespace VoxLib.Chunks.Processors
{
	public class ChunkVertexProcessor : IChunkProcessor
	{
		private GraphicsDevice m_GraphicsDevice;
		public bool FourSidedShading	{ get; set; }

		public ChunkVertexProcessor( GraphicsDevice gd )
		{
			m_GraphicsDevice = gd;
			FourSidedShading = true;
		}

		public void Process( Chunk chunk )
		{
			if( chunk.State != EChunkState.AwaitingBuild )
				return;

			chunk.State = EChunkState.Building;
			chunk.CalculateHeights();

			chunk.BoundingBox = new BoundingBox(
				new Vector3(chunk.WorldPosition.X, 0, chunk.WorldPosition.Z),
				new Vector3(chunk.WorldPosition.X + Chunk.ChunkWidth, chunk.HighestSolidBlock, chunk.WorldPosition.Z + Chunk.ChunkDepth) );

			BuildVertexList( chunk );

			chunk.State = EChunkState.Ready;
		}

		private void Clear( Chunk chunk )
		{
			chunk.VertexList.Clear();
			chunk.IndexList.Clear();
			chunk.nIndex = 0;
		}

		private void BuildVertexList( Chunk chunk )
		{
			Clear( chunk );

			for( byte x = 0; x < Chunk.ChunkWidth; x++ ) 
			{
				for( byte z = 0; z < Chunk.ChunkDepth; z++ )
				{
					int nOffset = BlockManager.BlockIndexAt( chunk, x, z );

					for( byte y = chunk.LowestEmptyBlock; y <= chunk.HighestSolidBlock; y++ )
					{
						int nBlockOffset = nOffset + y;
						Block block = BlockManager.Blocks[nBlockOffset];

						if( block.nType == EBlockType.Air )
							continue;

						Vector3i vecPos = new Vector3i( chunk.WorldPosition.X + x, y, chunk.WorldPosition.Z + z );

						if( this.FourSidedShading )
							BuildBlockVertices4Sided( chunk, nBlockOffset, vecPos );
						else
							BuildBlockVerticesSimpleShaded( chunk, nBlockOffset, vecPos );
					}
				}
			}

			BlockVertexPositionTextureLight[] verts = chunk.VertexList.ToArray();
			short[] inds = chunk.IndexList.ToArray();

			if( verts.Length == 0 || inds.Length == 0 )
				return;

			chunk.VertexBuffer = new VertexBuffer( m_GraphicsDevice, typeof(BlockVertexPositionTextureLight),
				verts.Length, BufferUsage.WriteOnly );
			chunk.IndexBuffer = new IndexBuffer( m_GraphicsDevice, IndexElementSize.SixteenBits, inds.Length, BufferUsage.WriteOnly );

			chunk.VertexBuffer.SetData( verts );
			chunk.IndexBuffer.SetData( inds );
		}

		private void BuildBlockVerticesSimpleShaded( Chunk chunk, int nBlockOffset, Vector3i vecPosition )
		{
			Block block = BlockManager.Blocks[nBlockOffset];

            Block blockTopNW, blockTopN, blockTopNE, blockTopW, blockTopM, blockTopE, blockTopSW, blockTopS, blockTopSE;
            Block blockMidNW, blockMidN, blockMidNE, blockMidW, blockMidM, blockMidE, blockMidSW, blockMidS, blockMidSE;
            Block blockBotNW, blockBotN, blockBotNE, blockBotW, blockBotM, blockBotE, blockBotSW, blockBotS, blockBotSE;

            blockTopNW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y + 1, vecPosition.Z + 1 );
            blockTopN = BlockManager.BlockAt( vecPosition.X, vecPosition.Y + 1, vecPosition.Z + 1 );
            blockTopNE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y + 1, vecPosition.Z + 1 );
            blockTopW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y + 1, vecPosition.Z );
            blockTopM = BlockManager.BlockAt( vecPosition.X, vecPosition.Y + 1, vecPosition.Z );
            blockTopE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y + 1, vecPosition.Z );
            blockTopSW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y + 1, vecPosition.Z - 1 );
            blockTopS = BlockManager.BlockAt( vecPosition.X, vecPosition.Y + 1, vecPosition.Z - 1 );
            blockTopSE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y + 1, vecPosition.Z - 1 );
            blockMidNW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y, vecPosition.Z + 1 );
            blockMidN = BlockManager.BlockAt( vecPosition.X, vecPosition.Y, vecPosition.Z + 1 );
            blockMidNE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y, vecPosition.Z + 1 );
            blockMidW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y, vecPosition.Z );
            blockMidE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y, vecPosition.Z );
            blockMidSW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y, vecPosition.Z - 1 );
            blockMidS = BlockManager.BlockAt( vecPosition.X, vecPosition.Y, vecPosition.Z - 1 );
            blockMidSE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y, vecPosition.Z - 1 );
            blockBotNW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y - 1, vecPosition.Z + 1 );
            blockBotN = BlockManager.BlockAt( vecPosition.X, vecPosition.Y - 1, vecPosition.Z + 1 );
            blockBotNE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y - 1, vecPosition.Z + 1 );
            blockBotW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y - 1, vecPosition.Z );
            blockBotM = BlockManager.BlockAt( vecPosition.X, vecPosition.Y - 1, vecPosition.Z );
            blockBotE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y - 1, vecPosition.Z );
            blockBotSW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y - 1, vecPosition.Z - 1 );
            blockBotS = BlockManager.BlockAt( vecPosition.X, vecPosition.Y - 1, vecPosition.Z - 1 );
            blockBotSE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y - 1, vecPosition.Z - 1 );

            float fSunTL = 0.0f, fSunBR = 0.0f;
			float fRedTL = 0.0f, fRedBR = 0.0f;
			float fGrnTL = 0.0f, fGrnBR = 0.0f;
			float fBluTL = 0.0f, fBluBR = 0.0f;
            Color localTL = Color.Black, localBR = Color.Black;
			byte nMaxSun = Configuration.WorldConfig.MaxSun;

			if( !blockMidW.Exists && !(block.nType == EBlockType.Water && blockMidW.nType == EBlockType.Water) )
			{
                //! 
				fSunTL = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockTopNW.nSun + blockTopW.nSun + blockMidNW.nSun + blockMidW.nSun) / 4);
				fSunBR = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockBotSW.nSun + blockBotW.nSun + blockMidSW.nSun + blockMidW.nSun) / 4);
				fRedTL = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockTopNW.R + blockTopW.R + blockMidNW.R + blockMidW.R) / 4);
				fRedBR = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockBotSW.R + blockBotW.R + blockMidSW.R + blockMidW.R) / 4);
				fGrnTL = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockTopNW.G + blockTopW.G + blockMidNW.G + blockMidW.G) / 4);
				fGrnBR = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockBotSW.G + blockBotW.G + blockMidSW.G + blockMidW.G) / 4);
				fBluTL = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockTopNW.B + blockTopW.B + blockMidNW.B + blockMidW.B) / 4);
				fBluBR = ( 1f / Configuration.WorldConfig.MaxSun) * ((blockBotSW.B + blockBotW.B + blockMidSW.B + blockMidW.B) / 4);

				localTL = new Color(fRedTL, fGrnTL, fBluTL);
				localBR = new Color(fRedBR, fGrnBR, fBluBR);

				BuildFaceVerticesSimpleShaded( chunk, vecPosition, block.nType, EBlockFaceDirection.XDecreasing,
					fSunTL, fSunBR, localTL, localBR );
			}
			if( !blockMidE.Exists && !(block.nType == EBlockType.Water && blockMidE.nType == EBlockType.Water) )
			{
				fSunTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.nSun + blockTopE.nSun + blockMidSE.nSun + blockMidE.nSun) / 4);
				fSunBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.nSun + blockBotE.nSun + blockMidNE.nSun + blockMidE.nSun) / 4);
				fRedTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.R + blockTopE.R + blockMidSE.R + blockMidE.R) / 4);
				fRedBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.R + blockBotE.R + blockMidNE.R + blockMidE.R) / 4);
				fGrnTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.G + blockTopE.G + blockMidSE.G + blockMidE.G) / 4);
				fGrnBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.G + blockBotE.G + blockMidNE.G + blockMidE.G) / 4);
				fBluTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.B + blockTopE.B + blockMidSE.B + blockMidE.B) / 4);
				fBluBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.B + blockBotE.B + blockMidNE.B + blockMidE.B) / 4);

				localTL = new Color(fRedTL, fGrnTL, fBluTL);
				localBR = new Color(fRedBR, fGrnBR, fBluBR);

				BuildFaceVerticesSimpleShaded( chunk, vecPosition, block.nType, EBlockFaceDirection.XIncreasing,
					fSunTL, fSunBR, localTL, localBR );
			}
			if( !blockBotM.Exists && !(block.nType == EBlockType.Water && blockBotM.nType == EBlockType.Water) )
			{
				fSunBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.nSun + blockBotS.nSun + blockBotM.nSun + blockTopE.nSun) / 4);
				fSunTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.nSun + blockBotN.nSun + blockBotM.nSun + blockTopW.nSun) / 4);
				fRedBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.R + blockBotS.R + blockBotM.R + blockTopE.R) / 4);
				fRedTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.R + blockBotN.R + blockBotM.R + blockTopW.R) / 4);
				fGrnBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.G + blockBotS.G + blockBotM.G + blockTopE.G) / 4);
				fGrnTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.G + blockBotN.G + blockBotM.G + blockTopW.G) / 4);
				fBluBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.B + blockBotS.B + blockBotM.B + blockTopE.B) / 4);
				fBluTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.B + blockBotN.B + blockBotM.B + blockTopW.B) / 4);

				localTL = new Color(fRedTL, fGrnTL, fBluTL);
				localBR = new Color(fRedBR, fGrnBR, fBluBR);

				BuildFaceVerticesSimpleShaded( chunk, vecPosition, block.nType, EBlockFaceDirection.YDecreasing,
					fSunTL, fSunBR, localTL, localBR );
			}
			if( !blockTopM.Exists && !(block.nType == EBlockType.Water && blockTopM.nType == EBlockType.Water) )
			{
				fSunTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNW.nSun + blockTopN.nSun + blockTopW.nSun + blockTopM.nSun) / 4);
				fSunBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.nSun + blockTopS.nSun + blockTopE.nSun + blockTopM.nSun) / 4);
				fRedTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNW.R + blockTopN.R + blockTopW.R + blockTopM.R) / 4);
				fRedBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.R + blockTopS.R + blockTopE.R + blockTopM.R) / 4);
				fGrnTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNW.G + blockTopN.G + blockTopW.G + blockTopM.G) / 4);
				fGrnBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.G + blockTopS.G + blockTopE.G + blockTopM.G) / 4);
				fBluTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNW.B + blockTopN.B + blockTopW.B + blockTopM.B) / 4);
				fBluBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.B + blockTopS.B + blockTopE.B + blockTopM.B) / 4);

				localTL = new Color(fRedTL, fGrnTL, fBluTL);
				localBR = new Color(fRedBR, fGrnBR, fBluBR);

				BuildFaceVerticesSimpleShaded( chunk, vecPosition, block.nType, EBlockFaceDirection.YIncreasing,
					fSunTL, fSunBR, localTL, localBR );
			}
			if( !blockMidS.Exists && !(block.nType == EBlockType.Water && blockMidS.nType == EBlockType.Water) )
			{
				fSunTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSW.nSun + blockTopS.nSun + blockMidSW.nSun + blockMidS.nSun) / 4);
				fSunBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.nSun + blockBotS.nSun + blockMidSE.nSun + blockMidS.nSun) / 4);
				fRedTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSW.R + blockTopS.R + blockMidSW.R + blockMidS.R) / 4);
				fRedBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.R + blockBotS.R + blockMidSE.R + blockMidS.R) / 4);
				fGrnTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSW.G + blockTopS.G + blockMidSW.G + blockMidS.G) / 4);
				fGrnBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.G + blockBotS.G + blockMidSE.G + blockMidS.G) / 4);
				fBluTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSW.B + blockTopS.B + blockMidSW.B + blockMidS.B) / 4);
				fBluBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.B + blockBotS.B + blockMidSE.B + blockMidS.B) / 4);

				localTL = new Color(fRedTL, fGrnTL, fBluTL);
				localBR = new Color(fRedBR, fGrnBR, fBluBR);

				BuildFaceVerticesSimpleShaded( chunk, vecPosition, block.nType, EBlockFaceDirection.ZDecreasing,
					fSunTL, fSunBR, localTL, localBR );
			}
			if( !blockMidN.Exists && !(block.nType == EBlockType.Water && blockMidN.nType == EBlockType.Water) )
			{
				fSunTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.nSun + blockTopN.nSun + blockMidNE.nSun + blockMidN.nSun) / 4);
				fSunBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.nSun + blockBotN.nSun + blockMidNW.nSun + blockMidN.nSun) / 4);
				fRedTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.R + blockTopN.R + blockMidNE.R + blockMidN.R) / 4);
				fRedBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.R + blockBotN.R + blockMidNW.R + blockMidN.R) / 4);
				fGrnTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.G + blockTopN.G + blockMidNE.G + blockMidN.G) / 4);
				fGrnBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.G + blockBotN.G + blockMidNW.G + blockMidN.G) / 4);
				fBluTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.B + blockTopN.B + blockMidNE.B + blockMidN.B) / 4);
				fBluBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.B + blockBotN.B + blockMidNW.B + blockMidN.B) / 4);

				localTL = new Color(fRedTL, fGrnTL, fBluTL);
				localBR = new Color(fRedBR, fGrnBR, fBluBR);

				BuildFaceVerticesSimpleShaded( chunk, vecPosition, block.nType, EBlockFaceDirection.ZIncreasing,
					fSunTL, fSunBR, localTL, localBR );
			}
		}
		private void BuildBlockVertices4Sided( Chunk chunk, int nBlockOffset, Vector3i vecPosition )
		{
			Block block = BlockManager.Blocks[nBlockOffset]; // get the block to process.

            Block blockTopNW, blockTopN, blockTopNE, blockTopW, blockTopM, blockTopE, blockTopSW, blockTopS, blockTopSE;
            Block blockMidNW, blockMidN, blockMidNE, blockMidW, blockMidM, blockMidE, blockMidSW, blockMidS, blockMidSE;
            Block blockBotNW, blockBotN, blockBotNE, blockBotW, blockBotM, blockBotE, blockBotSW, blockBotS, blockBotSE;

            blockTopNW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y + 1, vecPosition.Z + 1 );
            blockTopN = BlockManager.BlockAt( vecPosition.X, vecPosition.Y + 1, vecPosition.Z + 1 );
            blockTopNE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y + 1, vecPosition.Z + 1 );
            blockTopW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y + 1, vecPosition.Z );
            blockTopM = BlockManager.BlockAt( vecPosition.X, vecPosition.Y + 1, vecPosition.Z );
            blockTopE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y + 1, vecPosition.Z );
            blockTopSW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y + 1, vecPosition.Z - 1 );
            blockTopS = BlockManager.BlockAt( vecPosition.X, vecPosition.Y + 1, vecPosition.Z - 1 );
            blockTopSE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y + 1, vecPosition.Z - 1 );
            blockMidNW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y, vecPosition.Z + 1 );
            blockMidN = BlockManager.BlockAt( vecPosition.X, vecPosition.Y, vecPosition.Z + 1 );
            blockMidNE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y, vecPosition.Z + 1 );
            blockMidW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y, vecPosition.Z );
            blockMidE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y, vecPosition.Z );
            blockMidSW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y, vecPosition.Z - 1 );
            blockMidS = BlockManager.BlockAt( vecPosition.X, vecPosition.Y, vecPosition.Z - 1 );
            blockMidSE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y, vecPosition.Z - 1 );
            blockBotNW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y - 1, vecPosition.Z + 1 );
            blockBotN = BlockManager.BlockAt( vecPosition.X, vecPosition.Y - 1, vecPosition.Z + 1 );
            blockBotNE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y - 1, vecPosition.Z + 1 );
            blockBotW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y - 1, vecPosition.Z );
            blockBotM = BlockManager.BlockAt( vecPosition.X, vecPosition.Y - 1, vecPosition.Z );
            blockBotE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y - 1, vecPosition.Z );
            blockBotSW = BlockManager.BlockAt( vecPosition.X - 1, vecPosition.Y - 1, vecPosition.Z - 1 );
            blockBotS = BlockManager.BlockAt( vecPosition.X, vecPosition.Y - 1, vecPosition.Z - 1 );
            blockBotSE = BlockManager.BlockAt( vecPosition.X + 1, vecPosition.Y - 1, vecPosition.Z - 1 );

            float fSunTR = 0.0f, fSunTL = 0.0f, fSunBR = 0.0f, fSunBL = 0.0f;
			float fRedTR = 0.0f, fRedTL = 0.0f, fRedBR = 0.0f, fRedBL = 0.0f;
			float fGrnTR = 0.0f, fGrnTL = 0.0f, fGrnBR = 0.0f, fGrnBL = 0.0f;
			float fBluTR = 0.0f, fBluTL = 0.0f, fBluBR = 0.0f, fBluBL = 0.0f;
            Color localTR = Color.Black, localTL = Color.Black, localBR = Color.Black, localBL = Color.Black;
			byte nMaxSun = Configuration.WorldConfig.MaxSun;

			if( !blockMidW.Exists && !(block.nType == EBlockType.Water && blockMidW.nType == EBlockType.Water) )
			{
				fSunTL = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockTopNW.nSun + blockTopW.nSun + blockMidNW.nSun + blockMidW.nSun) / 4);
				fSunTR = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockTopSW.nSun + blockTopW.nSun + blockMidSW.nSun + blockMidW.nSun) / 4);
				fSunBL = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockBotNW.nSun + blockBotW.nSun + blockMidNW.nSun + blockMidW.nSun) / 4);
				fSunBR = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockBotSW.nSun + blockBotW.nSun + blockMidSW.nSun + blockMidW.nSun) / 4);

				fRedTL = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockTopNW.R + blockTopW.R + blockMidNW.R + blockMidW.R) / 4);
				fRedTR = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockTopSW.R + blockTopW.R + blockMidSW.R + blockMidW.R) / 4);
				fRedBL = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockBotNW.R + blockBotW.R + blockMidNW.R + blockMidW.R) / 4);
				fRedBR = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockBotSW.R + blockBotW.R + blockMidSW.R + blockMidW.R) / 4);

				fGrnTL = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockTopNW.G + blockTopW.G + blockMidNW.G + blockMidW.G) / 4);
				fGrnTR = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockTopSW.G + blockTopW.G + blockMidSW.G + blockMidW.G) / 4);
				fGrnBL = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockBotNW.G + blockBotW.G + blockMidNW.G + blockMidW.G) / 4);
				fGrnBR = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockBotSW.G + blockBotW.G + blockMidSW.G + blockMidW.G) / 4);

				fBluTL = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockTopNW.B + blockTopW.B + blockMidNW.B + blockMidW.B) / 4);
				fBluTR = ( 1f/ Configuration.WorldConfig.MaxSun) * ((blockTopSW.B + blockTopW.B + blockMidSW.B + blockMidW.B) / 4);
				fBluBL = ( 1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.B + blockBotW.B + blockMidNW.B + blockMidW.B) / 4);
				fBluBR = ( 1f / Configuration.WorldConfig.MaxSun) * ((blockBotSW.B + blockBotW.B + blockMidSW.B + blockMidW.B) / 4);

				localTL = new Color(fRedTL, fGrnTL, fBluTL);
				localTR = new Color(fRedTR, fGrnTR, fBluTR);
				localBL = new Color(fRedBL, fGrnBL, fBluBL);
				localBR = new Color(fRedBR, fGrnBR, fBluBR);

				BuildFaceVertices4Sided( chunk, vecPosition, block.nType, EBlockFaceDirection.XDecreasing,
					fSunTL, fSunTR, fSunBL, fSunBR, localTL, localTR, localBL, localBR );
			}
			if( !blockMidE.Exists && !(block.nType == EBlockType.Water && blockMidE.nType == EBlockType.Water) )
			{
				fSunTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.nSun + blockTopE.nSun + blockMidSE.nSun + blockMidE.nSun) / 4);
				fSunTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.nSun + blockTopE.nSun + blockMidNE.nSun + blockMidE.nSun) / 4);
				fSunBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.nSun + blockBotE.nSun + blockMidSE.nSun + blockMidE.nSun) / 4);
				fSunBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.nSun + blockBotE.nSun + blockMidNE.nSun + blockMidE.nSun) / 4);
				fRedTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.R + blockTopE.R + blockMidSE.R + blockMidE.R) / 4);
				fRedTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.R + blockTopE.R + blockMidNE.R + blockMidE.R) / 4);
				fRedBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.R + blockBotE.R + blockMidSE.R + blockMidE.R) / 4);
				fRedBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.R + blockBotE.R + blockMidNE.R + blockMidE.R) / 4);

				fGrnTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.G + blockTopE.G + blockMidSE.G + blockMidE.G) / 4);
				fGrnTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.G + blockTopE.G + blockMidNE.G + blockMidE.G) / 4);
				fGrnBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.G + blockBotE.G + blockMidSE.G + blockMidE.G) / 4);
				fGrnBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.G + blockBotE.G + blockMidNE.G + blockMidE.G) / 4);

				fBluTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.B + blockTopE.B + blockMidSE.B + blockMidE.B) / 4);
				fBluTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.B + blockTopE.B + blockMidNE.B + blockMidE.B) / 4);
				fBluBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.B + blockBotE.B + blockMidSE.B + blockMidE.B) / 4);
				fBluBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.B + blockBotE.B + blockMidNE.B + blockMidE.B) / 4);

				localTL = new Color(fRedTL, fGrnTL, fBluTL);
				localTR = new Color(fRedTR, fGrnTR, fBluTR);
				localBL = new Color(fRedBL, fGrnBL, fBluBL);
				localBR = new Color(fRedBR, fGrnBR, fBluBR);

				BuildFaceVertices4Sided( chunk, vecPosition, block.nType, EBlockFaceDirection.XIncreasing,
					fSunTL, fSunTR, fSunBL, fSunBR, localTL, localTR, localBL, localBR );
			}
			if( !blockBotM.Exists && !(block.nType == EBlockType.Water && blockBotM.nType == EBlockType.Water) )
			{
				fSunBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSW.nSun + blockBotS.nSun + blockBotM.nSun + blockTopW.nSun) / 4);
				fSunBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.nSun + blockBotS.nSun + blockBotM.nSun + blockTopE.nSun) / 4);
				fSunTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.nSun + blockBotN.nSun + blockBotM.nSun + blockTopW.nSun) / 4);
				fSunTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.nSun + blockBotN.nSun + blockBotM.nSun + blockTopE.nSun) / 4);
				fRedBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSW.R + blockBotS.R + blockBotM.R + blockTopW.R) / 4);
				fRedBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.R + blockBotS.R + blockBotM.R + blockTopE.R) / 4);
				fRedTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.R + blockBotN.R + blockBotM.R + blockTopW.R) / 4);
				fRedTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.R + blockBotN.R + blockBotM.R + blockTopE.R) / 4);

				fGrnBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSW.G + blockBotS.G + blockBotM.G + blockTopW.G) / 4);
				fGrnBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.G + blockBotS.G + blockBotM.G + blockTopE.G) / 4);
				fGrnTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.G + blockBotN.G + blockBotM.G + blockTopW.G) / 4);
				fGrnTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.G + blockBotN.G + blockBotM.G + blockTopE.G) / 4);

				fBluBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSW.B + blockBotS.B + blockBotM.B + blockTopW.B) / 4);
				fBluBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.B + blockBotS.B + blockBotM.B + blockTopE.B) / 4);
				fBluTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.B + blockBotN.B + blockBotM.B + blockTopW.B) / 4);
				fBluTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.B + blockBotN.B + blockBotM.B + blockTopE.B) / 4);

				localTL = new Color(fRedTL, fGrnTL, fBluTL);
				localTR = new Color(fRedTR, fGrnTR, fBluTR);
				localBL = new Color(fRedBL, fGrnBL, fBluBL);
				localBR = new Color(fRedBR, fGrnBR, fBluBR);

				BuildFaceVertices4Sided( chunk, vecPosition, block.nType, EBlockFaceDirection.YDecreasing,
					fSunTL, fSunTR, fSunBL, fSunBR, localTL, localTR, localBL, localBR );
			}
			if( !blockTopM.Exists && !(block.nType == EBlockType.Water && blockTopM.nType == EBlockType.Water) )
			{
				fSunTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNW.nSun + blockTopN.nSun + blockTopW.nSun + blockTopM.nSun) / 4);
				fSunTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.nSun + blockTopN.nSun + blockTopE.nSun + blockTopM.nSun) / 4);
				fSunBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSW.nSun + blockTopS.nSun + blockTopW.nSun + blockTopM.nSun) / 4);
				fSunBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.nSun + blockTopS.nSun + blockTopE.nSun + blockTopM.nSun) / 4);

				fRedTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNW.R + blockTopN.R + blockTopW.R + blockTopM.R) / 4);
				fRedTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.R + blockTopN.R + blockTopE.R + blockTopM.R) / 4);
				fRedBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSW.R + blockTopS.R + blockTopW.R + blockTopM.R) / 4);
				fRedBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.R + blockTopS.R + blockTopE.R + blockTopM.R) / 4);

				fGrnTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNW.G + blockTopN.G + blockTopW.G + blockTopM.G) / 4);
				fGrnTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.G + blockTopN.G + blockTopE.G + blockTopM.G) / 4);
				fGrnBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSW.G + blockTopS.G + blockTopW.G + blockTopM.G) / 4);
				fGrnBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.G + blockTopS.G + blockTopE.G + blockTopM.G) / 4);

				fBluTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNW.B + blockTopN.B + blockTopW.B + blockTopM.B) / 4);
				fBluTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.B + blockTopN.B + blockTopE.B + blockTopM.B) / 4);
				fBluBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSW.B + blockTopS.B + blockTopW.B + blockTopM.B) / 4);
				fBluBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.B + blockTopS.B + blockTopE.B + blockTopM.B) / 4);

				localTL = new Color(fRedTL, fGrnTL, fBluTL);
				localTR = new Color(fRedTR, fGrnTR, fBluTR);
				localBL = new Color(fRedBL, fGrnBL, fBluBL);
				localBR = new Color(fRedBR, fGrnBR, fBluBR);

				BuildFaceVertices4Sided( chunk, vecPosition, block.nType, EBlockFaceDirection.YIncreasing,
					fSunTL, fSunTR, fSunBL, fSunBR, localTL, localTR, localBL, localBR );
			}
			if( !blockMidS.Exists && !(block.nType == EBlockType.Water && blockMidS.nType == EBlockType.Water) )
			{
				fSunTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSW.nSun + blockTopS.nSun + blockMidSW.nSun + blockMidS.nSun) / 4);
				fSunTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.nSun + blockTopS.nSun + blockMidSE.nSun + blockMidS.nSun) / 4);
				fSunBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSW.nSun + blockBotS.nSun + blockMidSW.nSun + blockMidS.nSun) / 4);
				fSunBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.nSun + blockBotS.nSun + blockMidSE.nSun + blockMidS.nSun) / 4);

				fRedTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSW.R + blockTopS.R + blockMidSW.R + blockMidS.R) / 4);
				fRedTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.R + blockTopS.R + blockMidSE.R + blockMidS.R) / 4);
				fRedBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSW.R + blockBotS.R + blockMidSW.R + blockMidS.R) / 4);
				fRedBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.R + blockBotS.R + blockMidSE.R + blockMidS.R) / 4);

				fGrnTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSW.G + blockTopS.G + blockMidSW.G + blockMidS.G) / 4);
				fGrnTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.G + blockTopS.G + blockMidSE.G + blockMidS.G) / 4);
				fGrnBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSW.G + blockBotS.G + blockMidSW.G + blockMidS.G) / 4);
				fGrnBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.G + blockBotS.G + blockMidSE.G + blockMidS.G) / 4);

				fBluTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSW.B + blockTopS.B + blockMidSW.B + blockMidS.B) / 4);
				fBluTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopSE.B + blockTopS.B + blockMidSE.B + blockMidS.B) / 4);
				fBluBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSW.B + blockBotS.B + blockMidSW.B + blockMidS.B) / 4);
				fBluBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotSE.B + blockBotS.B + blockMidSE.B + blockMidS.B) / 4);

				localTL = new Color(fRedTL, fGrnTL, fBluTL);
				localTR = new Color(fRedTR, fGrnTR, fBluTR);
				localBL = new Color(fRedBL, fGrnBL, fBluBL);
				localBR = new Color(fRedBR, fGrnBR, fBluBR);

				BuildFaceVertices4Sided( chunk, vecPosition, block.nType, EBlockFaceDirection.ZDecreasing,
					fSunTL, fSunTR, fSunBL, fSunBR, localTL, localTR, localBL, localBR );
			}
			if( !blockMidN.Exists && !(block.nType == EBlockType.Water && blockMidN.nType == EBlockType.Water) )
			{
				fSunTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.nSun + blockTopN.nSun + blockMidNE.nSun + blockMidN.nSun) / 4);
				fSunTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNW.nSun + blockTopN.nSun + blockMidNW.nSun + blockMidN.nSun) / 4);
				fSunBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.nSun + blockBotN.nSun + blockMidNE.nSun + blockMidN.nSun) / 4);
				fSunBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.nSun + blockBotN.nSun + blockMidNW.nSun + blockMidN.nSun) / 4);

				fRedTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.R + blockTopN.R + blockMidNE.R + blockMidN.R) / 4);
				fRedTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNW.R + blockTopN.R + blockMidNW.R + blockMidN.R) / 4);
				fRedBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.R + blockBotN.R + blockMidNE.R + blockMidN.R) / 4);
				fRedBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.R + blockBotN.R + blockMidNW.R + blockMidN.R) / 4);

				fGrnTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.G + blockTopN.G + blockMidNE.G + blockMidN.G) / 4);
				fGrnTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNW.G + blockTopN.G + blockMidNW.G + blockMidN.G) / 4);
				fGrnBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.G + blockBotN.G + blockMidNE.G + blockMidN.G) / 4);
				fGrnBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.G + blockBotN.G + blockMidNW.G + blockMidN.G) / 4);

				fBluTL = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNE.B + blockTopN.B + blockMidNE.B + blockMidN.B) / 4);
				fBluTR = (1f / Configuration.WorldConfig.MaxSun) * ((blockTopNW.B + blockTopN.B + blockMidNW.B + blockMidN.B) / 4);
				fBluBL = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNE.B + blockBotN.B + blockMidNE.B + blockMidN.B) / 4);
				fBluBR = (1f / Configuration.WorldConfig.MaxSun) * ((blockBotNW.B + blockBotN.B + blockMidNW.B + blockMidN.B) / 4);

				localTL = new Color(fRedTL, fGrnTL, fBluTL);
				localTR = new Color(fRedTR, fGrnTR, fBluTR);
				localBL = new Color(fRedBL, fGrnBL, fBluBL);
				localBR = new Color(fRedBR, fGrnBR, fBluBR);

				BuildFaceVertices4Sided( chunk, vecPosition, block.nType, EBlockFaceDirection.ZIncreasing,
					fSunTL, fSunTR, fSunBL, fSunBR, localTL, localTR, localBL, localBR );
			}
		}

		private void BuildFaceVerticesSimpleShaded( Chunk chunk, Vector3i vecPos, EBlockType nBlockType,
			EBlockFaceDirection nFaceDir, float fSunTL, float fSunBR, Color colorTL, Color colorBR )
		{
			EBlockTexture nTexture = Block.GetTexture( nBlockType, nFaceDir );
			int nFaceIndex = 0;
			Vector2[] vecTexUV = BlockTextureMap.BlockTextures[(int)nTexture * 6 + nFaceIndex];

			switch( nFaceDir )
			{
				case EBlockFaceDirection.XIncreasing:
				{
					AddVertex( chunk, vecPos, new Vector3(1, 1, 1), vecTexUV[0], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(1, 1, 0), vecTexUV[1], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(1, 0, 1), vecTexUV[2], fSunBR, colorBR );
					AddVertex( chunk, vecPos, new Vector3(1, 0, 0), vecTexUV[5], fSunBR, colorBR );
					AddIndex( chunk, 0, 1, 2, 2, 1, 3 );
				}
				break;
				

				case EBlockFaceDirection.XDecreasing:
				{
					AddVertex( chunk, vecPos, new Vector3(0, 1, 0), vecTexUV[0], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(0, 1, 1), vecTexUV[1], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(0, 0, 0), vecTexUV[5], fSunBR, colorBR );
					AddVertex( chunk, vecPos, new Vector3(0, 0, 1), vecTexUV[2], fSunBR, colorBR );
					AddIndex( chunk, 0, 1, 3, 0, 3, 2 );
				}
				break;

				case EBlockFaceDirection.YIncreasing:
				{
					AddVertex( chunk, vecPos, new Vector3(1, 1, 1), vecTexUV[0], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(0, 1, 1), vecTexUV[2], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(1, 1, 0), vecTexUV[4], fSunBR, colorBR );
					AddVertex( chunk, vecPos, new Vector3(0, 1, 0), vecTexUV[5], fSunBR, colorBR );
					AddIndex( chunk, 3, 2, 0, 3, 0, 1 );
				}
				break;

				case EBlockFaceDirection.YDecreasing:
				{
					AddVertex( chunk, vecPos, new Vector3(1, 0, 1), vecTexUV[0], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(0, 0, 1), vecTexUV[2], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(1, 0, 0), vecTexUV[4], fSunBR, colorBR );
					AddVertex( chunk, vecPos, new Vector3(0, 0, 0), vecTexUV[5], fSunBR, colorBR );
					AddIndex( chunk, 0, 2, 1, 1, 2, 3 );
				}
				break;

				case EBlockFaceDirection.ZIncreasing:
				{
					AddVertex( chunk, vecPos, new Vector3(0, 1, 1), vecTexUV[0], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(1, 1, 1), vecTexUV[1], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(0, 0, 1), vecTexUV[5], fSunBR, colorBR );
					AddVertex( chunk, vecPos, new Vector3(1, 0, 1), vecTexUV[2], fSunBR, colorBR );
					AddIndex( chunk, 0, 1, 3, 0, 3, 2 );
				}
				break;

				case EBlockFaceDirection.ZDecreasing:
				{
					AddVertex( chunk, vecPos, new Vector3(1, 1, 0), vecTexUV[0], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(0, 1, 0), vecTexUV[1], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(1, 0, 0), vecTexUV[2], fSunBR, colorBR );
					AddVertex( chunk, vecPos, new Vector3(0, 0, 0), vecTexUV[5], fSunBR, colorBR );
					AddIndex( chunk, 0, 1, 2, 2, 1, 3 );
				}
				break;
			}
		}

		private void BuildFaceVertices4Sided( Chunk chunk, Vector3i vecPos, EBlockType nBlockType,
			EBlockFaceDirection nFaceDir, float fSunTL, float fSunTR, float fSunBL, float fSunBR,
			Color colorTL, Color colorTR, Color colorBL, Color colorBR )
		{
			EBlockTexture nTexture = Block.GetTexture( nBlockType, nFaceDir );
			int nFaceIndex = 0;
			Vector2[] vecTexUV = BlockTextureMap.BlockTextures[(int)nTexture * 6 + nFaceIndex];

			switch( nFaceDir )
			{
				case EBlockFaceDirection.XIncreasing:
				{
					AddVertex( chunk, vecPos, new Vector3(1, 1, 1), vecTexUV[0], fSunTR, colorTR );
					AddVertex( chunk, vecPos, new Vector3(1, 1, 0), vecTexUV[1], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(1, 0, 1), vecTexUV[2], fSunBR, colorBR );
					AddVertex( chunk, vecPos, new Vector3(1, 0, 0), vecTexUV[5], fSunBL, colorBL );
					AddIndex( chunk, 0, 1, 2, 2, 1, 3 );
				}
				break;
				

				case EBlockFaceDirection.XDecreasing:
				{
					AddVertex( chunk, vecPos, new Vector3(0, 1, 0), vecTexUV[0], fSunTR, colorTR );
					AddVertex( chunk, vecPos, new Vector3(0, 1, 1), vecTexUV[1], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(0, 0, 0), vecTexUV[5], fSunBR, colorBR );
					AddVertex( chunk, vecPos, new Vector3(0, 0, 1), vecTexUV[2], fSunBL, colorBL );
					AddIndex( chunk, 0, 1, 3, 0, 3, 2 );
				}
				break;

				case EBlockFaceDirection.YIncreasing:
				{
					AddVertex( chunk, vecPos, new Vector3(1, 1, 1), vecTexUV[0], fSunTR, colorTR );
					AddVertex( chunk, vecPos, new Vector3(0, 1, 1), vecTexUV[2], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(1, 1, 0), vecTexUV[4], fSunBR, colorBR );
					AddVertex( chunk, vecPos, new Vector3(0, 1, 0), vecTexUV[5], fSunBL, colorBL );
					AddIndex( chunk, 3, 2, 0, 3, 0, 1 );
				}
				break;

				case EBlockFaceDirection.YDecreasing:
				{
					AddVertex( chunk, vecPos, new Vector3(1, 0, 1), vecTexUV[0], fSunTR, colorTR );
					AddVertex( chunk, vecPos, new Vector3(0, 0, 1), vecTexUV[2], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(1, 0, 0), vecTexUV[4], fSunBR, colorBR );
					AddVertex( chunk, vecPos, new Vector3(0, 0, 0), vecTexUV[5], fSunBL, colorBL );
					AddIndex( chunk, 0, 2, 1, 1, 2, 3 );
				}
				break;

				case EBlockFaceDirection.ZIncreasing:
				{
					AddVertex( chunk, vecPos, new Vector3(0, 1, 1), vecTexUV[0], fSunTR, colorTR );
					AddVertex( chunk, vecPos, new Vector3(1, 1, 1), vecTexUV[1], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(0, 0, 1), vecTexUV[5], fSunBR, colorBR );
					AddVertex( chunk, vecPos, new Vector3(1, 0, 1), vecTexUV[2], fSunBL, colorBL );
					AddIndex( chunk, 0, 1, 3, 0, 3, 2 );
				}
				break;

				case EBlockFaceDirection.ZDecreasing:
				{
					AddVertex( chunk, vecPos, new Vector3(1, 1, 0), vecTexUV[0], fSunTR, colorTR );
					AddVertex( chunk, vecPos, new Vector3(0, 1, 0), vecTexUV[1], fSunTL, colorTL );
					AddVertex( chunk, vecPos, new Vector3(1, 0, 0), vecTexUV[2], fSunBR, colorBR );
					AddVertex( chunk, vecPos, new Vector3(0, 0, 0), vecTexUV[5], fSunBL, colorBL );
					AddIndex( chunk, 0, 1, 2, 2, 1, 3 );
				}
				break;
			}
        }

		private static void AddVertex( Chunk chunk, Vector3i vecPos, Vector3 vecAdd, Vector2 vecUV, float fSun, Color color )
		{
			chunk.VertexList.Add( new BlockVertexPositionTextureLight(vecPos.AsVector3() + vecAdd, vecUV, fSun, color.ToVector3()) );
		}

		private static void AddIndex( Chunk chunk, short i1, short i2, short i3, short i4, short i5, short i6 )
		{
			chunk.IndexList.Add( (short)(chunk.nIndex + i1) );
			chunk.IndexList.Add( (short)(chunk.nIndex + i2) );
			chunk.IndexList.Add( (short)(chunk.nIndex + i3) );
			chunk.IndexList.Add( (short)(chunk.nIndex + i4) );
			chunk.IndexList.Add( (short)(chunk.nIndex + i5) );
			chunk.IndexList.Add( (short)(chunk.nIndex + i6) );
			chunk.nIndex += 4;
		}
	}
}
