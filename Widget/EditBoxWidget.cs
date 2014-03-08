using System;

using Microsoft.Xna.Framework;
using VoxLib.Input;

namespace VoxLib.Widget
{
	public class EditBoxWidget : BaseWidget
	{
		private KeyboardInput m_Input = new KeyboardInput();
		private int m_nCaretUpdateTime = 650;
		private DateTime m_LastCaretUpdateTime = new DateTime();

		public void ResetInput()
		{
			m_Input.m_strText = "";
			Value = "";
		}

		public EditBoxWidget( WidgetDialog parent ) : base( parent )
		{
		}

		public override void Initialize()
		{
		}

		public override void Update( GameTime gameTime )
		{
			if( Enabled )
			{
				m_Input.Update( gameTime );

				if( MouseInBounds() )
				{
					if( InputManagerStatic.MouseLeftButtonDownOnce() )
						Focused = true;
				}
				else
				{
					if( InputManagerStatic.MouseLeftButtonDownOnce() )
						Focused = false;
				}

				if( Focused )
				{
					if( m_Input.m_strText != null )
						Value = m_Input.m_strText;

					//TODO
					if( DateTime.Now.Subtract(m_LastCaretUpdateTime).Milliseconds > m_nCaretUpdateTime )
					{
						m_LastCaretUpdateTime = DateTime.Now;
					}
				}
			}
		}

		public override void Render( GameTime gameTime )
		{
			Parent.SpriteBatch.Begin();

			Parent.SpriteBatch.Draw( WidgetDialog.TextBarTex, Area, Color.White );

			if( Value != null )
			{
				Vector2 vecSize = WidgetDialog.Font.MeasureString( Value );

				Parent.SpriteBatch.DrawString( WidgetDialog.Font, Value, new Vector2(Area.X + 3, Area.Y + 1), Color.Black );
				Parent.SpriteBatch.Draw( WidgetDialog.CaretTex, new Rectangle((Area.X + 3 + (int)vecSize.X + 2), Area.Y + 3, 16, 16), Color.Black );
			}

			Parent.SpriteBatch.End();
		}
	}
}
