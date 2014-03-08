using System;
using System.Diagnostics;

namespace VoxLib.Debugging
{
	public class Profiler
	{
		public Stopwatch Time = new Stopwatch();

		public Profiler()
		{
		}

		public void Start()
		{
			Time.Reset();
			Time.Start();
		}
		public void Stop()
		{
			Time.Stop();
		}
		public void Restart()
		{
			Time.Reset();
			Time.Start();
		}

		public long GetElapsedTicks()
		{
			return Time.ElapsedTicks;
		}
		public long GetElapsedTime()
		{
			return Time.ElapsedMilliseconds;
		}
	}
}
