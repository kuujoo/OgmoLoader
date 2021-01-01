using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public static class SimplePool<T> where T : new()
	{
		private static Queue<T> _pool = new Queue<T>(10);
		public static void Clear()
		{
			_pool.Clear();
		}
		public static T Obtain()
		{
			if (_pool.Count > 0)
			{
				return _pool.Dequeue();
			}
			return new T();
		}		
		public static void Free(T obj)
		{
			_pool.Enqueue(obj);
			if (obj is IResettable)
			{
				((IResettable)obj).Reset();
			}
		}
	}
}