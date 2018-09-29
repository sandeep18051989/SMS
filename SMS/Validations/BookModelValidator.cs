using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validation
{
	public partial class BookModelValidator : EntityValidatorBase<BookModel>
	{
		public BookModelValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter book name");
			RuleFor(x => x.Price).NotEmpty().WithMessage("Please enter price");
		}
	}
}