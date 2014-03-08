using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoxLib.Blocks
{
	public struct BlockVertexPositionTextureLight : IVertexType
	{
		#region Members
		private Vector3 m_vecPos;
        private Vector2 m_vecTexCoord;
        private float	m_fSunLight;
        private Vector3 m_vecLocalLight;
		#endregion

		#region Accessors
		public Vector3 Position { get { return m_vecPos; } set { m_vecPos = value; } }
		public Vector2 TextureCoordinate1 { get { return m_vecTexCoord; } set { m_vecTexCoord = value; } }
		public Vector3 LocalLight { get { return m_vecLocalLight; } set { m_vecLocalLight = value; } }
		public float SunLight { get { return m_fSunLight; } set { m_fSunLight = value; } }
		public static int SizeInBytes { get { return sizeof(float) * 8; } }
		#endregion

		public static readonly VertexElement[] VertexElements = new VertexElement[]
		{ 
			new VertexElement( 0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0 ),
			new VertexElement( sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0 ),
			new VertexElement( sizeof(float) * 5, VertexElementFormat.Single, VertexElementUsage.Color, 0 ),
			new VertexElement( sizeof(float) * 6, VertexElementFormat.Vector3, VertexElementUsage.Color, 1 )   
		};

		public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(VertexElements);
		VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }

		public BlockVertexPositionTextureLight( Vector3 vecPos, Vector2 vecTexCoord, float fSunLight, Vector3 vecLocalLight )
		{
			m_vecPos = vecPos;
			m_vecTexCoord = vecTexCoord;
			m_fSunLight = fSunLight;
			m_vecLocalLight = vecLocalLight;
		}

		public override String ToString()
		{
			return "(" + m_vecPos + "),(" + m_vecTexCoord + "),(" + m_vecTexCoord + ")";
		}
    }
}
