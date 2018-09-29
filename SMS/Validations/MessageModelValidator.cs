using System;
using EF.Services;
using FluentValidation;
using SMS.Models;

namespace SMS.Validations
{
	public partial class MessageModelValidator : EntityValidatorBase<MessageModel>
	{
		public MessageModelValidator()
		{
			RuleFor(x => x.MessageGroupId == 0 ? x.StudentId : x.MessageGroupId).NotEmpty().WithMessage("Please select student or message group");
		}
	}
}
