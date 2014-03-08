using System;

using Microsoft.Xna.Framework;

namespace VoxLib.Widget
{
	public class ButtonWidget : BaseWidget
	{
		public ButtonWidget( WidgetDialog parent ) : base( parent )
		{
		}

		public override void Initialize()
		{
		}

		public override void Update( GameTime gameTime )
		{
			Event = EWidgetEvent.EMPTY;

			if( MouseInBounds() && Enabled )
			{
				Focused = true;

				if( Input.InputManagerStatic.MouseLeftButtonDownOnce() && Enabled )
					Event = EWidgetEvent.BUTTON_PRESSED;
			}
		}

		public override void Render( GameTime gameTime )
		{
			Parent.SpriteBatch.Begin();

			if( Enabled )
				Parent.SpriteBatch.Draw( WidgetDialog.ButtonTex, Area, Focused ? FocusedColor : RegularColor );
			else
				Parent.SpriteBatch.Draw( WidgetDialog.ButtonTex, Area, DisabledColor );

			if( !string.IsNullOrEmpty( Value ) )
			{
				if( Enabled )
					Parent.SpriteBatch.DrawString( WidgetDialog.Font, Value, new Vector2(Area.Left + (Value.Length / 2), Area.Y - 1), RegularColor );
				else
					Parent.SpriteBatch.DrawString( WidgetDialog.Font, Value, new Vector2(Area.Left + (Value.Length / 2), Area.Y - 1), DisabledColor );
			}

			Parent.SpriteBatch.End();
		}
	}
}
