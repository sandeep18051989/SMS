using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(ClassExamModelValidator))]
	public partial class ClassExamModel : BaseEntityModel
	{
		public ClassExamModel()
		{
			Exam = new ExamModel();
			Class = new ClassModel();
			ClassRoom = new ClassRoomModel();
			AvailableResultStatuses = new List<SelectListItem>();
			AvailableGradeSystem = new List<SelectListItem>();
		}
		public int ClassId { get; set; }
		public int ExamId { get; set; }
		public int ResultStatusId { get; set; }
		public int GradeSystemId { get; set; }
		public double MarksObtained { get; set; }
		public ExamModel Exam { get; set; }
		public ClassModel Class { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public bool BreakAllowed { get; set; }
		public DateTime BreakTime { get; set; }
		public int ClassRoomId { get; set; }
		public int StudentGroup { get; set; }

		public ClassRoomModel ClassRoom { get; set; }
		public IList<SelectListItem> AvailableResultStatuses { get; set; }
		public IList<SelectListItem> AvailableGradeSystem { get; set; }
	}
}
