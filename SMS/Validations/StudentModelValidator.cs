using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class StudentModelValidator : EntityValidatorBase<StudentModel>
	{
		public StudentModelValidator()
		{
			RuleFor(x => x.AadharCardNo).NotEmpty().Length(16).WithMessage("Please enter 16 digit aadhar card number");
			RuleFor(x => x.AdmissionDate).NotEmpty().WithMessage("Please choose admission date");
			RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Please choose date of birth");
			RuleFor(x => x.FatherFName).NotEmpty().WithMessage("Please enter fathers first name");
			RuleFor(x => x.MotherFName).NotEmpty().WithMessage("Please enter mothers first name");
			RuleFor(x => x.FName).NotEmpty().WithMessage("Please enter student first name");
			RuleFor(x => x.Father_Contact).NotEmpty().WithMessage("Please enter fathers contact address");
			RuleFor(x => x.Sex).NotEmpty().WithMessage("Please choose gender");
			RuleFor(x => x.PictureId).NotEmpty().WithMessage("Please upload students latest picture");
			RuleFor(x => x.Username).NotEmpty().WithMessage("Invalid username for selected employee");
		}
	}
}
