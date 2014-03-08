using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace VoxLib
{
	public class Camera
	{
		#region Members
		private Matrix				m_matView;
		private Matrix				m_matProj;
		private Vector3				m_vecPos = Vector3.Zero;
		private float				m_fViewAngle = MathHelper.PiOver4;
		private float				m_fNear = 0.01f;
		private float				m_fFar = 260;
		public readonly	Viewport	m_Viewport;

		private PlayerIndex			m_PlayerIndex = PlayerIndex.One;

		private float		m_fRotSpeed = 0.05f;
		private float		m_fLRRot	= 0f;
		private float		m_fUDRot	= 0f;
		private Vector3		m_vecTarget	= Vector3.Zero;
		private	Vector3		m_vecLook	= Vector3.Zero;

		private const float m_fMoveSpeed	= 0.25f;
		private const float m_fRotMoveSpeed	= 0.1f;
#if WINDOWS
		private MouseState	m_mouseMoveState;
		private MouseState	m_mouseState;
#else
		private GamePadState m_GamePadState;
#endif
		#endregion

		#region Accessors
		public float Near
		{
			get{ return m_fNear; }
			set{ m_fNear = value; }
		}
		public float Far
		{
			get{ return m_fFar; }
			set{ m_fFar = value; }
		}
		public PlayerIndex GamePlayerIndex
		{
			get{ return m_PlayerIndex; }
			set{ m_PlayerIndex = value; }
		}
		public Matrix View
		{
			get{ return m_matView; }
			set{ m_matView = value; }
		}
		public Matrix Projection
		{
			get{ return m_matProj; }
			set{ m_matProj = value; }
		}
		public Vector3 Position
		{
			get{ return m_vecPos; }
			set{ m_vecPos = value; CalculateView(); }
		}


		public Vector3 Target { get{ return m_vecTarget; } }

		public Vector3 LookVector
		{
			get{ return m_vecLook; }
			set{ m_vecLook = value; }
		}
		public float LeftRightRot
		{
			get{ return m_fLRRot; }
			set{ m_fLRRot = value; CalculateView(); }
		}
		public float UpDownRot
		{
			get{ return m_fUDRot; }
			set{ m_fUDRot = value; CalculateView(); }
		}
		#endregion

		public Camera( Viewport viewPort )
		{
			m_Viewport = viewPort;
		}

		public void Initialize()
		{
			m_fLRRot = 0f;
			m_fUDRot = 0f;

#if WINDOWS
			m_mouseState = Mouse.GetState();
#endif

			CalculateView();
			CalculateProjection();
		}

		public void LookAt( Vector3 target )
		{
			m_matView = Matrix.CreateLookAt( m_vecPos, target, Vector3.Up );
		}

		public void CalculateProjection()
		{
			m_matProj = Matrix.CreatePerspectiveFieldOfView( m_fViewAngle,
				m_Viewport.AspectRatio, m_fNear, m_fFar );
		}

		public void CalculateView()
		{
			Matrix matRot = Matrix.CreateRotationX( UpDownRot ) *
				Matrix.CreateRotationY( LeftRightRot );
			m_vecLook = Vector3.Transform( Vector3.Forward, matRot );

			m_vecTarget = Position + m_vecLook;
			Vector3 vecCamRot = Vector3.Transform( Vector3.Up, matRot );
			m_matView = Matrix.CreateLookAt( Position, m_vecTarget, vecCamRot );
		}

        public void Update( GameTime gameTime )
        {
#if WINDOWS
			MouseState currMouseState = Mouse.GetState();

			float fMouseX = currMouseState.X - m_mouseMoveState.X;
			float fMouseY = currMouseState.Y - m_mouseMoveState.Y;

			if( fMouseX != 0 )
				LeftRightRot -= m_fRotSpeed * (fMouseX / 50);
			if( fMouseY != 0 )
			{
				UpDownRot -= m_fRotSpeed * (fMouseY / 50);

				float fNewPos = UpDownRot - m_fRotSpeed * (fMouseY / 50);

				if( fNewPos < -1.55f )
					fNewPos = -1.55f;
				else if( fNewPos > 1.55f )
					fNewPos = 1.55f;

				UpDownRot = fNewPos;
			}

			m_mouseMoveState = new MouseState( m_Viewport.Width / 2,
				m_Viewport.Height / 2, 0, ButtonState.Released, ButtonState.Released,
				ButtonState.Released, ButtonState.Released, ButtonState.Released );

			Mouse.SetPosition( (int)m_mouseMoveState.X, (int)m_mouseMoveState.Y );
			m_mouseState = Mouse.GetState();
#else

			GamePadState gamePadState = GamePad.GetState( PlayerIndex.One );
			float fJoystickX = gamePadState.ThumbSticks.Left.X;
			float fJoystickY = gamePadState.ThumbSticks.Left.Y;

			if( fJoystickX != 0f )
				LeftRightRot -= m_fRotSpeed * (fJoystickX / 50);
			if( fJoystickY != 0f )
			{
				float fNewPos = UpDownRot - m_fRotSpeed * (fJoystickY / 50);

				if( fNewPos < -1.55f )
					fNewPos = -1.55f;
				else if( fNewPos > 1.55f )
					fNewPos = 1.55f;

				UpDownRot = fNewPos;
			}
#endif

			CalculateView();
        }

		public void UpdateInput( GameTime gameTime )
		{
			Vector3 vecMove = new Vector3( 0, 0, 0 );

			KeyboardState state = Keyboard.GetState();

			if( state.IsKeyDown( Keys.W ) ) //|| CivilInput.GamePad.LeftThumbstick( m_PlayerIndex ).Y > 0f )
				vecMove += Vector3.Forward;

			if( state.IsKeyDown( Keys.S ) ) //|| CivilInputStatic.GamePadLeftThumbstick( m_PlayerIndex ).Y < 0f )
				vecMove += Vector3.Backward;

			if( state.IsKeyDown( Keys.A ) ) //|| CivilInputStatic.GamePadLeftThumbstick( m_PlayerIndex ).X < 0f )
				vecMove += Vector3.Left;

			if( state.IsKeyDown( Keys.D ) ) //|| CivilInputStatic.GamePadLeftThumbstick( m_PlayerIndex ).X > 0f )
				vecMove += Vector3.Right;

			if( vecMove != Vector3.Zero )
			{
				Matrix matRot = Matrix.CreateRotationX( UpDownRot ) *
					Matrix.CreateRotationY( LeftRightRot );
				Vector3 vecRot = Vector3.Transform( vecMove, matRot );

				Position += vecRot *  m_fMoveSpeed;
			}
		}
	}
}
