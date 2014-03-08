using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VoxLib.Widget
{
	public class WidgetDialog
	{
		#region Members
		public SpriteBatch SpriteBatch	{ get; private set; }
		public string Name				{ get; set; }
		public Area DialogArea			{ get; set; }
		public BaseWidget FocusedWidget	{ get; private set; }
		private int ID					{ get; set; }
		private bool Visible			{ get; set; }
		private bool DiagRender			{ get; set; }
		private int DiagRenderPos		{ get; set; }
		private WidgetMap Widgets		{ get; private set; }

		/* Drawing */
		public static Texture2D	ButtonTex;
		public static Texture2D CaretTex;
		public static Texture2D	BackgroundTex;
		public static Texture2D	CheckTex;
		public static SpriteFont	Font;
		#endregion

		public WidgetDialog( GraphicsDevice gd )
		{
			SpriteBatch = new SpriteBatch( gd );
			DiagRenderPos = gd.Viewport.Width - 200;
		}

		public void Initialize()
		{
#if DEBUG
			DiagRender = true;
#endif
			Widgets = new WidgetMap();
			Widgets.InitializeWidgets();
		}

		public void LoadContent()
		{
			ButtonTex = GameAssetManager.FindTexture( "GUI_ButtonTex" );
			TextBarTex = GameAssetManager.FindTexture( "GUI_TextBarTex" );
			BackgroundTex = GameAssetManager.FindTexture( "GUI_BackgroundTex" );
			CheckTex = GameAssetManager.FindTexture( "GUI_Check" );
			Font = GameAssetManager.FindFont( "BasicFont" );
		}

		public bool LoadFromXML( string strFilename )
		{
			XmlReaderSettings ws = new XmlReaderSettings();
			ws.IgnoreWhitespace = true;
			using( XmlReader reader = XmlReader.Create( strFilename, ws ) )
			{
				while( reader.Read() )
				{
					if( reader.NodeType == XmlNodeType.Element )
					{
						if( reader.Name == "dialog" )
						{
							reader.Read();
							ParseXMLDialog( reader );
						}
					}
				}
			}

			return true;
		}
		private void ParseXMLDialog( XmlReader reader )
		{
			if( reader.Name == "name" )
				this.Name = reader.ReadElementContentAsString();
			 if( reader.Name == "id" )
				this.m_nID = reader.ReadElementContentAsInt();
			if( reader.Name == "visible" )
				this.Visible = reader.ReadElementContentAsBoolean();
			if( reader.Name == "background" ) { /*Background*/ reader.ReadElementContentAsBoolean(); }
			if( reader.Name == "children" )
				ParseXMLChildren( reader );
		}
		private void ParseXMLChildren( XmlReader reader )
		{
			while( true )
			{
				reader.Read();
				if( reader.Name == "children" && reader.NodeType == XmlNodeType.EndElement )
					return;

				if( reader.Name == "BUTTON" && reader.NodeType != XmlNodeType.EndElement )
				{
					#region Button
					ButtonWidget widget = new ButtonWidget( this );
					reader.Read();

					if( reader.Name == "id" )
						widget.ID = reader.ReadElementContentAsInt();
					if( reader.Name == "name" )
						widget.Name = reader.ReadElementContentAsString();
					if( reader.Name == "value" )
						widget.Value = reader.ReadElementContentAsString();
					if( reader.Name == "visible" )
						widget.Visible = reader.ReadElementContentAsBoolean();
					if( reader.Name == "enabled" )
						widget.Enabled = reader.ReadElementContentAsBoolean();
					if( reader.Name == "BOUNDS" )
					{
						reader.Read();
						int x = 0, y = 0, w = 0, h = 0;
						x = reader.ReadElementContentAsInt();
						y = reader.ReadElementContentAsInt();
						w = reader.ReadElementContentAsInt();
						h = reader.ReadElementContentAsInt();

						widget.SetSizeAndPos( x, y, w, h );
					}
					#endregion

					m_Widgets.AddWidget( widget );
				}
			}
		}

		public void Update( GameTime gameTime )
		{
			if( Visible )
			{
				BaseWidget widget = Widgets.GetWidgetAtMouse();
				if( widget != null )
				{
					widget.Update( gameTime );

					if( widget.Focused )
						FocusedWidget = widget;
				}

				if( FocusedWidget != null )
					widget.Update( gameTime );
			}
		}

		public void Render( GameTime gameTime )
		{
			if( Visible )
			{
				//Render Background


				Widgets.Render( gameTime );

				#region Debug Info..
				if( DiagRender )
				{
					BaseWidget widget = Widgets.FindFocusedWidget();
					if( widget != null )
					{
						SpriteBatch.Begin();
						SpriteBatch.DrawString( Font, "==Focused Widget==", new Vector2(DiagRenderPos, 32), Color.Black );
						SpriteBatch.DrawString( Font, "==Focused Widget==", new Vector2(DiagRenderPos + 1, 33), Color.White );

						SpriteBatch.DrawString( Font, "Name: " + widget.Name, new Vector2(DiagRenderPos, 48), Color.Black );
						SpriteBatch.DrawString( Font, "Name: " + widget.Name, new Vector2(DiagRenderPos + 1, 49), Color.White );

						SpriteBatch.DrawString( Font, "ID: " + widget.ID, new Vector2(DiagRenderPos, 64), Color.Black );
						SpriteBatch.DrawString( Font, "ID: " + widget.ID, new Vector2(DiagRenderPos + 1, 65), Color.White );

						SpriteBatch.DrawString( Font, "Type: " + widget.Type.ToString(), new Vector2(DiagRenderPos, 80), Color.Black );
						SpriteBatch.DrawString( Font, "Type: " + widget.Type.ToString(), new Vector2(DiagRenderPos + 1, 81), Color.White );

						SpriteBatch.DrawString( Font, "X: " + widget.Area.X + " -- Y: " + widget.Area.Y, new Vector2(DiagRenderPos, 96), Color.Black );
						SpriteBatch.DrawString( Font, "X: " + widget.Area.X + " -- Y: " + widget.Area.Y, new Vector2(DiagRenderPos + 1, 97), Color.White );

						SpriteBatch.DrawString( Font, "Width: " + widget.Area.Width + " -- Height: " + widget.Area.Height, new Vector2(DiagRenderPos, 112), Color.Black );
						SpriteBatch.DrawString( Font, "Width: " + widget.Area.Width + " -- Height: " + widget.Area.Height, new Vector2(DiagRenderPos + 1, 113), Color.White );

						SpriteBatch.DrawString( Font, "StrValue: " + widget.Value, new Vector2(DiagRenderPos, 144), Color.Black );
						SpriteBatch.DrawString( Font, "StrValue: " + widget.Value, new Vector2(DiagRenderPos + 1, 145), Color.White );

						SpriteBatch.End();
					}
				}
				#endregion
			}
		}
	}
}
