using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMS.Models
{
	[Validator(typeof(PaymentModelValidator))]
	public partial class PaymentModel : BaseEntityModel
	{
		public PaymentModel()
		{
            AvailableAcadmicYears = new List<SelectListItem>();
            AvailableEmployees = new List<SelectListItem>();
        }
		public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public int AcadmicYearId { get; set; }
        public string AcadmicYear { get; set; }
        public double BasicPay { get; set; }
		public double DA { get; set; }
		public double TA { get; set; }
		public double HRA { get; set; }
		public double PF { get; set; }
        public double ESI { get; set; }
        public double Gross_Pay { get; set; }
		public double Net_Pay { get; set; }
		public double TDS { get; set; }
        public string StringDate { get; set; }
        public IList<SelectListItem> AvailableEmployees { get; set; }
        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
    }
}
