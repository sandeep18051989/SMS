using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(TimeTableModelValidator))]
	public partial class TimeTableModel : BaseEntityModel
	{
		public TimeTableModel()
		{
			Teacher = new TeacherModel();
			Subject = new SubjectModel();
			Division = new DivisionModel();
			AvailableWeekDays = new List<SelectListItem>();
		}
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public int LectureNumber { get; set; }
		public bool RecessAllowed { get; set; }
		public int WeekDayId { get; set; }

		public int SubjectId { get; set; }
		public int TeacherId { get; set; }
		public int DivisionId { get; set; }
		public DivisionModel Division { get; set; }
		public SubjectModel Subject { get; set; }
		public TeacherModel Teacher { get; set; }
		public IList<SelectListItem> AvailableWeekDays { get; set; }
	}
}
