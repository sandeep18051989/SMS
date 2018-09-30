using EF.Core.Data;
using EF.Core.Enums;
using System.Collections.Generic;

namespace EF.Services.Service
{
    public interface IEmailService
    {
        bool SendMail(string to, string subject, string messageHtml);

        bool SendMailUsingTemplate(string to, string subject, Template template);
    }
}
