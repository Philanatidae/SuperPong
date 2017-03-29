using System;
using System.Collections;
using System.Collections.Generic;

namespace ECS
{
	public class ImmutableList<T> : IEnumerable<T> where T : class
	{
		readonly List<T> _items;

		public int Count
		{
			get
			{
				return _items.Count;
			}
		}

		public ImmutableList(List<T> items)
		{
			_items = items;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		public T this[int key]
		{
			get
			{
				return _items[key];
			}
		}

	}
}
