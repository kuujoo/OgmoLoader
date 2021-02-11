using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class Pool<T> where T : new()
	{
		private Queue<T> _pool = new Queue<T>(10);
		public void Clear()
		{
			_pool.Clear();
		}
		public T Obtain()
		{
			if (_pool.Count > 0)
			{
				return _pool.Dequeue();
			}
			return new T();
		}		
		public void Free(T obj)
		{
			_pool.Enqueue(obj);
			if (obj is IResettable)
			{
				((IResettable)obj).Reset();
			}
		}
	}
}
