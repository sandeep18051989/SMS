using System;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
	[Validator(typeof(HolidayModelValidator))]
	public partial class HolidayModel : BaseEntityModel
	{
        public HolidayModel()
        {
            AvailableAcadmicYears = new List<SelectListItem>();
        }
        [UIHint("Date")]
        public DateTime? Date { get; set; }
        public string StringDate { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int AcadmicYearId { get; set; }
		public string AcadmicYear { get; set; }
        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
    }
}
