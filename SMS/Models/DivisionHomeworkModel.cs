using System;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.Web.Mvc;
using System.Collections.Generic;

namespace SMS.Models
{
	[Validator(typeof(DivisionHomeworkModelValidator))]
	public partial class DivisionHomeworkModel : BaseEntityModel
	{
        public DivisionHomeworkModel()
        {
            AvailableStudentApprovals = new List<SelectListItem>();
            AvailableTeacherApprovals = new List<SelectListItem>();
        }
        public int DivisionId { get; set; }
        public string Division { get; set; }
        public int ClassId { get; set; }
        public string Class { get; set; }
        public int HomeworkId { get; set; }
        public string Homework { get; set; }
        public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
        public int StudentHomeworkStatusId { get; set; }
        public int TeacherApprovalStatusId { get; set; }

        public IList<SelectListItem> AvailableStudentApprovals { get; set; }
        public IList<SelectListItem> AvailableTeacherApprovals { get; set; }
    }
}
