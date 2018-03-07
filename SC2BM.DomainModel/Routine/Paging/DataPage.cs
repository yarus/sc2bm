using System.Collections.Generic;

namespace SC2BM.DomainModel.Routine.Paging
{
	public class DataPage<T>
	{
		private readonly List<T> _items;
		private readonly int _totalCount;

		public DataPage(List<T> items, int totalCount)
		{
			_items = items;
			_totalCount = totalCount;
		}

		public int TotalCount
		{
			get { return _totalCount; }
		}

		public List<T> Items
		{
			get { return _items; }
		}
	}
}