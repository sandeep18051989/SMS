namespace EF.Core.Data
{
	public partial class Allowance : BaseEntity
	{
		public int DesignationId { get; set; }
		public double DA { get; set; }
		public double TA { get; set; }
		public double HRA { get; set; }
		public double PF { get; set; }
		public double TDS { get; set; }
		public double BasicPay { get; set; }
		public double ESI { get; set; }
		public double TotalSalary { get; set; }
		public virtual Designation Designation { get; set; }
		public bool IsDeleted { get; set; }
		public int AcadmicYearId { get; set; }
	}
}
