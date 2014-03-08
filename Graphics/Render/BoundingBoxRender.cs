using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoxLib.Graphics.Render
{
	public static class BoundingBoxRender
	{
		#region Fields
		private static BasicEffect m_Effect;
		private static VertexPositionColor[] m_Verts = new VertexPositionColor[8];
		private static int[] m_nIndices = new int[]
		{
			0, 1, 1, 2, 2, 3, 3, 0, 0, 4, 1, 5, 2, 6, 3, 7, 4, 5, 5, 6, 6, 7, 7, 4
		};
		#endregion

		public static void Render( GraphicsDevice gd, BoundingBox box,
			Matrix matView, Matrix matProj, Color color )
		{
			if( m_Effect == null )
			{
				m_Effect = new BasicEffect( gd )
				{
					TextureEnabled = false, VertexColorEnabled = true, LightingEnabled = false
				};
			}

			Vector3[] vecVerts = box.GetCorners();
			for( int i = 0; i < 8; i++ )
			{
				m_Verts[i].Position = vecVerts[i];
				m_Verts[i].Color = color;
			}

			m_Effect.View = matView;
			m_Effect.Projection = matProj;

			foreach( EffectPass pass in m_Effect.CurrentTechnique.Passes )
			{
				pass.Apply();
				gd.DrawUserIndexedPrimitives( PrimitiveType.LineList, m_Verts,
					0, 8, m_nIndices, 0, 4 );
			}
		}
	}
}
