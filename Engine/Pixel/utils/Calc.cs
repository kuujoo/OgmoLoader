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
		public static int Approach(int start, int end, int shift)
		{
			if (start < end)
			{
				return Math.Min(start + shift, end);
			}
			return Math.Max(start - shift, end);
		}
		public static float PointAngle(Vector2 v0, Vector2 v1)
        {
			return (float)Math.Atan2(v1.Y - v0.Y, v1.X - v0.X);
		}
		public static Vector2 Approach(Vector2 start, Vector2 end, float shift)
		{
			var to = end - start;
			float sdist = to.LengthSquared();
			if (sdist == 0 || (shift >= 0 && sdist <= shift * shift))
			{
				return end;
			}
			var d = (float)Math.Sqrt(sdist);
			return new Vector2(start.X + to.X / d * shift, start.Y + to.Y / d * shift);
		}
		public static float AngleDiff(float radians0, float radians1)
		{
			float num;
			for (num = radians1 - radians0; num > (float)Math.PI; num -= (float)Math.PI * 2f)
			{
			}
			for (; num <= -(float)Math.PI; num += (float)Math.PI * 2f)
			{
			}
			return num;
		}
		public static Vector2 LengthDir(float dist, float angle)
        {
			return new Vector2( dist * (float)Math.Cos((double)angle), dist * -(float)Math.Sin((double)angle) );
		}
		public static float AngleApproach(float val, float target, float maxMove)
		{
			return val + Math.Clamp(AngleDiff(val, target), 0f - maxMove, maxMove);
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
