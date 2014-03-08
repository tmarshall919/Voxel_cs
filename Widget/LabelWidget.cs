using System;

using Microsoft.Xna.Framework;

namespace VoxLib.Widget
{
	public class LabelWidget : BaseWidget
	{
		public LabelWidget( WidgetDialog parent ) : base( parent )
		{
		}

		public override void Initialize()
		{
		}

		public override void Update( GameTime gameTime )
		{
		}

		public override void Render( GameTime gameTime )
		{
			if( !string.IsNullOrEmpty( Value ) )
			{
				Parent.SpriteBatch.Begin();
				Parent.SpriteBatch.DrawString( WidgetDialog.Font, Value, new Vector2(Area.X, Area.Y), RegularColor );
				Parent.SpriteBatch.End();
			}
		}
	}
}
