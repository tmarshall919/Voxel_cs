using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VoxLib.Input
{
	public class InputManager
	{
		#region Members
		public GamePadState[]	GamePadState				{ get; private set; }
		public GamePadState[]	PreviousGamePadState		{ get; private set; }
		public KeyboardState	KeyboardState				{ get; private set; }
		public KeyboardState	PreviousKeyboardState		{ get; private set; }
		public MouseState		MouseState					{ get; private set; }
		public MouseState		PreviousMouseState			{ get; private set; }

		private Dictionary<EGameAction, Keys>	m_KeyboardActions;
		private Dictionary<EGameAction, Buttons> m_GamepadActions;
		#endregion

		public InputManager()
		{
			m_KeyboardActions = new Dictionary<EGameAction,Keys>();
			m_GamepadActions = new Dictionary<EGameAction,Buttons>();

			GamePadState = new GamePadState[4];
			PreviousGamePadState = new GamePadState[4];
		}

		public bool LoadActionKeys( string strFilename )
		{

			return true;
		}

		public bool IsActionKeyDown( EGameAction nAction, PlayerIndex nIndex = PlayerIndex.One )
		{
			if( GamePadState[(int)nIndex].IsConnected )
			{
				if( !GamePadButtonDown( nIndex, m_GamepadActions.ElementAt( (int)nAction ).Value ) )
					return KeyboardKeyDown( m_KeyboardActions.ElementAt( (int)nAction ).Value );
				else
					return true;
			}
			else
				return KeyboardKeyDown( m_KeyboardActions.ElementAt( (int)nAction ).Value );
		}
		public bool IsActionKeyDownOnce(  EGameAction nAction, PlayerIndex nIndex = PlayerIndex.One )
		{
			if( GamePadState[(int)nIndex].IsConnected )
			{
				if( !GamePadButtonDownOnce( nIndex, m_GamepadActions.ElementAt( (int)nAction ).Value ) )
					return KeyboardKeyDownOnce( m_KeyboardActions.ElementAt( (int)nAction ).Value );
				else
					return true;
			}
			else
				return KeyboardKeyDownOnce( m_KeyboardActions.ElementAt( (int)nAction ).Value );
		}
		public bool IsActionKeyUp( EGameAction nAction, PlayerIndex nIndex = PlayerIndex.One )
		{
			if( GamePadState[(int)nIndex].IsConnected )
			{
				if( !GamePadButtonUp( nIndex, m_GamepadActions.ElementAt( (int)nAction ).Value ) )
					return KeyboardKeyUp( m_KeyboardActions.ElementAt( (int)nAction ).Value );
				else
					return true;
			}
			else
				return KeyboardKeyUp( m_KeyboardActions.ElementAt( (int)nAction ).Value );
		}
		public bool IsActionKeyUpOnce( EGameAction nAction, PlayerIndex nIndex = PlayerIndex.One )
		{
			if( GamePadState[(int)nIndex].IsConnected )
			{
				if( !GamePadButtonUpOnce( nIndex, m_GamepadActions.ElementAt( (int)nAction ).Value ) )
					return KeyboardKeyUpOnce( m_KeyboardActions.ElementAt( (int)nAction ).Value );
				else
					return true;
			}
			else
				return KeyboardKeyUpOnce( m_KeyboardActions.ElementAt( (int)nAction ).Value );
		}


		public void Update()
		{
			for( int i = 0; i < 4; i++ )
			{
				PreviousGamePadState[i] = GamePadState[i];
				GamePadState[i] = GamePad.GetState( (PlayerIndex)i );
			}

			PreviousKeyboardState = KeyboardState;
			KeyboardState = Keyboard.GetState();

#if WINDOWS
			PreviousMouseState = MouseState;
			MouseState = Mouse.GetState();
#endif
		}


		#region GamePad

		public bool GamePadButtonDown( PlayerIndex nIndex, Buttons button )
		{
			return GamePadState[(int)nIndex].IsButtonDown( button );
		}
		public bool GamePadButtonUp( PlayerIndex nIndex, Buttons button )
		{
			return GamePadState[(int)nIndex].IsButtonUp( button );
		}

		public bool GamePadButtonDownOnce( PlayerIndex nIndex, Buttons button )
		{
			return GamePadState[(int)nIndex].IsButtonDown( button ) && !GamePadState[(int)nIndex].IsButtonDown( button );
		}
		public bool GamePadButtonUpOnce( PlayerIndex nIndex, Buttons button )
		{
			return GamePadState[(int)nIndex].IsButtonUp( button ) && !GamePadState[(int)nIndex].IsButtonUp( button );
		}

		public Vector2 GamePadLeftThumbstick( PlayerIndex nIndex )
		{
			return GamePadState[(int)nIndex].ThumbSticks.Left;
		}
		public Vector2 GamePadRightThumbstick( PlayerIndex nIndex )
		{
			return GamePadState[(int)nIndex].ThumbSticks.Right;
		}
		public float GamePadLeftTrigger( PlayerIndex nIndex )
		{
			return GamePadState[(int)nIndex].Triggers.Left;
		}
		public float GamePadRightTrigger( PlayerIndex nIndex )
		{
			return GamePadState[(int)nIndex].Triggers.Right;
		}

		#endregion

		#region Keyboard

		public bool KeyboardKeyDown( Keys key )
		{
			return KeyboardState.IsKeyDown( key );
		}
		public bool KeyboardKeyUp( Keys key )
		{
			return KeyboardState.IsKeyUp( key );
		}

		public bool KeyboardKeyDownOnce( Keys key )
		{
			return KeyboardState.IsKeyDown( key ) && !PreviousKeyboardState.IsKeyDown( key );
		}
		public bool KeyboardKeyUpOnce( Keys key )
		{
			return KeyboardState.IsKeyUp( key ) && !PreviousKeyboardState.IsKeyUp( key );
		}

		#endregion

		#region Mouse

		public bool MouseLeftButtonDown()
		{
#if WINDOWS
			return MouseState.LeftButton == ButtonState.Pressed;
#else
			return false;
#endif
		}
		public bool MouseMiddleButtonDown()
		{
#if WINDOWS
			return MouseState.MiddleButton == ButtonState.Pressed;
#else
			return false;
#endif
		}
		public bool MouseRightButtonDown()
		{
#if WINDOWS
			return MouseState.RightButton == ButtonState.Pressed;
#else
			return false;
#endif
		}

		public bool MouseLeftButtonDownOnce()
		{
#if WINDOWS
			return MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton != ButtonState.Pressed;
#else
			return false;
#endif
		}
		public bool MouseMiddleButtonDownOnce()
		{
#if WINDOWS
			return MouseState.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton != ButtonState.Pressed;
#else
			return false;
#endif
		}
		public bool MouseRightButtonDownOnce()
		{
#if WINDOWS
			return MouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton != ButtonState.Pressed;
#else
			return false;
#endif
		}

		public int MouseScrollWheelValue
		{
#if WINDOWS
			get{ return MouseState.ScrollWheelValue; }
#else
			get{ return 0; }
#endif
		}
		public int MousePositionX
		{
#if WINDOWS
			get{ return MouseState.X; }
#else
			get{ return 0; }
#endif
		}
		public int MousePositionY
		{
#if WINDOWS
			get{ return MouseState.Y; }
#else
			get{ return 0; }
#endif
		}

		#endregion
	}




	public static class InputManagerStatic
	{
		private static MouseState m_LastMouseState;
		private static MouseState m_MouseState;
		private static KeyboardState m_LastKeyboardState;
		private static KeyboardState m_KeyboardState;
		
		private static GamePadState[] m_LastGamePadState;

		static InputManagerStatic()
		{
			m_LastGamePadState = new GamePadState[4];
			m_LastMouseState = m_MouseState = Mouse.GetState();
			m_LastKeyboardState = m_KeyboardState = Keyboard.GetState();
		}

		public static void Update()
		{
			m_LastKeyboardState = m_KeyboardState;
			m_KeyboardState = Keyboard.GetState();
			m_LastMouseState = m_MouseState;
			m_MouseState = Mouse.GetState();
		}

		#region GamePad

		public static bool GamePadButtonDown( PlayerIndex index, Buttons button )
		{
			return GamePad.GetState( index ).IsButtonDown( button );
		}
		public static bool GamePadButtonUp( PlayerIndex index, Buttons button )
		{
			return GamePad.GetState( index ).IsButtonUp( button );
		}

		public static Vector2 GamePadLeftThumbstick( PlayerIndex index )
		{
			return GamePad.GetState( index ).ThumbSticks.Left;
		}
		public static Vector2 GamePadRightThumbstick( PlayerIndex index )
		{
			return GamePad.GetState( index ).ThumbSticks.Right;
		}

		public static float GameLeftTrigger( PlayerIndex index )
		{
			return GamePad.GetState( index ).Triggers.Left;
		}
		public static float GamePadRightTrigger( PlayerIndex index )
		{
			return GamePad.GetState( index ).Triggers.Right;
		}

		#endregion

		#region Keyboard

		public static bool KeyboardKeyDown( Keys key )
		{
			return m_KeyboardState.IsKeyDown( key );
		}
		public static bool KeyboardKeyDownOnce( Keys key )
		{
			return m_KeyboardState.IsKeyDown( key ) && !m_LastKeyboardState.IsKeyDown( key );
		}
		public static bool KeyboardKeyUp( Keys key )
		{
			return m_KeyboardState.IsKeyUp( key );
		}
		public static bool KeyboardKeyUpOnce( Keys key )
		{
			return m_KeyboardState.IsKeyUp( key ) && !m_LastKeyboardState.IsKeyUp( key );
		}

		#endregion

		#region Mouse

		public static bool MouseLeftButtonDown()
		{
#if WINDOWS
			return m_MouseState.LeftButton == ButtonState.Pressed;
#else
			return false;
#endif
		}
		public static bool MouseLeftButtonDownOnce()
		{
#if WINDOWS
			return m_MouseState.LeftButton == ButtonState.Pressed &&
				m_LastMouseState.LeftButton == ButtonState.Released;
#else
			return false;
#endif
		}
		public static bool MouseMiddleButtonDown()
		{
#if WINDOWS
			return m_MouseState.MiddleButton == ButtonState.Pressed;
#else
			return false;
#endif
		}
		public static bool MouseMiddleButtonDownOnce()
		{
#if WINDOWS
			return m_MouseState.MiddleButton == ButtonState.Pressed &&
				m_LastMouseState.MiddleButton == ButtonState.Released;
#else
			return false;
#endif
		}
		public static bool MouseRightButtonDown()
		{
#if WINDOWS
			return m_MouseState.RightButton == ButtonState.Pressed;
#else
			return false;
#endif
		}
		public static bool MouseRightButtonDownOnce()
		{
#if WINDOWS
			return m_MouseState.RightButton == ButtonState.Pressed &&
				m_LastMouseState.RightButton == ButtonState.Released;
#else
			return false;
#endif
		}

		public static int MouseScrollWheelValue
		{
#if WINDOWS
			get{ return Mouse.GetState().ScrollWheelValue; }
#else
			get{ return 0; }
#endif
		}

		public static int MousePositionX
		{
#if WINDOWS
			get{ return Mouse.GetState().X; }
#else
			get{ return 0; }
#endif
		}
		public static int MousePositionDeltaX
		{
			get{ return m_MouseState.X - m_LastMouseState.X; }
		}
		public static int MousePositionY
		{
#if WINDOWS
			get{ return Mouse.GetState().Y; }
#else
			get{ return 0; }
#endif
		}
		public static int MousePositionDeltaY
		{
			get{ return m_MouseState.Y - m_LastMouseState.Y; }
		}

		#endregion
	}

	public class KeyboardInput
	{
		public bool m_bAllowNumbers = true;
		public bool m_bAllowLetters = true;
		public bool m_bAllowSpecials = true;
		public bool m_bAllowSpaces = true;
		public int m_nMaxAmount = 600;
		public string m_strText { get; set; }
		private KeyboardState m_CurrState;
		private KeyboardState m_PrevState;
		private DateTime m_PrevTime = DateTime.Now;
		private const int m_nUpdateTime = 60;

		public KeyboardInput()
		{
			m_strText = "";
		}

		public void Update( GameTime gameTime )
		{
			m_CurrState = Keyboard.GetState();
			DateTime currTime = DateTime.Now;

			if( currTime.Subtract(m_PrevTime).Milliseconds < m_nUpdateTime )
				return;

			string strText = Convert( m_CurrState.GetPressedKeys() );

			foreach( char i in strText )
			{
				if( i == '\b' )
				{
					if( m_strText.Length > 0 )
						m_strText = m_strText.Remove( m_strText.Length - 1, 1 );
				}
				else if( m_strText.Length < m_nMaxAmount )
					m_strText += i;
			}

			if( !string.IsNullOrEmpty( strText ) )
				m_PrevTime = currTime;

			m_PrevState = m_CurrState;
		}
		
		private string Convert( Keys[] keys )
		{
			string strRet = "";
			bool bIsShiftDown = (keys.Contains(Keys.LeftShift) || keys.Contains(Keys.RightShift));

			foreach( Keys keyDown in keys )
			{
				if( m_PrevState.IsKeyDown( keyDown ) && keyDown != Keys.Back )
					continue;

				if( keyDown >= Keys.A && keyDown <= Keys.Z && m_bAllowLetters )
					strRet += keyDown.ToString();
				else if( keyDown >= Keys.NumPad0 && keyDown <= Keys.NumPad9 && m_bAllowNumbers )
					strRet += ((int)(keyDown - Keys.NumPad0)).ToString();
				else if( keyDown == Keys.Back )
					strRet += "\b";
				else if( keyDown == Keys.Space && m_bAllowSpaces )
					strRet += " ";
				else if( keyDown >= Keys.D0 && keyDown <= Keys.D9 )
				{
					#region Numbers & Special Characters, ! -- )
					switch( ((int)(keyDown - Keys.D0)).ToString() )
					{
						case "1":
						{
							if( bIsShiftDown && m_bAllowSpecials )
								strRet += "!";
							else if( !bIsShiftDown && m_bAllowNumbers )
								strRet += "1";

							break;
						}
						case "2":
							if( bIsShiftDown && m_bAllowSpecials )
								strRet += "@";
							else if( !bIsShiftDown && m_bAllowNumbers )
								strRet += "2";

							break;
						case "3":
							if( bIsShiftDown && m_bAllowSpecials )
								strRet += "#";
							else if( !bIsShiftDown && m_bAllowNumbers )
								strRet += "3";

							break;
						case "4":
							if( bIsShiftDown && m_bAllowSpecials )
								strRet += "$";
							else if( !bIsShiftDown && m_bAllowNumbers )
								strRet += "4";

							break;
						case "5":
							if( bIsShiftDown && m_bAllowSpecials )
								strRet += "%";
							else if( !bIsShiftDown && m_bAllowNumbers )
								strRet += "5";

							break;
						case "6":
							if( bIsShiftDown && m_bAllowSpecials )
								strRet += "^";
							else if( !bIsShiftDown && m_bAllowNumbers )
								strRet += "6";

							break;
						case "7":
							if( bIsShiftDown && m_bAllowSpecials )
								strRet += "&";
							else if( !bIsShiftDown && m_bAllowNumbers )
								strRet += "7";

							break;
						case "8":
							if( bIsShiftDown && m_bAllowSpecials )
								strRet += "*";
							else if( !bIsShiftDown && m_bAllowNumbers )
								strRet += "8";

							break;
						case "9":
							if( bIsShiftDown && m_bAllowSpecials )
								strRet += "(";
							else if( !bIsShiftDown && m_bAllowNumbers )
								strRet += "9";

							break;
						case "0":
							if( bIsShiftDown && m_bAllowSpecials )
								strRet += ")";
						else if( !bIsShiftDown && m_bAllowNumbers )
								strRet += "0";

							break;
						default:
							break;
					}
					#endregion
				}
			}

			if( !bIsShiftDown )
				strRet = strRet.ToLower();

			return strRet;
		}
	}
}
