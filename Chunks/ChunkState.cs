using System;

namespace VoxLib.Chunks
{
	public enum EChunkState : byte
	{
		AwaitingGeneration = 0,
		Generating,
		AwaitingLighting,
		AwaitingRelighting,
		Lighting,
		AwaitingBuild,
		AwaitingRebuild,
		Building,
		AwaitingRemoval,
		Removing,
		Ready,

		ENDOFENUM
	}
}
