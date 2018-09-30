using System;

namespace EF.Core.Data
{
	public partial class Payment : BaseEntity
	{
		public int EmployeeId { get; set; }
		public int AllowanceId { get; set; }
		public string PDate { get; set; }
		public double BasicPay { get; set; }
		public double DA { get; set; }
		public double TA { get; set; }
		public double HRA { get; set; }
		public double PF { get; set; }
		public double Gross_Pay { get; set; }
		public double Net_Pay { get; set; }
		public double TDS { get; set; }
		public virtual Allowance Allowance { get; set; }
	}
}
