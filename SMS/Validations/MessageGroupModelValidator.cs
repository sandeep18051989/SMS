using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class MessageGroupModelValidator : EntityValidatorBase<MessageGroupModel>
	{
		public MessageGroupModelValidator()
		{
			RuleFor(x => x.GroupName).NotEmpty().WithMessage("Please enter group name");
		}
	}
}
