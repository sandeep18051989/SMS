using System;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(DivisionHomeworkModelValidator))]
	public partial class DivisionHomeworkModel : BaseEntityModel
	{
		public DivisionHomeworkModel()
		{
			Homework = new HomeworkModel();
			Division = new DivisionModel();
		}
		public int DivisionId { get; set; }
		public int HomeworkId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public HomeworkModel Homework { get; set; }
		public DivisionModel Division { get; set; }
	}
}
