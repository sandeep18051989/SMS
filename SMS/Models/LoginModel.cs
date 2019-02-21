using FluentValidation.Attributes;
using SMS.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMS.Models
{
	public partial class LoginModel
	{

		public LoginModel()
		{
			ForgotPassword = new ForgotPasswordModel();
		}

		[Required]
		public string Email { get; set; }

		public string ReturnUrl { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public bool RememberMe { get; set; }

		public ForgotPasswordModel ForgotPassword { get; set; }
	}

    public partial class VerificationModel
    {
        public bool Success { get; set; }

        public string Message { get; set; }
    }

    [Validator(typeof(ResetPasswordModelValidator))]
    public partial class ResetPasswordModel
    {
        public bool IsReset { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string EmailAddress { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}