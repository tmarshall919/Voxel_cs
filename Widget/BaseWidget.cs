using System;

using Microsoft.Xna.Framework;

namespace VoxLib.Widget
{
	public abstract class BaseWidget
	{
		#region Members
		public WidgetDialog Parent		{ get; private set; }
		public int ID					{ get; set; }
		public string Name				{ get; set; }
		public string Value				{ get; set; }
		public bool Visible				{ get; set; }
		public bool Enabled				{ get; set; }
		public bool Focused				{ get; set; }
		public Rectangle Area			{ get; set; }
		public EWidgetType Type			{ get; set; }
		public EWidgetEvent Event		{ get; set; }
		
		//TODO : These should be a one-time-set static color.
		public Color RegularColor		{ get; set; }
		public Color FocusedColor		{ get; set; }
		public Color DisabledColor		{ get; set; }
		#endregion

		public BaseWidget( WidgetDialog parent )
		{
			Parent = parent;

			ID = -1;
			Name = Value = "";
			Visible = Enabled = true;
			Focused = false;
			Type = EWidgetType.End;
			Event = EWidgetEvent.EMPTY;
			RegularColor = Color.White;
			FocusedColor = new Color( 0, 0, 75, 126 );
			DisabledColor = Color.LightGray;
		}

		public void SetSizeAndPos( int x, int y, int nWidth, int nHeight )
		{
			Area = new Rectangle( x, y, nWidth, nHeight );
		}

		public bool MouseInBounds()
		{
#if WINDOWS
			int x = Input.InputManagerStatic.MousePositionX;
			int y = Input.InputManagerStatic.MousePositionY;

			return (x >= Area.Left && x <= Area.Right && y <= Area.Bottom && y >= Area.Top);
#else
			return false;
#endif
		}

		public bool InBounds( int x, int y )
		{
			return (x >= Area.Left && x <= Area.Right && y <= Area.Bottom && y >= Area.Top);
		}

		public abstract void Initialize();
		public abstract void Update( GameTime gameTime );
		public abstract void Render( GameTime gameTime );
	}
}
