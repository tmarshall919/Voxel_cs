using System;

namespace VoxLib.Configurations
{
	public static class Configuration
	{
		public static ChunkConfiguration ChunkConfig		{ get; private set; }
		public static WorldConfiguration WorldConfig		{ get; private set; }
		public static InternalConfiguration InternalConfig	{ get; private set; }

		static Configuration()
		{
			
		}

		public static void Setup()
		{
			ChunkConfig = new ChunkConfiguration();
			WorldConfig = new WorldConfiguration();
			InternalConfig = new InternalConfiguration();
		}
	}
}
