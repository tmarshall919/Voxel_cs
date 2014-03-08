//! Heavily follows the TechCraft format.

using System;

using VoxLib.Blocks;
using VoxLib.Configurations;

namespace VoxLib.Chunks.Processors
{
	public class ChunkLightProcessor : IChunkProcessor
	{
		public void Process( Chunk chunk )
		{
			if( chunk.State != EChunkState.AwaitingLighting && chunk.State != EChunkState.AwaitingRelighting )
				return;

			chunk.State = EChunkState.Lighting;

			SetInitialLighting( chunk );
			FillSunlight( chunk );
			FillR( chunk );
			FillG( chunk );
			FillB( chunk );
			
			chunk.State = EChunkState.AwaitingBuild;
		}

		private void SetInitialLighting( Chunk chunk )
		{
			byte nSun = Configuration.WorldConfig.MaxSun;

			for( byte x = 0; x < Chunk.ChunkWidth; x++ )
			{
				for( byte z = 0; z < Chunk.ChunkDepth; z++ )
				{
					int nOffset = BlockManager.BlockIndexAt( chunk, x, z );
					bool bInShade = false;

					for( byte y = Chunk.ChunkMaxHeight; y > 0; y-- )
					{
						if( !bInShade && BlockManager.Blocks[nOffset + y].nType != EBlockType.Air )
							bInShade = true;

						BlockManager.Blocks[nOffset + y].nSun = nSun;
						BlockManager.Blocks[nOffset + y].R = 0;
						BlockManager.Blocks[nOffset + y].G = 0;
						BlockManager.Blocks[nOffset + y].B = 0;
					}
				}
			}
		}

		private void FillSunlight( Chunk chunk )
		{
			for( byte x = 0; x < Chunk.ChunkWidth; x++ )
			{
				for( byte z = 0; z < Chunk.ChunkDepth; z++ )
				{
					int nOffsetExa = BlockManager.BlockIndexAt( chunk, x, z );

					for( byte y = Chunk.ChunkMaxHeight; y > 0; y-- )
					{
						int nOffset = nOffsetExa + y;

						if( BlockManager.Blocks[nOffset].nType != EBlockType.Air )
							continue;

						byte nBlockLight = BlockManager.Blocks[nOffset].nSun;
						if( nBlockLight <= 1 )
							continue;

						byte nLight = (byte)((nBlockLight * 9) / 10);

						PropogateSunlight( nOffset + BlockManager.AdvanceX, nLight );
						PropogateSunlight( nOffset - BlockManager.AdvanceX, nLight );
						PropogateSunlight( nOffset + BlockManager.AdvanceZ, nLight );
						PropogateSunlight( nOffset - BlockManager.AdvanceZ, nLight );
						PropogateSunlight( nOffset- 1, nLight );
					}
				}
			}
		}
		private void FillR( Chunk chunk )
		{
			for( byte x = 0; x < Chunk.ChunkWidth; x++ )
			{
				for( byte z = 0; z < Chunk.ChunkDepth; z++ )
				{
					int nOffsetExa = BlockManager.BlockIndexAt( chunk, x, z );

					for( byte y = Chunk.ChunkMaxHeight; y > 0; y-- )
					{
						int nOffset = nOffsetExa + y;

						if( BlockManager.Blocks[nOffset].nType != EBlockType.Air )
							continue;

						byte nBlockLight = BlockManager.Blocks[nOffset].R;
						if( nBlockLight <= 1 )
							continue;

						byte nLight = (byte)((nBlockLight * 9) / 10);

						PropogateR( nOffset + BlockManager.AdvanceX, nLight );
						PropogateR( nOffset - BlockManager.AdvanceX, nLight );
						PropogateR( nOffset + BlockManager.AdvanceZ, nLight );
						PropogateR( nOffset - BlockManager.AdvanceZ, nLight );
						PropogateR( nOffset- 1, nLight );
					}
				}
			}
		}
		private void FillG( Chunk chunk )
		{
			for( byte x = 0; x < Chunk.ChunkWidth; x++ )
			{
				for( byte z = 0; z < Chunk.ChunkDepth; z++ )
				{
					int nOffsetExa = BlockManager.BlockIndexAt( chunk, x, z );

					for( byte y = Chunk.ChunkMaxHeight; y > 0; y-- )
					{
						int nOffset = nOffsetExa + y;

						if( BlockManager.Blocks[nOffset].nType != EBlockType.Air )
							continue;

						byte nBlockLight = BlockManager.Blocks[nOffset].G;
						if( nBlockLight <= 1 )
							continue;

						byte nLight = (byte)((nBlockLight * 9) / 10);

						PropogateG( nOffset + BlockManager.AdvanceX, nLight );
						PropogateG( nOffset - BlockManager.AdvanceX, nLight );
						PropogateG( nOffset + BlockManager.AdvanceZ, nLight );
						PropogateG( nOffset - BlockManager.AdvanceZ, nLight );
						PropogateG( nOffset- 1, nLight );
					}
				}
			}
		}
		private void FillB( Chunk chunk )
		{
			for( byte x = 0; x < Chunk.ChunkWidth; x++ )
			{
				for( byte z = 0; z < Chunk.ChunkDepth; z++ )
				{
					int nOffsetExa = BlockManager.BlockIndexAt( chunk, x, z );

					for( byte y = Chunk.ChunkMaxHeight; y > 0; y-- )
					{
						int nOffset = nOffsetExa + y;

						if( BlockManager.Blocks[nOffset].nType != EBlockType.Air )
							continue;

						byte nBlockLight = BlockManager.Blocks[nOffset].B;
						if( nBlockLight <= 1 )
							continue;

						byte nLight = (byte)((nBlockLight * 9) / 10);

						PropogateB( nOffset + BlockManager.AdvanceX, nLight );
						PropogateB( nOffset - BlockManager.AdvanceX, nLight );
						PropogateB( nOffset + BlockManager.AdvanceZ, nLight );
						PropogateB( nOffset - BlockManager.AdvanceZ, nLight );
						PropogateB( nOffset- 1, nLight );
					}
				}
			}
		}

		private void PropogateSunlight( int nOffset, byte nLight )
		{
			if( nLight <= 1 )
				return;

			int nIndex = nOffset % BlockManager.Blocks.Length;
			if( nIndex < 0 )
				nIndex += BlockManager.Blocks.Length;

			if( BlockManager.Blocks[nIndex].nType != EBlockType.Air )
				return;
			if( nLight <= BlockManager.Blocks[nIndex].nSun )
				return;
       
			BlockManager.Blocks[nIndex].nSun = nLight;

			byte nPropLight = (byte)((nLight * 9) / 10);
			PropogateSunlight( nOffset + BlockManager.AdvanceX, nPropLight );
			PropogateSunlight( nOffset - BlockManager.AdvanceX, nPropLight );
			PropogateSunlight( nOffset + BlockManager.AdvanceZ, nPropLight );
			PropogateSunlight( nOffset - BlockManager.AdvanceZ, nPropLight );
			PropogateSunlight( nOffset- 1, nPropLight );
		}
		private void PropogateR( int nOffset, byte nLight )
		{
			if( nLight <= 1 )
				return;

			int nIndex = nOffset % BlockManager.Blocks.Length;
			if( nIndex < 0 )
				nIndex += BlockManager.Blocks.Length;

			if( BlockManager.Blocks[nIndex].nType != EBlockType.Air )
				return;
			if( nLight <= BlockManager.Blocks[nIndex].R )
				return;

			byte nPropLight = (byte)((nLight * 9) / 10);
			PropogateR( nOffset + BlockManager.AdvanceX, nPropLight );
			PropogateR( nOffset - BlockManager.AdvanceX, nPropLight );
			PropogateR( nOffset + BlockManager.AdvanceZ, nPropLight );
			PropogateR( nOffset - BlockManager.AdvanceZ, nPropLight );
			PropogateR( nOffset- 1, nPropLight );
		}
		private void PropogateG( int nOffset, byte nLight )
		{
			if( nLight <= 1 )
				return;

			int nIndex = nOffset % BlockManager.Blocks.Length;
			if( nIndex < 0 )
				nIndex += BlockManager.Blocks.Length;

			if( BlockManager.Blocks[nIndex].nType != EBlockType.Air )
				return;
			if( nLight <= BlockManager.Blocks[nIndex].G )
				return;

			byte nPropLight = (byte)((nLight * 9) / 10);
			PropogateG( nOffset + BlockManager.AdvanceX, nPropLight );
			PropogateG( nOffset - BlockManager.AdvanceX, nPropLight );
			PropogateG( nOffset + BlockManager.AdvanceZ, nPropLight );
			PropogateG( nOffset - BlockManager.AdvanceZ, nPropLight );
			PropogateG( nOffset- 1, nPropLight );
		}
		private void PropogateB( int nOffset, byte nLight )
		{
			if( nLight <= 1 )
				return;

			int nIndex = nOffset % BlockManager.Blocks.Length;
			if( nIndex < 0 )
				nIndex += BlockManager.Blocks.Length;

			if( BlockManager.Blocks[nIndex].nType != EBlockType.Air )
				return;
			if( nLight <= BlockManager.Blocks[nIndex].B )
				return;

			byte nPropLight = (byte)((nLight * 9) / 10);
			PropogateB( nOffset + BlockManager.AdvanceX, nPropLight );
			PropogateB( nOffset - BlockManager.AdvanceX, nPropLight );
			PropogateB( nOffset + BlockManager.AdvanceZ, nPropLight );
			PropogateB( nOffset - BlockManager.AdvanceZ, nPropLight );
			PropogateB( nOffset- 1, nPropLight );
		}
	}
}
