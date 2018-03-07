using System;
using System.Collections.Generic;

namespace SC2BM.ServiceModel.Responses
{
	[Serializable]
	public class ServiceListResponse<T> : GeneralResponse
	{
		public List<T> Result { get; protected set; }

		public ServiceListResponse()
		{
			Result = new List<T>();
		}

		public ServiceListResponse(List<T> data)
		{
			Result = data;
		}

		public override object GetResult()
		{
			return Result;
		}
		
		public override void SetError(Exception exp)
		{
			base.SetError(exp);
			Result = null;
		}
	}
}