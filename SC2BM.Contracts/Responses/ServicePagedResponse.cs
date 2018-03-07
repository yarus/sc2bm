using System;
using System.Collections.Generic;
using SC2BM.DomainModel.Routine.Paging;

namespace SC2BM.ServiceModel.Responses
{
	[Serializable]
	public class ServicePagedResponse<T> : GeneralResponse
	{
		public IList<T> Items { get; protected set; }
		public int PageNumber { get; private set; }
		public int TotalCount { get; private set; }

		public ServicePagedResponse()
		{
			Items = new List<T>();
		}

		public ServicePagedResponse(DataPage<T> result, int pageId)
		{
			PageNumber = pageId;
			TotalCount = result.TotalCount;
			Items = result.Items;
		}

		public override object GetResult()
		{
			return new
			{
				TotalCount,
				PageNumber,
				Items
			};
		}
		
		public override void SetError(Exception exp)
		{
			base.SetError(exp);
			Items = null;
		}
	}
}