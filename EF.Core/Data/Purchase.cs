using System;

namespace EF.Core.Data
{
	public partial class Purchase : BaseEntity
	{
		public int ProductId { get; set; }
		public int VendorId { get; set; }
        public int AcadmicYearId { get; set; }
        public string IName { get; set; }
		public double IQuantity { get; set; }
		public double IRate { get; set; }
		public DateTime? IPurchaseDate { get; set; }
		public double ITax { get; set; }
		public double ITotal { get; set; }
		public virtual Product Product { get; set; }
        public virtual AcadmicYear AcadmicYear { get; set; }
    }
}
