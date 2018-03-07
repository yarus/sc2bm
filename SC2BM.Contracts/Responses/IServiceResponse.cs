using System;

namespace SC2BM.ServiceModel.Responses
{
    public interface IServiceResponse
    {
        bool Success { get; }
        string Message { get; }
        Exception Exception { get; }
        object GetResult();
        void SetError(Exception exp);
    }
}