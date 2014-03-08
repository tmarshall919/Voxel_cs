using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoxLib.Graphics.Render
{
	public static class TextRender
	{
		public static SpriteBatch	SpriteBatch;
		public static SpriteFont	SpriteFont;

		static TextRender()
		{
		}

		public static void BeginRender()
		{
			SpriteBatch.Begin();
		}
		public static void Render( string strMessage, int x, int y, Color color )
		{
			SpriteBatch.DrawString( SpriteFont, strMessage, new Vector2((float)x, (float)y), color );
		}
		public static void Render( string strMessage, float x, float y, Color color )
		{
			SpriteBatch.DrawString( SpriteFont, strMessage, new Vector2(x, y), color );
		}
		public static void Render( string strMessage, Vector2 vecPosition, Color color )
		{
			SpriteBatch.DrawString( SpriteFont, strMessage, vecPosition, color );
		}

		public static void RenderShaded( string strMessage, int x, int y, Color colorA, Color colorB )
		{
			SpriteBatch.DrawString( SpriteFont, strMessage, new Vector2((float)x, (float)y), colorA );
			SpriteBatch.DrawString( SpriteFont, strMessage, new Vector2((float)x + 1, (float)y + 1), colorB );
		}
		public static void RenderShaded( string strMessage, float x, float y, Color colorA, Color colorB )
		{
			SpriteBatch.DrawString( SpriteFont, strMessage, new Vector2(x + 1, y + 1), colorA );
			SpriteBatch.DrawString( SpriteFont, strMessage, new Vector2(x + 1, y + 1), colorB );
		}
		public static void RenderShaded( string strMessage, Vector2 vecPosition, Color colorA, Color colorB )
		{
			SpriteBatch.DrawString( SpriteFont, strMessage, vecPosition, colorA );
			SpriteBatch.DrawString( SpriteFont, strMessage, vecPosition + new Vector2(1, 1), colorB );
		}

		public static void EndRender()
		{
			SpriteBatch.End();
		}
	}
}
