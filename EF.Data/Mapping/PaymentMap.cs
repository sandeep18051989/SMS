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
			//this.HasRequired(all => all.Employee).WithMany().HasForeignKey(all => all.EmployeeId);
			this.HasRequired(all => all.Allowance).WithMany().HasForeignKey(all => all.AllowanceId);

			EntityTracker.TrackAllProperties<Payment>().Except(x => x.Allowance).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
