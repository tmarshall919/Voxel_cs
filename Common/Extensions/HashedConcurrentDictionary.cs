using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace VoxLib.Common.Extensions
{
	class HashedConcurrentDictionary<T> : ConcurrentDictionary<long, T>
	{
		const long m_nSize = Int32.MaxValue;

		public long KeyFromCoords( int x, int z )
		{
			return (long)(x + (z * m_nSize));
		}

		public virtual T this[int x, int z]
		{
			get
			{
				T outVal = default( T );

				TryGetValue( (long)(x + (z * m_nSize)), out outVal );

				return outVal;
			}
			set
			{
				long key = (long)(x + (z * m_nSize));
           
				this[key] = value;
			}
		}

		public virtual void Remove( int x, int z )
		{
			T removed;
			TryRemove( KeyFromCoords(x, z), out removed );
		}
	}
}
