using System;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(Student_MessageGroupModelValidator))]
	public partial class Student_MessageGroupModel : BaseEntityModel
	{
		public Student_MessageGroupModel()
		{
			Student = new StudentModel();
			MessageGroup = new MessageGroupModel();
		}
		public DateTime DDate { get; set; }
		public int MessageGroupId { get; set; }
		public int StudentId { get; set; }
		public StudentModel Student { get; set; }
		public MessageGroupModel MessageGroup { get; set; }
	}
}
