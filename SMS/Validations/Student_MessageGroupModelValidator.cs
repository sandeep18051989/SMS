using System;
using EF.Services;
using SMS.Models;
using FluentValidation;

namespace SMS.Validations
{
	public partial class Student_MessageGroupModelValidator : EntityValidatorBase<Student_MessageGroupModel>
	{
		public Student_MessageGroupModelValidator()
		{
			RuleFor(x => x.DDate).NotEmpty().WithMessage("Please choose date");
			RuleFor(x => x.StudentId).NotEmpty().WithMessage("Please choose student");
			RuleFor(x => x.MessageGroupId).NotEmpty().WithMessage("Please choose message group");
		}
	}
}
