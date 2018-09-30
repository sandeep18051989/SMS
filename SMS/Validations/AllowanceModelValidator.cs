using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class AllowanceModelValidator : EntityValidatorBase<AllowanceModel>
	{
		public AllowanceModelValidator()
		{
			RuleFor(x => x.DesignationId).NotEmpty().WithMessage("Please Select Designation");
			//RuleFor(x => x.StartDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).WithMessage("Start date is required");
			//RuleFor(x => x.EndDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date required and should be greater than start date");


			//RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter discription");
			RuleFor(x => x.HRA).NotEmpty().WithMessage("Please Enter HRA");

		}


	}
}
