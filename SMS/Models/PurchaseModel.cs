using System;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMS.Models
{
	[Validator(typeof(PurchaseModelValidator))]
	public partial class PurchaseModel : BaseEntityModel
	{
		public PurchaseModel()
		{
            AvailableVendors = new List<SelectListItem>();
            AvailableAcadmicYears = new List<SelectListItem>();

        }
		public string IName { get; set; }
		public double IQuantity { get; set; }
		public double IRate { get; set; }
		public DateTime IPurchaseDate { get; set; }
		public double ITax { get; set; }
		public double ITotal { get; set; }
		public int VendorId { get; set; }
		public int ProductId { get; set; }
		public string Product { get; set; }

        public string AcadmicYear { get; set; }

        public int AcadmicYearId { get; set; }
        public IList<SelectListItem> AvailableVendors { get; set; }

        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
    }
}
