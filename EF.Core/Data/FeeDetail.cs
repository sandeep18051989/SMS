using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class FeeDetail : BaseEntity
	{
		public int StudentId { get; set; }
		public int FeeCategoryStructureId { get; set; }
		public string FeeType { get; set; }
		public int DD { get; set; }
		public int MM { get; set; }
		public int YYYY { get; set; }
		public int Installments { get; set; }
		public DateTime Date { get; set; }
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
		public virtual Student Student { get; set; }
		public virtual Category FeeCategoryStructure { get; set; }
		public virtual Employee Employee { get; set; }
		[NotMapped]
		public PaymentStatus AdmissionStatus
		{
			get
			{
				return (PaymentStatus)this.StatusId;
			}
			set
			{
				this.StatusId = (int)value;
			}
		}
	}
}
