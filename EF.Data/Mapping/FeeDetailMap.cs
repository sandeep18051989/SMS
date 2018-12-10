using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class FeeDetailMap : CMSEntityTypeConfiguration<FeeDetail>
	{
		public FeeDetailMap()
		{
			this.ToTable("Fee_Detail");
			this.HasKey(b => b.Id);
            this.Property(b => b.BankName).IsOptional();
            this.Property(b => b.CashierId).IsRequired();
            this.Property(b => b.CashierName).IsOptional();
            this.Property(b => b.Date).IsRequired();
            this.Property(b => b.DD).IsOptional();
            this.Property(b => b.MM).IsOptional();
            this.Property(b => b.YYYY).IsOptional();
            this.Property(b => b.DDChequeNumber).IsOptional();
            this.Property(b => b.FeeCategoryStructureId).IsOptional();
            this.Property(b => b.PaidBy).IsOptional();
            this.Property(b => b.PayingMode).IsOptional();
            this.Property(b => b.Remarks).IsOptional();
            this.Property(b => b.StudentId).IsRequired();
            this.Property(b => b.TotalFees).IsOptional();

            this.HasRequired(all => all.FeeCategoryStructure).WithMany().HasForeignKey(all => all.FeeCategoryStructureId);
			this.HasRequired(all => all.Student).WithMany().HasForeignKey(all => all.StudentId);
			this.HasRequired(all => all.Employee).WithMany().HasForeignKey(all => all.CashierId);

			EntityTracker.TrackAllProperties<FeeDetail>().Except(x => x.Student).And(x => x.FeeCategoryStructure).And(x => x.Employee).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
