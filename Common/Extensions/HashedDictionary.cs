using System;
using System.Collections.Generic;

namespace VoxLib.Common.Extensions
{
	public class HashedDictionary<T> : Dictionary<long, T>
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
			
			if( TryGetValue( KeyFromCoords(x, z), out removed ) )
				Remove( x, z );
		}
	}
}
