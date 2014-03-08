using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoxLib.Widget
{
	public class WidgetMap
	{
		private Dictionary<int, BaseWidget> m_Widgets = new Dictionary<int, BaseWidget>();

		public WidgetMap()
		{
		}

		public void AddWidget( BaseWidget widget )
		{
			m_Widgets.Add( widget.ID, widget );
		}

		public BaseWidget GetWidgetAtMouse()
		{
			foreach( BaseWidget widget in m_Widgets.Values )
			{
				if( widget.MouseInBounds() )
					return widget;
			}

			return null;
		}

		public BaseWidget FindWidget( int nID )
		{
			foreach( BaseWidget widget in m_Widgets.Values )
			{
				if( widget.ID == nID )
					return widget;
			}

			return null;
		}
		public BaseWidget FindWidget( string strName )
		{
			foreach( BaseWidget widget in m_Widgets.Values )
			{
				if( widget.Name == strName )
					return widget;
			}

			return null;
		}
		public BaseWidget FindFocusedWidget()
		{
			foreach( BaseWidget widget in m_Widgets.Values )
			{
				if( widget == null )
					continue;

				if( widget.Focused )
					return widget;
			}

			return null;
		}
		public Dictionary<int, BaseWidget> GetWidgets()
		{
			return m_Widgets;
		}

		public void InitializeWidgets()
		{
			foreach( BaseWidget wig in m_Widgets.Values )
			{
				if( wig == null )
					continue;

				wig.Initialize();
			}
		}

		public void Update( GameTime gameTime )
		{
			foreach( BaseWidget wig in m_Widgets.Values )
			{
				if( wig == null )
					continue;

				if( wig.Enabled && wig.Visible )
					wig.Update( gameTime );
			}
		}

		public void Render( GameTime gameTime )
		{
			foreach( BaseWidget wig in m_Widgets.Values )
			{
				if( wig == null )
					continue;

				if( wig.Visible )
					wig.Render( gameTime );
			}
		}
	}
}
