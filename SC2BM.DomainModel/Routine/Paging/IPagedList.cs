using System.Collections.Generic;

namespace SC2BM.DomainModel.Routine.Paging
{
	public interface IPagedList<T>
	{
		IList<T> Result { get; set; }
		int PageId { get; set; }
		int TotalCount { get; set; }
	}
}