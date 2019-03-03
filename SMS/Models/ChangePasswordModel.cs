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
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		public string ConfirmNewPassword { get; set; }

		[DataType(DataType.Password)]
		public string OldPassword { get; set; }

	}
}