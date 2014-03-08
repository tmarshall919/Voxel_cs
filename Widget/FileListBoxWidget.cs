using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;

using VoxLib.Graphics.Render;

namespace VoxLib.Widget
{
	public class FileListBoxWidget : BaseWidget
	{
		private struct FileListBoxFiles
		{
			public string strFilename;
			public string strDirectory;
		}

		public string SelectedFile		{ get; set; }
		public string SearchDirectory	{ get; set; }
		private List<FileListBoxFiles> m_Files = new List<FileListBoxFiles>();
		private string[] m_strLastUpdateFiles;

		public FileListBoxWidget( WidgetDialog parent ) : base( parent )
		{
		}

		public override void Initialize()
		{
			if( !string.IsNullOrEmpty( SearchDirectory ) )
			{
				m_strLastUpdateFiles = Directory.GetFiles( SearchDirectory );

				foreach( string str in m_strLastUpdateFiles )
				{
					FileInfo info = new FileInfo( str );
					if( info != null )
					{
						FileListBoxFiles file = new FileListBoxFiles();
						file.strFilename = info.Name;
						file.strDirectory = info.DirectoryName;
						m_Files.Add( file );
					}
				}
			}
		}

		public override void Update( GameTime gameTime )
		{
			if( m_Files.Count <= 0 )
				return;

			if( MouseInBounds() )
			{
				int y = Input.InputManagerStatic.MousePositionY;
				if( y > Area.Top && y < Area.Bottom )
				{
					if( Input.InputManagerStatic.MouseLeftButtonDownOnce() )
					{
						int nValue = (y - (Area.Top - 13)) / m_Files.Count;
						nValue /= TextRender.SpriteFont.LineSpacing;

						try
						{
							SelectedFile = m_Files[nValue].strFilename;
						}
						catch( Exception exc )
						{
						}
					}
				}
			}
			else
				SelectedFile = "";
		}

		public override void Render( GameTime gameTime )
		{
			if( !Visible )
				return;

			Parent.SpriteBatch.Begin();

			/* Background */
			if( Enabled )
				Parent.SpriteBatch.Draw( WidgetDialog.ButtonTex, Area, Focused ? FocusedColor : RegularColor );
			else
				Parent.SpriteBatch.Draw( WidgetDialog.ButtonTex, Area, DisabledColor );

			if( m_Files.Count > 0 )
			{
				int y = Area.Top + 2;

				Color drawColor = Enabled ? RegularColor : DisabledColor;

			//	TextRender.BeginRender();
				foreach( FileListBoxFiles file in m_Files )
				{
					Parent.SpriteBatch.DrawString( TextRender.SpriteFont, file.strFilename, new Vector2(Area.Left + 15, (float)y), drawColor );
					y += TextRender.SpriteFont.LineSpacing;
				}
			//	TextRender.EndRender();
			}

			Parent.SpriteBatch.End();
		}
	}
}
