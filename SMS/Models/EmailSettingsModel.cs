using System.ComponentModel.DataAnnotations;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(EmailSettingsModelValidator))]
    public class EmailSettingsModel : BaseEntityModel
    {
        public string ActiveSettings { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }

        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public bool EnableSSL { get; set; }

        public string Result { get; set; }
        public string FromEmail { get; set; }

    }
}