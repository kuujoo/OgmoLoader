using Microsoft.Xna.Framework;
using System;

namespace kuujoo.Pixel
{
    public static class MathExt
    {
		public static float Approach(float start, float end, float shift)
		{
			if (start < end)
			{
				return Math.Min(start + shift, end);
			}
			return Math.Max(start - shift, end);
		}
		public static void UnionRects(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
		{
			result.X = Math.Min(value1.X, value2.X);
			result.Y = Math.Min(value1.Y, value2.Y);
			result.Width = Math.Max(value1.Right, value2.Right) - result.X;
			result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;
		}
	}
}