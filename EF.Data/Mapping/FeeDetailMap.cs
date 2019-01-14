using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class FeeDetailMap : CMSEntityTypeConfiguration<FeeDetail>
	{
		public FeeDetailMap()
		{
			ToTable("Fee_Detail");
			HasKey(b => b.Id);
			Property(b => b.BankName).HasMaxLength(100).IsOptional();
			Property(b => b.CashierId).IsRequired();
			Property(b => b.AcadmicYearId).IsRequired();
			Property(b => b.CashierName).HasMaxLength(100).IsOptional();
			Property(b => b.Date).IsOptional();
			Property(b => b.DDChequeNumber).HasMaxLength(100).IsOptional();
			Property(b => b.FeeCategoryStructureId).IsOptional();
			Property(b => b.PaidBy).HasMaxLength(100).IsOptional();
			Property(b => b.PayingMode).HasMaxLength(100).IsOptional();
			Property(b => b.Remarks).HasMaxLength(500).IsOptional();
			Property(b => b.StudentId).IsRequired();
			Property(b => b.TotalFees).IsOptional();

			HasRequired(all => all.Student).WithMany().HasForeignKey(all => all.StudentId);
			HasRequired(all => all.AcadmicYear).WithMany().HasForeignKey(all => all.AcadmicYearId);
			HasRequired(all => all.Employee).WithMany().HasForeignKey(all => all.CashierId);

			EntityTracker.TrackAllProperties<FeeDetail>().Except(x => x.Student).And(x => x.AcadmicYear).And(x => x.Employee).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
