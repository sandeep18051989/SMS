using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMS.Models
{
	[Validator(typeof(VendorModelValidator))]
	public partial class VendorModel : BaseEntityModel
	{
        public VendorModel()
        {
            AvailableAcadmicYears = new List<SelectListItem>();
        }
        public string RegNumber { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string OfficeContact { get; set; }
		public string MobileContact { get; set; }
		public DateTime? Date { get; set; }
		public int AcadmicYearId { get; set; }
        public bool IsActive { get; set; }
        public string AcadmicYear { get; set; }
        public int ProductCount { get; set; }
		public bool Selected { get; set; }
        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
	}
}
