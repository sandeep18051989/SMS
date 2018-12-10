using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class PaymentMap : CMSEntityTypeConfiguration<Payment>
	{
		public PaymentMap()
		{
			this.ToTable("Payment");
			this.HasKey(b => b.Id);
            this.Property(b => b.AllowanceId).IsOptional();
            this.Property(b => b.BasicPay).IsOptional();
            this.Property(b => b.EmployeeId).IsOptional();
            this.Property(b => b.Gross_Pay).IsOptional();
            this.Property(b => b.Net_Pay).IsOptional();
            this.Property(b => b.PDate).IsOptional();
            this.Property(b => b.PF).IsOptional();
            this.Property(b => b.TA).IsOptional();
            this.Property(b => b.TDS).IsOptional();
            this.Property(b => b.DA).IsOptional();

            this.HasRequired(all => all.Allowance).WithMany().HasForeignKey(all => all.AllowanceId);

			EntityTracker.TrackAllProperties<Payment>().Except(x => x.Allowance).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
