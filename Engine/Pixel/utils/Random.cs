using System;
using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
	public static class Random
	{
		private static int _seed = Environment.TickCount;
		static System.Random _rng = new System.Random(_seed);

		public static void SetSeed(int seed)
		{
			_seed = seed;
			_rng = new System.Random(_seed);
		}
		public static float NextFloat(float max)
		{
			return (float)_rng.NextDouble() * max;
		}
		public static int NextInt(int max)
		{
			return _rng.Next(max);
		}
		public static int Range(int min, int max)
		{
			return _rng.Next(min, max);
		}
		public static float Range(float min, float max)
		{
			return min + NextFloat(max - min);
		}
		public static Vector2 Range(Vector2 min, Vector2 max)
        {
			return new Vector2(Range(min.X, max.X), Range(min.Y, max.Y));
        }
		public static Microsoft.Xna.Framework.Vector2 Point(Microsoft.Xna.Framework.Vector2 min, Microsoft.Xna.Framework.Vector2 max)
		{
			return min + new Microsoft.Xna.Framework.Vector2(NextFloat(max.X - min.X), NextFloat(max.Y - min.Y));
		}
	}
}