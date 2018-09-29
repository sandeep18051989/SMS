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
}