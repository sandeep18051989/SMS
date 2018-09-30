using System;
using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class NewsModelValidator : EntityValidatorBase<NewsModel>
	{
		public NewsModelValidator()
		{
			RuleFor(x => x.Author).NotEmpty().WithMessage("Please enter author");
			RuleFor(x => x.StartDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).GreaterThan(x => x.EndDate).WithMessage("Start date is required and must be less than or equal to end date");
			RuleFor(x => x.EndDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).LessThan(x => x.StartDate).WithMessage("End date is required and must be greater than or equal to start date");
			RuleFor(x => x.Date).NotEmpty().WithMessage("Please choose date");
			RuleFor(x => x.Description).NotEmpty().WithMessage("Please write description");
			RuleFor(x => x.NewsStatusId).NotEmpty().WithMessage("Please choose news status");
			RuleFor(x => x.ShortName).NotEmpty().WithMessage("Please choose a short name");
			RuleFor(x => x.SystemName).NotEmpty().WithMessage("Please choose a name used for urls");
			RuleFor(x => x.UserId).NotEmpty().WithMessage("Please choose user who is publishing this news");
			RuleFor(x => x.AcadmicYear).NotEmpty().WithMessage("Please choose academic year");
		}
	}
}
