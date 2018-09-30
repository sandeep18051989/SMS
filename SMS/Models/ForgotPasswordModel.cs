using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using EF.Services;
using System.Collections.Generic;
using SMS.Areas.Admin.Validations;

namespace SMS.Models
{
	public class ForgotPasswordModel : BaseEntityModel
	{
		public string name { get; set; }
	}
}