using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMS.Models
{
	[Validator(typeof(ExamModelValidator))]
	public partial class ExamModel : BaseEntityModel
	{
        public ExamModel()
        {
            AvailableAcadmicYears = new List<SelectListItem>();
            AvailableClassRooms = new List<SelectListItem>();
        }
		public string ExamName { get; set; }
        [UIHint("DateRange")]
        public DateTime? StartDate { get; set; }
        [UIHint("DateRange")]
        public DateTime? EndDate { get; set; }
        public double PassingMarks { get; set; }
        public double MaxMarks { get; set; }
        public int AcadmicYearId { get; set; }
        public bool Selected { get; set; }
        public string AcadmicYear { get; set; }

        public string StringStartDate { get; set; }

        public string StringEndDate { get; set; }
        public bool IsActive { get; set; }

        public IList<SelectListItem> AvailableAcadmicYears { get; set; }

        public IList<SelectListItem> AvailableClassRooms { get; set; }
    }
}
