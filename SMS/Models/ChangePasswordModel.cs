using System.ComponentModel.DataAnnotations;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(ChangePasswordModelValidator))]
	public class ChangePasswordModel : BaseEntityModel
	{
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		[DataType(DataType.Password)]
		public string OldPassword { get; set; }

	}
}