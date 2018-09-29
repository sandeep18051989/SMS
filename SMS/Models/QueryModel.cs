using EF.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	public partial class QueryModel : BaseEntityModel
    {
        public QueryModel()
        {
            EmailSettings = new EmailSettingsModel();
        }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        [AllowHtml]
        public string Description { get; set; }

        public string Result { get; set; }

        public EmailSettingsModel EmailSettings { get; set; }

        public partial class EmailSettingsModel : BaseEntityModel
        {
            public string Host { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public int Port { get; set; }
            public bool EnableSSL { get; set; }
            public bool UseDefaultCredentials { get; set; }
            public string FromEmail { get; set; }
        }
    }
}