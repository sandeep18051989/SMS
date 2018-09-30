using System;
using System.Collections.Generic;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class EmployeeModelValidator : EntityValidatorBase<EmployeeModel>
	{
		public EmployeeModelValidator()
		{
			RuleFor(x => x.EmpFName).NotEmpty().WithMessage("Please enter first name");
			RuleFor(x => x.EmpLName).NotEmpty().WithMessage("Please enter last name");
			RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter email address");
			RuleFor(x => x.AadharCardNo).NotEmpty().WithMessage("Please enter aadhar card number");
			RuleFor(x => x.AllowanceId).NotEmpty().WithMessage("Please select allowance");
			RuleFor(x => x.Contact1).NotEmpty().WithMessage("Please enter valid contact");
			RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Please enter date of birth");
			RuleFor(x => x.DesignationId).NotEmpty().WithMessage("Please select designation");
			RuleFor(x => x.Emergency_Contact).NotEmpty().WithMessage("Please enter emergency contact");
			RuleFor(x => x.EmployeePictureId).NotEmpty().WithMessage("Please upload picture");
			RuleFor(x => x.FatherFName).NotEmpty().WithMessage("Please enter fathers first name");
			RuleFor(x => x.FatherLName).NotEmpty().WithMessage("Please enter fathers last name");
			RuleFor(x => x.JoiningDate).NotEmpty().WithMessage("Please select joining date");
		}
	}
}
