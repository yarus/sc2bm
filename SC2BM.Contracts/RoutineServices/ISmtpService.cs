using System.Collections.Generic;
using System.IO;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.ServiceModel.RoutineServices
{
    public interface ISmtpService
    {
        ServiceResponse<bool> Send(string toAddress, string subject, string body, bool isBodyHtml = false);
        ServiceResponse<bool> Send(string toAddress, string subject, string body, string attachmentPath, bool isBodyHtml = false);
        ServiceResponse<bool> Send(string toAddress, string subject, string body, byte[] attachmentContent, bool isBodyHtml = false);
        ServiceResponse<bool> Send(string toAddress, string subject, string body, Dictionary<string, Stream> attachments, bool isBodyHtml = false);
        ServiceResponse<bool> Send(string toAddress, string ccAddress, string bccAddress, string subject, string body, bool isBodyHtml = false);
        ServiceResponse<bool> Send(string toAddress, string ccAddress, string bccAddress, string subject, string body, string attachmentPath, bool isBodyHtml = false);
        ServiceResponse<bool> Send(string toAddress, string ccAddress, string bccAddress, string subject, string body, byte[] attachmentContent, bool isBodyHtml = false);
        ServiceResponse<bool> Send(string toAddress, string ccAddress, string bccAddress, string subject, string body, Dictionary<string, Stream> attachments, bool isBodyHtml = false);
        ServiceResponse<bool> Send(string fromAddress, string toAddress, string subject, string body, byte[] attachmentContent, bool isBodyHtml = false);
        ServiceResponse<bool> Send(string fromAddress, string toAddress, string subject, string body, Dictionary<string, Stream> attachments, bool isBodyHtml = false);
        ServiceResponse<bool> Send(string fromAddress, string toAddress, string ccAddress, string bccAddress, string subject, string body, string attachmentPath, bool isBodyHtml = false);
        ServiceResponse<bool> Send(string fromAddress, string toAddress, string ccAddress, string bccAddress, string subject, string body, byte[] attachmentContent, bool isBodyHtml = false);
        ServiceResponse<bool> Send(string fromAddress, string toAddress, string ccAddress, string bccAddress, string subject, string body, Dictionary<string, Stream> attachments, bool isBodyHtml = false);
    }
}