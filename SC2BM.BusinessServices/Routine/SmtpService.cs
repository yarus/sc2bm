using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using SC2BM.Logging;
using SC2BM.ServiceModel.Responses;
using SC2BM.ServiceModel.RoutineServices;

namespace SC2BM.BusinessFacade.Routine
{
    public class SmtpService : ISmtpService
    {
        private static readonly ILogger Logger = Logging.Logger.Server;

        public SmtpService()
        {
        }

        public ServiceResponse<bool> Send(string toAddress, string subject, string body, bool isBodyHtml = false)
        {
            return new ServiceResponse<bool>(Send(null, toAddress, subject, body, isBodyHtml, null));
        }

        public ServiceResponse<bool> Send(string toAddress, string subject, string body, string attachmentPath, bool isBodyHtml = false)
        {
            return new ServiceResponse<bool>(Send(null, toAddress, subject, body, isBodyHtml, null, null, attachmentPath));
        }

        public ServiceResponse<bool> Send(string toAddress, string subject, string body, byte[] attachmentContent, bool isBodyHtml = false)
        {
            return new ServiceResponse<bool>(Send(null, toAddress, subject, body, isBodyHtml, null, null, null, attachmentContent));
        }

        public ServiceResponse<bool> Send(string toAddress, string subject, string body, Dictionary<string, Stream> attachments, bool isBodyHtml = false)
        {
            return new ServiceResponse<bool>(Send(null, toAddress, subject, body, isBodyHtml, null, null, null, null, attachments));
        }

        public ServiceResponse<bool> Send(string toAddress, string ccAddress, string bccAddress, string subject, string body, bool isBodyHtml = false)
        {
            return new ServiceResponse<bool>(Send(null, toAddress, subject, body, isBodyHtml, ccAddress, bccAddress));
        }

        public ServiceResponse<bool> Send(string toAddress, string ccAddress, string bccAddress, string subject, string body, string attachmentPath, bool isBodyHtml = false)
        {
            return new ServiceResponse<bool>(Send(null, toAddress, subject, body, isBodyHtml, ccAddress, bccAddress, attachmentPath));
        }

        public ServiceResponse<bool> Send(string toAddress, string ccAddress, string bccAddress, string subject, string body, byte[] attachmentContent, bool isBodyHtml = false)
        {
            return new ServiceResponse<bool>(Send(null, toAddress, subject, body, isBodyHtml, ccAddress, bccAddress, null, attachmentContent));
        }

        public ServiceResponse<bool> Send(string toAddress, string ccAddress, string bccAddress, string subject, string body, Dictionary<string, Stream> attachments, bool isBodyHtml = false)
        {
            return new ServiceResponse<bool>(Send(null, toAddress, subject, body, isBodyHtml, ccAddress, bccAddress, null, null, attachments));
        }

        public ServiceResponse<bool> Send(string fromAddress, string toAddress, string subject, string body, byte[] attachmentContent, bool isBodyHtml = false)
        {
            return new ServiceResponse<bool>(Send(fromAddress, toAddress, subject, body, isBodyHtml, null, null, null, attachmentContent));
        }

        public ServiceResponse<bool> Send(string fromAddress, string toAddress, string subject, string body, Dictionary<string, Stream> attachments, bool isBodyHtml = false)
        {
            return new ServiceResponse<bool>(Send(fromAddress, toAddress, subject, body, isBodyHtml, null, null, null, null, attachments));
        }

        public ServiceResponse<bool> Send(string fromAddress, string toAddress, string ccAddress, string bccAddress, string subject, string body, string attachmentPath, bool isBodyHtml = false)
        {
            return new ServiceResponse<bool>(Send(fromAddress, toAddress, subject, body, isBodyHtml, ccAddress, bccAddress, attachmentPath));
        }

        public ServiceResponse<bool> Send(string fromAddress, string toAddress, string ccAddress, string bccAddress, string subject, string body, byte[] attachmentContent, bool isBodyHtml = false)
        {
            return new ServiceResponse<bool>(Send(fromAddress, toAddress, subject, body, isBodyHtml, ccAddress, bccAddress, null, attachmentContent));
        }

        public ServiceResponse<bool> Send(string fromAddress, string toAddress, string ccAddress, string bccAddress, string subject, string body, Dictionary<string, Stream> attachments, bool isBodyHtml = false)
        {
            return new ServiceResponse<bool>(Send(fromAddress, toAddress, subject, body, isBodyHtml, ccAddress, bccAddress, null, null, attachments));
        }

        private bool Send(
            string fromAddress,
            string toAddress,
            string subject,
            string body,
            bool isBodyHtml = false,
            string ccAddress = null,
            string bccAddress = null,
            string attachmentPath = null,
            byte[] attachmentContent = null,
            Dictionary<string, Stream> attachments = null
        )
        {
            var mail = new MailMessage();

            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            if (!string.IsNullOrWhiteSpace(fromAddress))
                mail.From = new MailAddress(fromAddress);
            mail.To.Add(toAddress);
            if (!string.IsNullOrWhiteSpace(ccAddress))
                mail.Bcc.Add(ccAddress);
            if (!string.IsNullOrWhiteSpace(bccAddress))
                mail.Bcc.Add(bccAddress);
            mail.Subject = subject;
            if (isBodyHtml)
                body =
                    "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" +
                    "<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
                    "<head>" +
                    "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />" +
                    "<title>" + subject + "</title>" +
                    "</head>" +
                    "<body>" +
                    body +
                    "</body>" +
                    "</html>";
            mail.Body = body;
            mail.IsBodyHtml = isBodyHtml;

            Stream stream = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(attachmentPath))
                {
                    stream = attachmentContent != null ? (Stream)new MemoryStream(attachmentContent) : new FileStream(attachmentPath, FileMode.Open);
                    var attachementFile = new Attachment(stream, Path.GetFileName(attachmentPath));
                    mail.Attachments.Add(attachementFile);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                stream = null;
                mail.Attachments.Clear();
            }

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    mail.Attachments.Add(new Attachment(attachment.Value, attachment.Key));
                }
            }

            var result = true;

            try
            {
                new SmtpClient().Send(mail);
            }
            catch (Exception e)
            {
                Logger.Error(string.Format("Unable to send email with subject {0} to recipient(s) {1}.", subject, toAddress), e);
                result = false;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return result;
        }
    }
}