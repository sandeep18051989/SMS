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
            RuleFor(x => x.AdmissionStatusId).NotEmpty().WithMessage("Please choose admission status");
            RuleFor(x => x.AcadmicYearId).NotEmpty().WithMessage("Please choose acadmic year");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Please choose date of birth");
			RuleFor(x => x.FatherFName).NotEmpty().WithMessage("Please enter fathers first name");
			RuleFor(x => x.MotherFName).NotEmpty().WithMessage("Please enter mothers first name");
			RuleFor(x => x.FName).NotEmpty().WithMessage("Please enter student first name");
			RuleFor(x => x.Father_Contact).NotEmpty().WithMessage("Please enter fathers contact address");
			RuleFor(x => x.Sex).NotEmpty().WithMessage("Please choose gender");
            RuleFor(x => x.SystemName).NotEmpty().WithMessage("Please enter systemname used for urls");
            RuleFor(x => x.StudentPictureId).NotEmpty().WithMessage("Please upload student picture");
			RuleFor(x => x.Username).NotEmpty().When(x => x.Id > 0).WithMessage("Invalid username for selected student");
            RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress().WithMessage("Please enter valid email address");
        }
	}
}
