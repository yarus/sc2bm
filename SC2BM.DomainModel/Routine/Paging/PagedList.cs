using System.Collections.Generic;

namespace SC2BM.DomainModel.Routine.Paging
{
	public class PagedList<T> : IPagedList<T>
	{
		public IList<T> Result { get; set; }
		public int PageId { get; set; }
		public int TotalCount { get; set; }
	}
}