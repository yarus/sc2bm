using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.Unity.InterceptionExtension;
using SC2BM.Logging;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.BusinessFacade.Unity
{
	public class ExceptionHandlingInterceptorBehavior : IInterceptionBehavior
	{
		private readonly ILogger _logger = Logger.Common;

		public IEnumerable<Type> GetRequiredInterfaces()
		{
			return Type.EmptyTypes;
		}

		public bool WillExecute
		{
			get { return true; }
		}
		
		public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
		{
			IMethodReturn result = getNext()(input, getNext);

			if (result.Exception != null)
			{
				_logger.Error(result.Exception);

				if (result.ReturnValue == null)
				{
					MethodInfo m = input.MethodBase as MethodInfo;
					if (m != null)
					{
						IServiceResponse response = Activator.CreateInstance(m.ReturnType) as IServiceResponse;
						if (response != null)
						{
							response.SetError(result.Exception);
							result.ReturnValue = response;
							result.Exception = null;
						}
					}
				}
			}
			
			return result;
		}
	}
}