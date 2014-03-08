using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VoxLib.Blocks;
using VoxLib.Chunks.Generators;
using VoxLib.Chunks.Processors;
using VoxLib.Common.Vector;
using VoxLib.Configurations;
using VoxLib.Debugging;
using VoxLib.Graphics.Render;

namespace VoxLib.Chunks
{
	public class ChunkCache
	{
		private Game	m_Game;

		public static byte CacheRange = Configuration.ChunkConfig.ChunkCacheRange;
		public static byte ViewRange = Configuration.ChunkConfig.ChunkViewRange;

		public static BoundingBox BoundingBox		{ get; set; }
		public BoundingBox CacheRangeBoundingBox	{ get; set; }
		public BoundingBox ViewRangeBoundingBox		{ get; set; }

		private Effect m_BlockEffect;
		private Effect m_WaterEffect;
		private Texture2D m_BlockAtlas;

		public TerrainChunkGenerator	ChunkGenerator			{ get; set; }
		public IChunkProcessor			ChunkLightProcessor		{ get; set; }
		public IChunkProcessor			ChunkVertexProcessor	{ get; set; }

		public ChunkManager				ChunkMgr	{ get; private set; }
		public Camera					Camera		{ get; private set; }
		public bool CacheThreadStarted	{ get; set; }

		public float	TimeOfDay		{ get; set; }

		#region Debug Members
		public bool EnableDebugInfo	{ get; set; }
		public int ChunksDrawn		{ get; set; }
		public int SecondsToRefreshProfiling	{ get; set; }

		private Profiler m_ChunkProfiler = new Profiler();

		private DateTime m_LastGenerationTime = new DateTime();
		public long EstimatedGenerationTime { get; private set; }

		private DateTime m_LastLightingTime = new DateTime();
		public long EstimatedLightingTime	{ get; private set; }

		private DateTime m_LastBuildingTime = new DateTime();
		public long EstimatedBuildingTime	{ get; private set; }
		#endregion

		public ChunkCache( Game game, Camera camera )
		{
			BoundingBox = new Microsoft.Xna.Framework.BoundingBox();
			m_Game = game;
			Camera = camera;
		}

		public static void RerollSettings()
		{
			ChunkCache.CacheRange = Configuration.ChunkConfig.ChunkCacheRange;
			ChunkCache.ViewRange = Configuration.ChunkConfig.ChunkViewRange;
		}

		public void Initialize( int nWorldSeed )
		{
			ChunkGenerator = new TerrainChunkGenerator( nWorldSeed );
			ChunkLightProcessor = new ChunkLightProcessor();
			ChunkVertexProcessor = new ChunkVertexProcessor( m_Game.GraphicsDevice );
			ChunkMgr = new ChunkManager();

			CacheThreadStarted = false;
			TimeOfDay = 12f;
			EnableDebugInfo = false;
			ChunksDrawn = 0;
			SecondsToRefreshProfiling = 5;
			EstimatedBuildingTime = EstimatedGenerationTime = EstimatedLightingTime = 0;
		}

		public void LoadContent()
		{
			m_BlockAtlas = GameAssetManager.FindTexture( GameAssetManager.BlockTextureAtlas );
			m_BlockEffect = GameAssetManager.FindEffect( GameAssetManager.BlockEffect );
			m_WaterEffect = GameAssetManager.FindEffect( GameAssetManager.WaterEffect );
		}

		public void Update( GameTime gameTime )
		{
			UpdateBoundingBoxes();

			if( CacheThreadStarted )
				return;

			Thread thread = new Thread( CacheThread ) { IsBackground = true };
			thread.Start();
			CacheThreadStarted = true;
		}

		public void Render( GameTime gameTime )
		{
			BoundingFrustum viewFrustum = new BoundingFrustum( Camera.View * Camera.Projection );

			m_Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			m_Game.GraphicsDevice.BlendState = BlendState.Opaque;

			m_BlockEffect.Parameters["World"].SetValue( Matrix.Identity );
			m_BlockEffect.Parameters["View"].SetValue( Camera.View );
			m_BlockEffect.Parameters["Projection"].SetValue( Camera.Projection );
			m_BlockEffect.Parameters["CameraPosition"].SetValue( Camera.Position );
			m_BlockEffect.Parameters["Texture1"].SetValue( m_BlockAtlas );
			m_BlockEffect.Parameters["SunColor"].SetValue( Color.White.ToVector4() );
			m_BlockEffect.Parameters["NightColor"].SetValue( Color.Black.ToVector4() );
			m_BlockEffect.Parameters["HorizonColor"].SetValue( Color.White.ToVector4() );
			m_BlockEffect.Parameters["MorningTint"].SetValue( Color.Gold.ToVector4() );
			m_BlockEffect.Parameters["EveningTint"].SetValue( Color.Red.ToVector4() );
			m_BlockEffect.Parameters["timeOfDay"].SetValue( TimeOfDay );
			m_BlockEffect.Parameters["FogNear"].SetValue( 14 * 16 ); //TODO
			m_BlockEffect.Parameters["FogFar"].SetValue( 16 * 16 ); //TODO

			foreach( EffectPass pass in m_BlockEffect.CurrentTechnique.Passes )
			{
				pass.Apply();

				foreach( Chunk chunk in ChunkMgr.Values )
				{
					if( chunk.State != EChunkState.Ready )
						continue;
					if( chunk.VertexBuffer == null || chunk.IndexBuffer == null ||
						chunk.VertexBuffer.VertexCount == 0 )
					{
						continue;
					}

				//	if( !IsChunkInViewRange( chunk ) )
				//		continue;
					if( !chunk.BoundingBox.Intersects(viewFrustum) )
						continue;

					m_Game.GraphicsDevice.SetVertexBuffer( chunk.VertexBuffer );
					m_Game.GraphicsDevice.Indices = chunk.IndexBuffer;
					m_Game.GraphicsDevice.DrawIndexedPrimitives( PrimitiveType.TriangleList, 0, 0,
						chunk.VertexBuffer.VertexCount, 0, chunk.IndexBuffer.IndexCount / 3 );
				}
			}
		}

		public void RenderDebugBoundingBox( GameTime gameTime )
		{
			BoundingFrustum viewFrustum = new BoundingFrustum( Camera.View * Camera.Projection );

			m_Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			m_Game.GraphicsDevice.BlendState = BlendState.Opaque;

			foreach( EffectPass pass in m_BlockEffect.CurrentTechnique.Passes )
			{
				pass.Apply();

				foreach( Chunk chunk in ChunkMgr.Values )
				{
					if( !chunk.BoundingBox.Intersects(viewFrustum) )
						continue;

					//Blue = AwaitingBuild
					//SteelBlue = Building
					//Pink = AwaitingLighting
					//Orange = Lighting
					//Red = AwaitingBuild
					//Gold = Building
					//White = Ready

					switch( chunk.State )
					{
						case EChunkState.AwaitingGeneration:
							BoundingBoxRender.Render( m_Game.GraphicsDevice, chunk.BoundingBox,
								Camera.View, Camera.Projection, Color.Blue );
							break;
						case EChunkState.Generating:
							BoundingBoxRender.Render( m_Game.GraphicsDevice, chunk.BoundingBox,
								Camera.View, Camera.Projection, Color.SteelBlue );
							break;
						case EChunkState.AwaitingLighting:
						case EChunkState.AwaitingRelighting:
							BoundingBoxRender.Render( m_Game.GraphicsDevice, chunk.BoundingBox,
								Camera.View, Camera.Projection, Color.HotPink );
							break;
						case EChunkState.Lighting:
							BoundingBoxRender.Render( m_Game.GraphicsDevice, chunk.BoundingBox,
								Camera.View, Camera.Projection, Color.Orange );
							break;
						case EChunkState.AwaitingBuild:
						case EChunkState.AwaitingRebuild:
							BoundingBoxRender.Render( m_Game.GraphicsDevice, chunk.BoundingBox,
								Camera.View, Camera.Projection, Color.Red );
							break;
						case EChunkState.Building:
							BoundingBoxRender.Render( m_Game.GraphicsDevice, chunk.BoundingBox,
								Camera.View, Camera.Projection, Color.Gold );
							break;
						case EChunkState.Ready:
							BoundingBoxRender.Render( m_Game.GraphicsDevice, chunk.BoundingBox,
								Camera.View, Camera.Projection, Color.White );
							break;

					}
				}
			}
		}

		private void UpdateBoundingBoxes()
		{
			Chunk currChunk = GetChunk( (int)Camera.Position.X, (int)Camera.Position.Z );
			if( currChunk != null )
			{
				ViewRangeBoundingBox = new BoundingBox(
					new Vector3(currChunk.WorldPosition.X - (ViewRange * Chunk.ChunkWidth), 0,
							currChunk.WorldPosition.Z - (ViewRange * Chunk.ChunkDepth)),
					new Vector3(currChunk.WorldPosition.X + ((ViewRange + 1) * Chunk.ChunkWidth),
							Chunk.ChunkHeight, currChunk.WorldPosition.Z + ((ViewRange + 1) * Chunk.ChunkDepth)) );

				CacheRangeBoundingBox = new BoundingBox(
					new Vector3(currChunk.WorldPosition.X - (CacheRange * Chunk.ChunkWidth), 0,
							currChunk.WorldPosition.Z - (CacheRange * Chunk.ChunkDepth)),
					new Vector3(currChunk.WorldPosition.X + ((CacheRange + 1) * Chunk.ChunkWidth),
							Chunk.ChunkHeight, currChunk.WorldPosition.Z + ((CacheRange + 1) * Chunk.ChunkDepth)) );
			}
		}

		private void CacheThread()
		{
			while( true )
			{
				foreach( Chunk chunk in ChunkMgr.Values )
				{
					if( IsChunkInViewRange( chunk ) )
						ProcessChunkInViewRange( chunk );
					else
					{
						if( IsChunkInCacheRange( chunk ) )
							ProcessChunkInCacheRange( chunk );
						else
						{
							chunk.State = EChunkState.AwaitingRemoval;
							ChunkMgr.Remove( chunk.Position.X, chunk.Position.Z );
							chunk.Dispose( true );
						}
					}
				}

				System.Threading.Thread.Sleep( 8 );
				RecacheChunks();
			}
		}
		private void ProcessChunkInViewRange( Chunk chunk )
		{
			bool bRefreshProfile = false;

			switch( chunk.State )
			{
				case EChunkState.AwaitingGeneration:
				{
					if( EnableDebugInfo )
					{
						if( DateTime.Now.Subtract(m_LastGenerationTime).Milliseconds < SecondsToRefreshProfiling )
						{
							bRefreshProfile = true;
							m_LastGenerationTime = DateTime.Now;
							m_ChunkProfiler.Restart();
						}
					}

					ChunkGenerator.Generate( chunk );

					if( bRefreshProfile )
					{
						m_ChunkProfiler.Stop();
						EstimatedGenerationTime = m_ChunkProfiler.GetElapsedTime();
					}

					//
					ChunkGenerator.Generate( chunk );
					//

					if( bRefreshProfile )
					{
						m_ChunkProfiler.Stop();
						EstimatedGenerationTime = m_ChunkProfiler.GetElapsedTime();
					}

					break;
				}
				case EChunkState.AwaitingLighting:
				case EChunkState.AwaitingRelighting:
				{
					if( EnableDebugInfo )
					{
						if( DateTime.Now.Subtract(m_LastLightingTime).Milliseconds < SecondsToRefreshProfiling )
						{
							bRefreshProfile = true;
							m_LastLightingTime = DateTime.Now;
							m_ChunkProfiler.Restart();
						}
					}

					//
					ChunkLightProcessor.Process( chunk );
					//

					if( bRefreshProfile )
					{
						m_ChunkProfiler.Stop();
						EstimatedLightingTime = m_ChunkProfiler.GetElapsedTime();
					}

					break;
				}
				case EChunkState.AwaitingBuild:
				case EChunkState.AwaitingRebuild:
				{
					if( EnableDebugInfo )
					{
						if( DateTime.Now.Subtract(m_LastBuildingTime).Milliseconds < SecondsToRefreshProfiling )
						{
							bRefreshProfile = true;
							m_LastBuildingTime = DateTime.Now;
							m_ChunkProfiler.Restart();
						}
					}

					//
					ChunkVertexProcessor.Process( chunk );
					//

					if( bRefreshProfile )
					{
						m_ChunkProfiler.Stop();
						EstimatedBuildingTime = m_ChunkProfiler.GetElapsedTime();
					}

					break;
				}
				default:
					break;
			}
		}
		private void ProcessChunkInCacheRange( Chunk chunk )
		{
			bool bRefreshProfile = false;

			switch( chunk.State )
			{
				case EChunkState.AwaitingGeneration:
				{
					if( EnableDebugInfo )
					{
						if( DateTime.Now.Subtract(m_LastGenerationTime).Milliseconds < SecondsToRefreshProfiling )
						{
							bRefreshProfile = true;
							m_LastGenerationTime = DateTime.Now;
							m_ChunkProfiler.Restart();
						}
					}

					ChunkGenerator.Generate( chunk );

					if( bRefreshProfile )
					{
						m_ChunkProfiler.Stop();
						EstimatedGenerationTime = m_ChunkProfiler.GetElapsedTime();
					}

					break;
				}
					
				case EChunkState.AwaitingLighting:
				{
					if( EnableDebugInfo )
					{
						if( DateTime.Now.Subtract(m_LastLightingTime).Milliseconds < SecondsToRefreshProfiling )
						{
							bRefreshProfile = true;
							m_LastLightingTime = DateTime.Now;
							m_ChunkProfiler.Restart();
						}
					}

					//
					ChunkLightProcessor.Process( chunk );
					//

					if( bRefreshProfile )
					{
						m_ChunkProfiler.Stop();
						EstimatedLightingTime = m_ChunkProfiler.GetElapsedTime();
					}
				
					break;
				}
				default:
					break;
			}
		}
		private void RecacheChunks()
		{
			Chunk currChunk = GetChunk( (int)Camera.Position.X, (int)Camera.Position.Z );
			if( currChunk != null )
			{
				for( int z = -CacheRange; z <= CacheRange; z++ )
				{
					for( int x = -CacheRange; x <= CacheRange; x++ )
					{
						if( ChunkMgr.ContainsKey( currChunk.Position.X + x, currChunk.Position.Z + z ) )
							continue;

						Chunk chunk = new Chunk( new ZVector2i(currChunk.Position.X + x, currChunk.Position.Z + z) );
						ChunkMgr[chunk.Position.X, chunk.Position.Z] = chunk;
					}
				}
			}

			ZVector2i swEdge = new ZVector2i( currChunk.Position.X - ViewRange, currChunk.Position.Z - ViewRange );
			ZVector2i neEdge = new ZVector2i( currChunk.Position.X + ViewRange, currChunk.Position.Z + ViewRange );

			BoundingBox = new BoundingBox(
				new Vector3( swEdge.X * Chunk.ChunkWidth, 0, swEdge.Z * Chunk.ChunkDepth),
				new Vector3( (neEdge.X + 1) * Chunk.ChunkWidth, Chunk.ChunkHeight, (neEdge.Z + 1) * Chunk.ChunkDepth) );
		}





		public Chunk GetChunk( int x, int z )
		{
			if( x < 0 )
				x -= Chunk.ChunkWidth;
			if( z < 0 )
				z -= Chunk.ChunkDepth;

			return !ChunkMgr.ContainsKey(x / Chunk.ChunkWidth, z / Chunk.ChunkDepth) ? null :
				ChunkMgr[x / Chunk.ChunkWidth, z / Chunk.ChunkDepth];
		}
		public bool IsChunkInCacheRange( Chunk chunk )
		{
			return CacheRangeBoundingBox.Contains( chunk.BoundingBox ) == ContainmentType.Contains;
		}
		public bool IsChunkInViewRange( Chunk chunk )
		{
			return ViewRangeBoundingBox.Contains( chunk.BoundingBox ) == ContainmentType.Contains;
		}
		public static bool IsInBounds( int x, int y, int z )
		{
			if( x < BoundingBox.Min.X || y < BoundingBox.Min.Y || z < BoundingBox.Min.Z ||
				x >= BoundingBox.Max.X || y >= BoundingBox.Max.Y || z >= BoundingBox.Max.Z )
			{
				return false;
			}

			return true;
		}
	}
}
