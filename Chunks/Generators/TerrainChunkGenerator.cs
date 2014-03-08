using System;

using VoxLib.Blocks;
using VoxLib.Common.Math.Noise;

namespace VoxLib.Chunks.Generators
{
	public class TerrainChunkGenerator : IChunkGenerator
	{
		public TerrainChunkGenerator( int nSeed ) : base( nSeed )
		{
		}

		public void Generate( Chunk chunk )
		{
			chunk.State = EChunkState.Generating;
			for( byte x = 0; x < Chunk.ChunkWidth; x++ )
			{
				int nPosX = chunk.WorldPosition.X + x;

				for( byte z = 0; z < Chunk.ChunkDepth; z++ )
				{
					int nPosZ = chunk.WorldPosition.Z + z;
					this.GenerateBlocks( chunk, nPosX, nPosZ );
				}
			}
			chunk.State = EChunkState.AwaitingLighting;
		}

		private void GenerateBlocks( Chunk chunk, int nPosX, int nPosZ )
		{
			float fRockHeight = this.GetRockHeight( nPosX, nPosZ );
			int nDirtHeight = this.GetDirtHeight( nPosX, nPosZ, fRockHeight );

			int nOffset = BlockManager.BlockIndexAtWorld( nPosX, nPosZ );

			for( int y = Chunk.ChunkMaxHeight; y >= 0; y-- )
			{
				if( y > nDirtHeight )
					BlockManager.Blocks[nOffset + y].nType = EBlockType.Air;
				else if( y > fRockHeight )
					BlockManager.Blocks[nOffset + y].nType = EBlockType.Grass;
				else
					BlockManager.Blocks[nOffset + y].nType = EBlockType.Grass;
			}
		}

		private int GetDirtHeight( int nBlockX, int nBlockZ, float fRockHeight )
		{
			nBlockX += m_nSeed;

			float octave1 = SimplexNoise.noise( nBlockX * 0.001f, m_nSeed, nBlockZ * 0.001f ) * 0.5f;
			float octave2 = SimplexNoise.noise( nBlockX * 0.002f, m_nSeed, nBlockZ * 0.002f ) * 0.25f;
			float octave3 = SimplexNoise.noise( nBlockX * 0.01f, m_nSeed, nBlockZ * 0.01f ) * 0.25f;
			float octaveSum = octave1 + octave2 + octave3;

			return (int)(octaveSum * (Chunk.ChunkHeight / 8)) + (int)fRockHeight;
		}
		private float GetRockHeight( int nBlockX, int nBlockZ )
		{
			nBlockX += m_nSeed;

			int nMinHeight = Chunk.ChunkHeight / 2;
			int nMinDepth = (int)(Chunk.ChunkHeight * 0.4f);

			float octave1 = SimplexNoise.noise( nBlockX * 0.004f, m_nSeed, nBlockZ * 0.004f ) * 0.5f;
			float octave2 = SimplexNoise.noise( nBlockX * 0.003f, m_nSeed, nBlockZ * 0.003f ) * 0.25f;
			float octave3 = SimplexNoise.noise( nBlockX * 0.02f, m_nSeed, nBlockZ * 0.02f ) * 0.15f;
			float lowerGroundHeight = octave1 + octave2 + octave3;

			lowerGroundHeight = lowerGroundHeight * nMinDepth + nMinHeight;

			return lowerGroundHeight;
		}
	}
}
