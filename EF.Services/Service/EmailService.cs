using EF.Core.Data;
using EF.Core.Enums;
using EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace EF.Services.Service
{
    public class EmailService : IEmailService
    {
        #region Fields

        public readonly ISettingService _settingService;

        #endregion

        #region Const

        public EmailService(ISettingService settingService)
        {
            this._settingService = settingService;
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
                        smtp.Send(mail);
                        return true;
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
                            smtp.Send(mail);
                            return true;
                        }
                    }
                }

                return false;
            }

            return false;
        }

        #endregion
    }
}
