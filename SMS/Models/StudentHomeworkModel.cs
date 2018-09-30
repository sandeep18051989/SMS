using System;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(StudentHomeworkModelValidator))]
	public partial class StudentHomeworkModel : BaseEntityModel
	{
		public StudentHomeworkModel()
		{
			Homework = new HomeworkModel();
			Student = new StudentModel();
		}
		public int StudentId { get; set; }
		public int HomeworkId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public HomeworkModel Homework { get; set; }
		public StudentModel Student { get; set; }
	}
}
