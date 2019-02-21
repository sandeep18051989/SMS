using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace EF.Services.Service
{
    public class EmailService : IEmailService
    {
        #region Fields

        public readonly ISettingService _settingService;
        public readonly ITemplateService _templateService;

        #endregion

        #region Const

        public EmailService(ISettingService settingService, ITemplateService templateService)
        {
            this._settingService = settingService;
            this._templateService = templateService;
        }

        #endregion

        #region Utilities

        public bool SendMail(string to, string subject, string messageHtml)
        {
            string host = "";
            string password = "";
            string username = "";
            bool enableSSL = false;
            bool usedefaultcredentials = false;
            int port = 80;
            string fromEmail = "";
            var _settings = _settingService.GetSettingsByType(SettingTypeEnum.EmailSetting);
            if (_settings.Count > 0)
            {
                foreach (Settings setting in _settings)
                {
                    if (setting.Name.ToLower() == "host")
                    {
                        host = setting.Value;
                    }
                    if (setting.Name.ToLower() == "password")
                    {
                        password = setting.Value;
                    }
                    if (setting.Name.ToLower() == "enablessl")
                    {
                        enableSSL = setting.Value.ToLower() == "true" ? true : false;
                    }
                    if (setting.Name.ToLower() == "usedefaultcredentials")
                    {
                        usedefaultcredentials = setting.Value.ToLower() == "true" ? true : false;
                    }
                    if (setting.Name.ToLower() == "port")
                    {
                        port = Convert.ToInt32(setting.Value);
                    }
                    if (setting.Name.ToLower() == "username")
                    {
                        username = setting.Value;
                    }
                    if (setting.Name.ToLower() == "fromemail")
                    {
                        fromEmail = setting.Value;
                    }
                }

                using (MailMessage mail = new MailMessage())
                {
                    if (subject.ToLower() != "Feedback")
                    {
                        mail.From = new MailAddress(fromEmail);
                    }

                    mail.To.Add(to);
                    mail.Subject = subject;
                    mail.Body = messageHtml;
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.EnableSsl = enableSSL;
                        smtp.Host = host;
                        smtp.Port = port;
                        smtp.UseDefaultCredentials = usedefaultcredentials;
                        smtp.Credentials = new NetworkCredential(username, password);
                        try
                        {
                            smtp.Send(mail);
                            mail.Dispose();
                        }
                        catch (SmtpFailedRecipientException ex)
                        {
                            SmtpStatusCode statusCode = ex.StatusCode;
                            if (statusCode == SmtpStatusCode.MailboxBusy ||
                                statusCode == SmtpStatusCode.MailboxUnavailable ||
                                statusCode == SmtpStatusCode.TransactionFailed)
                            {
                                Thread.Sleep(5000);
                                smtp.Send(mail);
                                return true;
                            }
                            else
                            {
                                // System Log
                                var systemLogger = ContextHelper.Current.Resolve<ISystemLogService>();
                                systemLogger.InsertSystemLog(LogLevel.Error, ex.Message, ex.StackTrace);
                            }
                        }
                        finally
                        {
                            mail.Dispose();
                        }
                    }
                }
            }

            return false;
        }

        public bool SendMailUsingTemplate(string to, string subject, Template template)
        {
            string host = "";
            string password = "";
            string username = "";
            bool enableSSL = false;
            bool usedefaultcredentials = false;
            int port = 80;
            string fromEmail = "";
            var _settings = _settingService.GetSettingsByType(SettingTypeEnum.EmailSetting);
            if (_settings.Count > 0)
            {
                foreach (Settings setting in _settings)
                {
                    if (setting.Name.ToLower() == "host")
                    {
                        host = setting.Value;
                    }
                    if (setting.Name.ToLower() == "password")
                    {
                        password = setting.Value;
                    }
                    if (setting.Name.ToLower() == "enablessl")
                    {
                        enableSSL = setting.Value.ToLower() == "true" ? true : false;
                    }
                    if (setting.Name.ToLower() == "usedefaultcredentials")
                    {
                        usedefaultcredentials = setting.Value.ToLower() == "true" ? true : false;
                    }
                    if (setting.Name.ToLower() == "port")
                    {
                        port = Convert.ToInt32(setting.Value);
                    }
                    if (setting.Name.ToLower() == "username")
                    {
                        username = setting.Value;
                    }
                    if (setting.Name.ToLower() == "fromemail")
                    {
                        fromEmail = setting.Value;
                    }
                }

                if (template != null)
                {
                    // Get Tokens and Replace
                    var _tokens = template.Tokens.ToList();
                    if (_tokens != null && _tokens.Count > 0)
                    {
                        foreach (var tkn in _tokens)
                        {
                            template.BodyHtml = template.BodyHtml.Replace(tkn.Name, tkn.Value);
                        }
                    }

                    using (MailMessage mail = new MailMessage())
                    {
                        if (subject.ToLower() != "Feedback")
                        {
                            mail.From = new MailAddress(fromEmail);
                        }

                        mail.To.Add(to);
                        mail.Subject = subject;
                        mail.Body = template.BodyHtml;
                        mail.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient())
                        {
                            smtp.EnableSsl = enableSSL;
                            smtp.Host = host;
                            smtp.Port = port;
                            smtp.UseDefaultCredentials = usedefaultcredentials;
                            smtp.Credentials = new NetworkCredential(username, password);
                            try
                            {
                                smtp.Send(mail); mail.Dispose();
                            }
                            catch (SmtpFailedRecipientException ex)
                            {
                                SmtpStatusCode statusCode = ex.StatusCode;
                                if (statusCode == SmtpStatusCode.MailboxBusy ||
                                    statusCode == SmtpStatusCode.MailboxUnavailable ||
                                    statusCode == SmtpStatusCode.TransactionFailed)
                                {
                                    Thread.Sleep(5000);
                                    smtp.Send(mail);
                                    return true;
                                }
                                else
                                {
                                    // System Log
                                    var systemLogger = ContextHelper.Current.Resolve<ISystemLogService>();
                                    systemLogger.InsertSystemLog(LogLevel.Error, ex.Message, ex.StackTrace);
                                }
                            }
                            finally
                            {
                                mail.Dispose();
                            }
                        }
                    }
                }

                return false;
            }

            return false;
        }

        #endregion

        #region Methods

        public void SendUserWelcomeMessage(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            // Send Notification To The User
            var templateSetting = _settingService.GetSettingByKey("UserSignInAttempt");
            var tokens = new List<DataToken>();

            var template = _templateService.GetTemplateByName(templateSetting.Value);
            _templateService.AddUserTokens(tokens, user);

            foreach (var dt in tokens)
            {
                template.BodyHtml = CodeHelper.Replace(template.BodyHtml.ToString(), $"[{dt.SystemName}]", dt.Value, StringComparison.InvariantCulture);
            }

            var toEmail = user.Email;
            var toName = user.GetFullName();
            var fromEmail = _settingService.GetSettingByKey("FromEmail");

            SendNotification(template, tokens, toEmail, toName);
        }

        public void SendUserEmailVerificationMessage(User user, string verificationLink)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            // Send Notification To The User
            var templateSetting = _settingService.GetSettingByKey("Email Verification");
            var template = _templateService.GetTemplateByName(templateSetting.Value);

            var tokens = template.Tokens.Count > 0 ? template.Tokens.ToList() : new List<DataToken>();

            if (tokens.Count == 0)
                _templateService.AddUserTokens(tokens, user);

            foreach (var dt in tokens)
            {
                template.BodyHtml = CodeHelper.Replace(template.BodyHtml.ToString(), $"[{dt.SystemName}]", dt.Value, StringComparison.InvariantCulture);
            }

            template.BodyHtml = CodeHelper.Replace(template.BodyHtml.ToString(), $"[RedirecLink]", verificationLink, StringComparison.InvariantCulture);
            var toEmail = user.Email;
            var toName = user.GetFullName();
            var fromEmail = _settingService.GetSettingByKey("FromEmail");

            SendNotification(template, tokens, toEmail, toName);
        }

        public void SendUserForgotPasswordMessage(User user, string passwordLink)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            // Send Notification To The User
            var templateSetting = _settingService.GetSettingByKey("ForgotPassword");
            var template = _templateService.GetTemplateByName(templateSetting.Value);

            var tokens = template.Tokens.Count > 0 ? template.Tokens.ToList() : new List<DataToken>();

            if (tokens.Count == 0)
                _templateService.AddUserTokens(tokens, user);

            foreach (var dt in tokens)
            {
                template.BodyHtml = CodeHelper.Replace(template.BodyHtml.ToString(), $"[{dt.SystemName}]", dt.Value, StringComparison.InvariantCulture);
            }

            template.BodyHtml = CodeHelper.Replace(template.BodyHtml.ToString(), $"[RedirecLink]", passwordLink, StringComparison.InvariantCulture);
            var toEmail = user.Email;
            var toName = user.GetFullName();
            var fromEmail = _settingService.GetSettingByKey("FromEmail");

            SendNotification(template, tokens, toEmail, toName);
        }

        public void SendUserPasswordChangedMessage(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            // Send Notification To The User
            var templateSetting = _settingService.GetSettingByKey("ResetPassword");
            var template = _templateService.GetTemplateByName(templateSetting.Value);

            var tokens = template.Tokens.Count > 0 ? template.Tokens.ToList() : new List<DataToken>();

            if (tokens.Count == 0)
                _templateService.AddUserTokens(tokens, user);

            foreach (var dt in tokens)
            {
                template.BodyHtml = CodeHelper.Replace(template.BodyHtml.ToString(), $"[{dt.SystemName}]", dt.Value, StringComparison.InvariantCulture);
            }
            var toEmail = user.Email;
            var toName = user.GetFullName();
            var fromEmail = _settingService.GetSettingByKey("FromEmail");

            SendNotification(template, tokens, toEmail, toName);
        }

        public void SendUserRegistrationMessage(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            // Send Notification To The User
            var templateSetting = _settingService.GetSettingByKey("NewUserRegister");
            var tokens = new List<DataToken>();

            var template = _templateService.GetTemplateByName(templateSetting.Value);
            _templateService.AddUserTokens(tokens, user);

            foreach (var dt in tokens)
            {
                template.BodyHtml = CodeHelper.Replace(template.BodyHtml.ToString(), $"[{dt.SystemName}]", dt.Value, StringComparison.InvariantCulture);
            }

            var toEmail = user.Email;
            var toName = user.GetFullName();
            var fromEmail = _settingService.GetSettingByKey("FromEmail");

            SendNotification(template, tokens, toEmail, toName);
        }

        public virtual void SendNotification(Template messageTemplate,
            IList<DataToken> tokens,
            string toEmailAddress,
            string toName,
            string attachmentFilePath = null,
            string attachmentFileName = null,
            string replyToEmailAddress = null,
            string replyToName = null,
            string subject = null)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException("messageTemplate");

            if (String.IsNullOrEmpty(subject))
                subject = messageTemplate.Subject;

            //Replace subject and body tokens 
            var subjectReplaced = _templateService.Replace(subject, tokens, false);
            var bodyReplaced = _templateService.Replace(messageTemplate.BodyHtml, tokens, true);

            //limit name length
            toName = CommonHelper.EnsureMaximumLength(toName, 300);

            SendEmail(subject, bodyReplaced, toEmailAddress, toName);
        }

        public void SendEmail(string subject, string body, string toAddress, string toName,
     string replyTo = null, string replyToName = null,
    IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
    string attachmentFilePath = null, string attachmentFileName = null,
    int attachedDownloadId = 0, IDictionary<string, string> headers = null)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(toAddress, toName));
            if (!String.IsNullOrEmpty(replyTo))
            {
                message.ReplyToList.Add(new MailAddress(replyTo, replyToName));
            }

            //BCC
            if (bcc != null)
            {
                foreach (var address in bcc.Where(bccValue => !String.IsNullOrWhiteSpace(bccValue)))
                {
                    message.Bcc.Add(address.Trim());
                }
            }

            //CC
            if (cc != null)
            {
                foreach (var address in cc.Where(ccValue => !String.IsNullOrWhiteSpace(ccValue)))
                {
                    message.CC.Add(address.Trim());
                }
            }

            //content
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            //headers
            if (headers != null)
                foreach (var header in headers)
                {
                    message.Headers.Add(header.Key, header.Value);
                }

            string host = "";
            string password = "";
            string username = "";
            string displayName = "";
            bool enableSSL = false;
            bool usedefaultcredentials = false;
            int port = 80;
            string fromEmail = "";
            var _settings = _settingService.GetSettingsByType(SettingTypeEnum.EmailSetting);
            if (_settings.Count > 0)
            {
                foreach (Settings setting in _settings)
                {
                    if (setting.Name.ToLower() == "host")
                    {
                        host = setting.Value;
                    }
                    if (setting.Name.ToLower() == "password")
                    {
                        password = setting.Value;
                    }
                    if (setting.Name.ToLower() == "enablessl")
                    {
                        enableSSL = setting.Value.ToLower() == "true" ? true : false;
                    }
                    if (setting.Name.ToLower() == "usedefaultcredentials")
                    {
                        usedefaultcredentials = setting.Value.ToLower() == "true" ? true : false;
                    }
                    if (setting.Name.ToLower() == "port")
                    {
                        port = Convert.ToInt32(setting.Value);
                    }
                    if (setting.Name.ToLower() == "username")
                    {
                        username = setting.Value;
                    }
                    if (setting.Name.ToLower() == "displayname")
                    {
                        displayName = setting.Value;
                    }
                    if (setting.Name.ToLower() == "fromemail")
                    {
                        fromEmail = setting.Value;
                    }
                }

                message.From = new MailAddress(fromEmail, displayName);

                //send email
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.UseDefaultCredentials = usedefaultcredentials;
                    smtpClient.Host = host;
                    smtpClient.Port = port;
                    smtpClient.EnableSsl = enableSSL;
                    smtpClient.Credentials = usedefaultcredentials ?
                        CredentialCache.DefaultNetworkCredentials :
                        new NetworkCredential(username, password);
                    smtpClient.Send(message);
                }
            }
        }

        #endregion
    }
}
