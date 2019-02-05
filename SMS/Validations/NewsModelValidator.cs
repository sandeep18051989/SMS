using System;
using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class NewsModelValidator : BaseModelValidator<NewsModel>
	{
		public NewsModelValidator()
		{
			RuleFor(x => x.Author).NotEmpty().WithMessage("Please enter author");
			RuleFor(x => x.StartDate).NotEmpty().Must(date => date != default(DateTime)).WithMessage("Start date is required and must be less than or equal to end date");
			RuleFor(x => x.EndDate).NotEmpty().Must(date => date != default(DateTime)).WithMessage("End date is required and must be greater than or equal to start date");
			RuleFor(x => x.Description).NotEmpty().WithMessage("Please write description");
            RuleFor(x => x.NewsStatusId).NotEmpty().WithMessage("Please choose news status");
			RuleFor(x => x.ShortName).NotEmpty().WithMessage("Please choose a short name");
			RuleFor(x => x.SystemName).NotEmpty().WithMessage("Please enter system name without spaces");
			RuleFor(x => x.AcadmicYearId).NotEmpty().WithMessage("Please choose academic year");
		}
	}
}
