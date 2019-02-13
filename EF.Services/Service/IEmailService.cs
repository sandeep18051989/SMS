using EF.Core.Data;
using System.Collections.Generic;

namespace EF.Services.Service
{
    public interface IEmailService
    {
        bool SendMail(string to, string subject, string messageHtml);

        bool SendMailUsingTemplate(string to, string subject, Template template);

        void SendUserWelcomeMessage(User user);

        void SendUserRegistrationMessage(User user);

        void SendNotification(Template messageTemplate,
            IList<DataToken> tokens,
            string toEmailAddress,
            string toName,
            string attachmentFilePath = null,
            string attachmentFileName = null,
            string replyToEmailAddress = null,
            string replyToName = null,
            string subject = null);

        void SendEmail(string subject, string body, string toAddress, string toName, string replyTo = null, string replyToName = null, IEnumerable<string> bcc = null, IEnumerable<string> cc = null, string attachmentFilePath = null, string attachmentFileName = null, int attachedDownloadId = 0, IDictionary<string, string> headers = null);
    }
}
