using System;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(ClassHomeworkModelValidator))]
	public partial class ClassHomeworkModel : BaseEntityModel
	{
		public ClassHomeworkModel()
		{
			Class = new ClassModel();
			Homework = new HomeworkModel();
		}
		public int ClassId { get; set; }
		public int HomeworkId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public HomeworkModel Homework { get; set; }
		public ClassModel Class { get; set; }
	}
}
