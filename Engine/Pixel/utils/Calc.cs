using Microsoft.Xna.Framework;
using System;

namespace kuujoo.Pixel
{
    public static class Calc
    {
		public static float Approach(float start, float end, float shift)
		{
			if (start < end)
			{
				return Math.Min(start + shift, end);
			}
			return Math.Max(start - shift, end);
		}
		public static Vector2 Approach(Vector2 start, Vector2 end, float shift)
		{
			return new Vector2(Approach(start.X, end.X, shift), Approach(start.Y, end.Y, shift));
		}
		public static void UnionRects(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
		{
			result.X = Math.Min(value1.X, value2.X);
			result.Y = Math.Min(value1.Y, value2.Y);
			result.Width = Math.Max(value1.Right, value2.Right) - result.X;
			result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;
		}
		public static float DegToRad(float deg)
        {
			return ( (float)Math.PI / 180.0f) * deg;
        }
	}
}
