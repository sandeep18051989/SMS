using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class FeeDetail : BaseEntity
	{
		public int StudentId { get; set; }
		public int? FeeCategoryStructureId { get; set; }
		public string FeeType { get; set; }
		public int CashierId { get; set; }
        public DateTime? Date { get; set; }
		public string CashierName { get; set; }
		public string PayingMode { get; set; }
		public string BankName { get; set; }
		public string DDChequeNumber { get; set; }
		public string Remarks { get; set; }
		public double TotalFees { get; set; }
		public double FeesPaid { get; set; }
		public string PaidBy { get; set; }
		public int StatusId { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual Student Student { get; set; }

		public virtual AcadmicYear AcadmicYear { get; set; }
		public virtual Employee Employee { get; set; }
		[NotMapped]
		public FeeStatus FeeStatus
		{
			get
			{
				return (FeeStatus)StatusId;
			}
			set
			{
				StatusId = (int)value;
			}
		}
	}
}
