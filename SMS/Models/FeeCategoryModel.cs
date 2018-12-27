using System;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMS.Models
{
	[Validator(typeof(FeeCategoryModelValidator))]
	public partial class FeeCategoryModel : BaseEntityModel
	{
		public FeeCategoryModel()
		{
            AvailableCategories = new List<SelectListItem>();
            AvailableAcadmicYears = new List<SelectListItem>();
            AvailableClassDivisions = new List<SelectListItem>();
        }
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public int ClassDivisionId { get; set; }
        public string ClassDivisionName { get; set; }
        public double FeeAmount { get; set; }
		public DateTime? PeriodFrom { get; set; }
		public DateTime? PeriodTo { get; set; }
		public int AcadmicYearId { get; set; }
        public string AcadmicYear { get; set; }
        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
        public IList<SelectListItem> AvailableClassDivisions { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }

    }
}
