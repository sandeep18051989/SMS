using System;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.Web.Mvc;
using System.Collections.Generic;

namespace SMS.Models
{
	[Validator(typeof(StudentHomeworkModelValidator))]
	public partial class StudentHomeworkModel : BaseEntityModel
	{
		public StudentHomeworkModel()
		{
            AvailableStudentApprovals = new List<SelectListItem>();
            AvailableTeacherApprovals = new List<SelectListItem>();
        }
        public int StudentId { get; set; }
		public int HomeworkId { get; set; }
        public int StudentHomeworkStatusId { get; set; }
        public int TeacherApprovalStatusId { get; set; }

        public string Student { get; set; }
        public string Homework { get; set; }
        public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
        public IList<SelectListItem> AvailableStudentApprovals { get; set; }
        public IList<SelectListItem> AvailableTeacherApprovals { get; set; }

    }
}
