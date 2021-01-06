/**
 * Mostly Cherry picked from the Nez Framework: https://github.com/prime31/Nez
 * ---------------------------------------------------------------------------------
		The MIT License (MIT)

		Copyright (c) 2016 Mike, 2020 Joonas Kuusela 

		Permission is hereby granted, free of charge, to any person obtaining a copy
		of this software and associated documentation files (the "Software"), to deal
		in the Software without restriction, including without limitation the rights
		to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
		copies of the Software, and to permit persons to whom the Software is
		furnished to do so, subject to the following conditions:

		The above copyright notice and this permission notice shall be included in all
		copies or substantial portions of the Software.

		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
		IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
		FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
		AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
		LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
		OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
		SOFTWARE.
* ------------------------------------------------------------------------------------
*/


namespace kuujoo.Pixel
{	public interface ICoroutine
	{
		void Stop();
		ICoroutine SetUseUnscaledDeltaTime(bool useUnscaledDeltaTime);
	}

	public static class Coroutine
	{
		public static object WaitForSeconds(float seconds)
		{
			return kuujoo.Pixel.WaitForSeconds.waiter.Wait(seconds);
		}
	}

	class WaitForSeconds
	{
		internal static WaitForSeconds waiter = new WaitForSeconds();
		internal float waitTime;

		internal WaitForSeconds Wait(float seconds)
		{
			waiter.waitTime = seconds;
			return waiter;
		}
	}
}