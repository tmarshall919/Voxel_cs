using System;
using System.IO;
using System.Text;
using System.Diagnostics;

using VoxLib.Configurations;

namespace VoxLib
{
	public class VoxLogger
	{
		public static string LogName	{ get; set; }
		private static object LockObject = new object();

		public static void Log( string strMessage )
		{
			if( !Configuration.InternalConfig.EnableLogging )
				return;

			lock( LockObject )
			{
				using( StreamWriter writer = new StreamWriter( LogName + ".txt" ) )
				{
					writer.WriteLine( strMessage );

					if( Configuration.InternalConfig.EnableVSOutput )
						Debug.WriteLine( strMessage );
				}
			}
		}

		public static void Log( string strMessage, params object[] param )
		{
			if( !Configuration.InternalConfig.EnableLogging )
				return;

			lock( LockObject )
			{
				using( StreamWriter writer = new StreamWriter( LogName + ".txt" ) )
				{
					if( param != null )
					{
						string strMsg = GetString( strMessage, param );

						writer.WriteLine( strMsg );

						if( Configuration.InternalConfig.EnableVSOutput )
							Debug.WriteLine( strMsg );
					}
					else
					{
						writer.WriteLine( strMessage );

						if( Configuration.InternalConfig.EnableVSOutput )
							Debug.WriteLine( strMessage );
					}
				}
			}
		}

		

		private static string GetString( string strMessage, params object[] param )
		{
			string strRet = "";

			for( int i = 0; i < strMessage.Length; i++ )
			{
				if( strMessage[i] == '{' )
				{
					if( strMessage[i + 2] == '}' || strMessage[i + 3] == '}' )
					{
						int nIndex = 0;

						if( Int32.TryParse( strMessage[i + 1].ToString(), out nIndex ) )
							strRet += param[nIndex].ToString();
					}
				}

				strRet += strMessage[i];
			}

			return strRet;
		}
	}
}
