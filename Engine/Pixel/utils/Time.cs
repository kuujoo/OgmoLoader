using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
	public static class Time
	{
		public static float TotalTime;
		public static float DeltaTime;
		public static float UnscaledDeltaTime;
		public static float TimeScale = 1f;
		public static uint FrameCount;

		internal static void Update(float dt)
		{
			TotalTime += dt;
			DeltaTime = dt * TimeScale;
			UnscaledDeltaTime = dt;
			FrameCount++;
		}	
	}
}
