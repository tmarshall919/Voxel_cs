using System;

namespace VoxLib.Configurations
{
	public class InternalConfiguration
	{
		public bool EnableLogging	{ get; set; }
		public bool EnableVSOutput	{ get; set; }

		public InternalConfiguration()
		{
			EnableLogging = true;
			EnableVSOutput = true;
		}

		public void Reroll()
		{
		}
	}
}
