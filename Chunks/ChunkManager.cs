using System;
using System.Collections.Generic;

using VoxLib.Common.Vector;
using VoxLib.Common.Extensions;

namespace VoxLib.Chunks
{
	public class ChunkManager
	{
		public Vector2i SouthwestEdge;
		public Vector2i NortheastEdge;

		private HashedConcurrentDictionary<Chunk> m_Chunks = new HashedConcurrentDictionary<Chunk>();
		private object m_Locker = new object();

		public ChunkManager()
		{
		}

		public Chunk this[int x, int z]
		{
			get { return m_Chunks[x, z]; }
			set { m_Chunks[x, z] = value; }
		}

		public void Remove( int x, int z )
		{
			m_Chunks.Remove( x, z );
		}

		public bool ContainsKey( int x, int z )
		{
			long nKey = m_Chunks.KeyFromCoords( x, z );
			return m_Chunks.ContainsKey( nKey );
		}
		public long KeyFromCoords( int x, int z )
		{
			return m_Chunks.KeyFromCoords( x, z );
		}
		public IEnumerable<Chunk> Values
		{
			get { return m_Chunks.Values; }
		}
	}
}
