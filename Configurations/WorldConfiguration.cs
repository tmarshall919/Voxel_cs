using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxLib.Configurations
{
	public class WorldConfiguration
	{
		public byte MaxSun			{ get; set; }
		public bool UseRealTime		{ get; set; }
		public int GameFalseTimer	{ get; set; }

		public WorldConfiguration()
		{
			MaxSun = 16;
			UseRealTime = false;
			GameFalseTimer = 20000;
		}

		public void Reroll()
		{
		}
	}
}
