using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class AllowanceMap : CMSEntityTypeConfiguration<Allowance>
	{
		public AllowanceMap()
		{
			this.ToTable("Allowance");
			this.HasKey(b => b.Id);
			this.Property(b => b.ESI).IsOptional();
			this.Property(b => b.DA).IsOptional();
			this.Property(b => b.HRA).IsOptional();
			this.Property(b => b.PF).IsOptional();
			this.Property(b => b.TA).IsOptional();
			this.Property(b => b.TDS).IsOptional();

			this.HasRequired(all => all.Designation).WithMany().HasForeignKey(all => all.DesignationId);
			EntityTracker.TrackAllProperties<Allowance>().Except(x => x.Designation).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
