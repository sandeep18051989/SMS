using System;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class BookIssueModelValidator : EntityValidatorBase<BookIssueModel>
	{
		public BookIssueModelValidator()
		{
			RuleFor(x => x.BookId).NotEmpty().WithMessage("Please choose a book");
			RuleFor(x => x.LibrarianId).NotEmpty().WithMessage("Please choose issuer name");
			RuleFor(x => x.PenaltyAmount).NotEmpty().WithMessage("Please enter penalty amount");
			RuleFor(x => x.StartDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).WithMessage("Start date is required");
			RuleFor(x => x.EndDate).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(date => date != default(DateTime)).GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date required and should be greater than start date");

		}
	}
}