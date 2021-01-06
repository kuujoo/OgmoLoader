using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public static class ListPooler<T>
	{
		private static Queue<List<T>> _objectQueue = new Queue<List<T>>();
		public static List<T> Obtain()
		{
			if (_objectQueue.Count > 0)
				return _objectQueue.Dequeue();

			return new List<T>();
		}
		public static void Free(List<T> obj)
		{
			_objectQueue.Enqueue(obj);
			obj.Clear();
		}
	}
}
