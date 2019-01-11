using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMS.Models
{
	[Validator(typeof(AllowanceModelValidator))]
	public partial class AllowanceModel : BaseEntityModel
	{
		public AllowanceModel()
		{
            AvailableAcadmicYears = new List<SelectListItem>();
            AvailableDesignations = new List<SelectListItem>();
        }
		public int DesignationId { get; set; }
        public int AcadmicYearId { get; set; }
		public double DA { get; set; }
		public double TA { get; set; }
		public double HRA { get; set; }
		public double PF { get; set; }
		public double TDS { get; set; }
        public double BasicPay { get; set; }
        public double ESI { get; set; }
        public double TotalSalary { get; set; }
        public string Designation { get; set; }

        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
        public IList<SelectListItem> AvailableDesignations { get; set; }
    }
}
