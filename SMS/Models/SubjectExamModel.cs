using System.Collections.Generic;
using System.Web.Mvc;
using EF.Core.Enums;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(SubjectExamModelValidator))]
	public partial class SubjectExamModel : BaseEntityModel
	{
		public SubjectExamModel()
		{
			Exam = new ExamModel();
			Subject = new SubjectModel();
			AvailableResultStatuses = new List<SelectListItem>();
		}
		public int SubjectId { get; set; }
		public int ExamId { get; set; }
		public int ResultStatusId { get; set; }
		public double MarksObtained { get; set; }
		public ExamModel Exam { get; set; }
		public SubjectModel Subject { get; set; }
		public IList<SelectListItem> AvailableResultStatuses { get; set; }
	}
}
