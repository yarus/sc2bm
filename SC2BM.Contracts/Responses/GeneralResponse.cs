using System;

namespace SC2BM.ServiceModel.Responses
{
    [Serializable]
    public class GeneralResponse : IServiceResponse
    {
        public bool Success { get; private set; }

        public string Message { get; private set; }

        public Exception Exception { get; private set; }

        public virtual object GetResult()
        {
            return null;
        }

        public GeneralResponse()
        {
            Success = true;
            Message = string.Empty;
        }

        public virtual void SetError(Exception exp)
        {
            Success = false;
            Message = exp.Message;
            Exception = exp;
        }
    }
}