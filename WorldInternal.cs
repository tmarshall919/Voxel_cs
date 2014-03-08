using System;

using Microsoft.Xna.Framework;

namespace VoxLib
{
	public static class WorldInternal
	{
		public static float TimeOfDay		{ get; set; }
		public static Vector3 SunPosition	{ get; set; }

		static WorldInternal()
		{
			TimeOfDay = 18f;
			SunPosition = new Vector3( 0f, 0f, 0f );
		}

		public static Vector3 UpdateTOD( GameTime gameTime )
		{
			if( !Configurations.Configuration.WorldConfig.UseRealTime )
				TimeOfDay += ((float)gameTime.ElapsedGameTime.Milliseconds / Configurations.Configuration.WorldConfig.GameFalseTimer );
			else
				TimeOfDay = ((float)DateTime.Now.Hour) + ((float)DateTime.Now.Minute) / 60 +
					(((float)DateTime.Now.Second) / 60) / 60;

			if( TimeOfDay >= 24 )
				TimeOfDay = 0;
			if( TimeOfDay >= 8 && TimeOfDay <= 20 ) { } //Day Time
			else { }									//Night Time

			float x = 0f, y = 0f, z = 0f;

			if( TimeOfDay <= 12 )
			{
				x = 12f - TimeOfDay;
				y = TimeOfDay / 12f;
			}
			else
			{
				x = 12f - TimeOfDay;
				y = (24f - TimeOfDay) / 12f;
			}

			x /= 10f;
			return new Vector3( -x, y, z );
        }
	}
}
