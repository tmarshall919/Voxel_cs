using System;

namespace VoxLib.Chunks.Processors
{
	public interface IChunkProcessor
	{
		void Process( Chunk chunk );
	}
}
