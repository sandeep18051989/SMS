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
			RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter email address");
			RuleFor(x => x.AadharCardNo).NotEmpty().Length(16).WithMessage("Please enter aadhar card number");
			RuleFor(x => x.DesignationId).NotEmpty().WithMessage("Please select designation");
			RuleFor(x => x.Contact1).NotEmpty().WithMessage("Please enter valid contact");
			RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Please enter date of birth");
			RuleFor(x => x.EmployeePictureId).NotEmpty().WithMessage("Please upload picture");
			RuleFor(x => x.JoiningDate).NotEmpty().WithMessage("Please select joining date");
            RuleFor(x => x.Username).NotEmpty().When(x => x.Id > 0).WithMessage("Invalid username for selected employee");
        }
    }
}
