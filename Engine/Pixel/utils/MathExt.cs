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
	}
}