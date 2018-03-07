using System;

namespace SC2BM.ServiceModel.Responses
{
	[Serializable]
	public class ServiceResponse<T> : GeneralResponse
	{
		public T Result { get; private set; }

		public ServiceResponse()
		{
			Result = default(T);
		}

		public ServiceResponse(T result)
		{
			Result = result;
		}

		public override object GetResult()
		{
			return Result;
		}
		
		public override void SetError(Exception exp)
		{
			base.SetError(exp);
			Result = default(T);
		}
	}
}