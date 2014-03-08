using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxLib.Chunks.Generators
{
	public class IChunkGenerator
	{
		protected int m_nSeed;

		public IChunkGenerator( int nSeed )
		{
			m_nSeed = nSeed;
		}

		protected virtual void Generate( Chunk chunk, int nSeed )
		{
		}
	}
}
