using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class LicenseMap : CMSEntityTypeConfiguration<License>
	{
		public LicenseMap()
		{
			this.ToTable("License");
			this.HasKey(b => b.Id);
			this.Property(b => b.Company).IsOptional();
			this.Property(b => b.LicenseStartDate).IsOptional();
            this.Property(b => b.LicenseEndDate).IsOptional();
            this.Property(b => b.LicenseKey).IsRequired();
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.LicenseUrl).IsRequired();

            EntityTracker.TrackAllProperties<License>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
