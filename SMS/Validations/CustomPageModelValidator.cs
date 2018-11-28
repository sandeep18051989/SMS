using EF.Services;
using FluentValidation;
using FluentValidation.Results;
using SMS.Models;

namespace SMS.Validations
{
	public class CustomPageModelValidator : AbstractValidator<CustomPageModel>
	{
		public CustomPageModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter name");
			RuleFor(x => x.SystemName).NotEmpty().WithMessage("Please enter system name");
		    RuleFor(x => x.TemplateId).NotEqual(0).WithMessage("Please select template");
		    
        }
    }
}