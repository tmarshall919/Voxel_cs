using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VoxLib
{
	public static class GameAssetManager
	{
		public static string BlockTextureAtlas = "Tex_BlockAtlas";
		public static string BlockEffect = "Effect_SolidBlock";
		public static string WaterEffect = "Effect_WaterBlock";

		private struct TexturesContent
		{
			public Texture2D tex;
			public string strName;
		}
		private struct EffectsContent
		{
			public Effect effect;
			public string strName;
		}
		private struct ModelsContent
		{
			public Model model;
			public string strName;
		}
		private struct FontContent
		{
			public SpriteFont font;
			public string strName;
		}

		private static List<TexturesContent> m_Textures;
		private static List<EffectsContent> m_Effects;
		private static List<ModelsContent> m_Models;
		private static List<FontContent> m_Fonts;

		static GameAssetManager()
		{
			m_Textures = new List<TexturesContent>();
			m_Effects = new List<EffectsContent>();
			m_Models = new List<ModelsContent>();
			m_Fonts = new List<FontContent>();
		}

		public static void LoadContent( ContentManager content )
		{
			string[] strFiles;

			#region Textures/Images/etc..
			string[] strTextureDirs = { "GUI", "Images", "Textures" };
			foreach( string strDir in strTextureDirs )
			{
				if( Directory.Exists( content.RootDirectory + "\\" + strDir ) )
				{
					strFiles = Directory.GetFiles( Directory.GetCurrentDirectory() + "\\" + content.RootDirectory + "\\" + strDir + "\\" );
					foreach( string strFile in strFiles )
					{
						string strResourceName = strDir + strFile.Substring( strFile.LastIndexOf("\\") );
						strResourceName = strResourceName.Replace( ".xnb", "" );

						TexturesContent tex = new TexturesContent();
						tex.tex = content.Load<Texture2D>( strResourceName );
						tex.strName = strResourceName.Substring( strResourceName.LastIndexOf("\\") + 1 );

						m_Textures.Add( tex );
					}
				}
			}
			#endregion

			#region Effects
			string[] strEffectsDir = { "Effects" };
			foreach( string strDir in strEffectsDir )
			{
				if( Directory.Exists( content.RootDirectory + "\\" + strDir ) )
				{
					strFiles = Directory.GetFiles( Directory.GetCurrentDirectory() + "\\" + content.RootDirectory + "\\" + strDir + "\\" );
					foreach( string strFile in strFiles )
					{
						string strResourceName = "Effects" + strFile.Substring( strFile.LastIndexOf("\\") );
						strResourceName = strResourceName.Replace( ".xnb", "" );

						EffectsContent effect = new EffectsContent();
						effect.effect = content.Load<Effect>( strResourceName );
						effect.strName = strResourceName.Substring( strResourceName.LastIndexOf("\\") + 1 );

						m_Effects.Add( effect );
					}
				}
			}
			#endregion

			#region Models
			string[] strModelsDir = { "Models" };
			foreach( string strDir in strModelsDir )
			{
				if( Directory.Exists( content.RootDirectory + "\\" + strDir ) )
				{
					strFiles = Directory.GetFiles( Directory.GetCurrentDirectory() + "\\" + content.RootDirectory + "\\" + strDir + "\\" );
					foreach( string strFile in strFiles )
					{
						if( strFile.Contains( "skydometex" ) )
							continue;

						string strResourceName = "Models" + strFile.Substring( strFile.LastIndexOf("\\") );
						strResourceName = strResourceName.Replace( ".xnb", "" );

						ModelsContent mod = new ModelsContent();
						mod.model = content.Load<Model>( strResourceName );
						mod.strName = strResourceName.Substring( strResourceName.LastIndexOf("\\") + 1 );

						m_Models.Add( mod );
					}
				}
			}
			#endregion

			#region Fonts
			string[] strFontsDir = { "Fonts" };
			foreach( string strDir in strFontsDir )
			{
				if( Directory.Exists( content.RootDirectory + "\\" + strDir ) )
				{
					strFiles = Directory.GetFiles( Directory.GetCurrentDirectory() + "\\" + content.RootDirectory + "\\" + strDir + "\\" );
					foreach( string strFile in strFiles )
					{
						string strResourceName = "Fonts" + strFile.Substring( strFile.LastIndexOf("\\") );
						strResourceName = strResourceName.Replace( ".xnb", "" );

						FontContent font = new FontContent();
						font.font = content.Load<SpriteFont>( strResourceName );
						font.strName = strResourceName.Substring( strResourceName.LastIndexOf("\\") + 1 );

						m_Fonts.Add( font );
					}
				}
			}
			#endregion
		}

		public static Texture2D FindTexture( string strName )
		{
			foreach( TexturesContent tex in m_Textures )
			{
				if( tex.strName == strName )
					return tex.tex;
			}

			return null;
		}

		public static Effect FindEffect( string strName )
		{
			foreach( EffectsContent effect in m_Effects )
			{
				if( effect.strName == strName )
					return effect.effect;
			}

			return null;
		}

		public static Model FindModel( string strName )
		{
			foreach( ModelsContent model in m_Models )
			{
				if( model.strName == strName )
					return model.model;
			}
		
			return null;
		}

		public static SpriteFont FindFont( string strName )
		{
			foreach( FontContent font in m_Fonts )
			{
				if( font.strName == strName )
					return font.font;
			}

			return null;
		}
	}
}
