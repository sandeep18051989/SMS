using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(FeeDetailModelValidator))]
	public partial class FeeDetailModel : BaseEntityModel
	{
		public FeeDetailModel()
		{
            AvailableStudents = new List<SelectListItem>();
            AvailableFeeCategoryStructures = new List<SelectListItem>();
            AvailableStatuses = new List<SelectListItem>();
            AvailableAcadmicYears = new List<SelectListItem>();
        }
		public int StudentId { get; set; }
		public int FeeCategoryStructureId { get; set; }
        public int AcadmicYearId { get; set; }
        public string AcadmicYear { get; set; }
        public string FeeType { get; set; }
		public int DD { get; set; }
		public int MM { get; set; }
		public int YY { get; set; }
		public int Installments { get; set; }
		public DateTime? Date { get; set; }
		public int CashierId { get; set; }
		public string CashierName { get; set; }
		public string PayingMode { get; set; }
		public string BankName { get; set; }
		public string DDChequeNumber { get; set; }
		public string Remarks { get; set; }
		public double TotalFees { get; set; }
		public double FeesPaid { get; set; }
		public string PaidBy { get; set; }
		public int StatusId { get; set; }
		public IList<SelectListItem> AvailableStudents { get; set; }
        public IList<SelectListItem> AvailableFeeCategoryStructures { get; set; }
        public IList<SelectListItem> AvailableStatuses { get; set; }
        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
    }
}
