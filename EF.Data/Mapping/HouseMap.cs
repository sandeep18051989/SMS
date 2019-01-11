using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class HouseMap : CMSEntityTypeConfiguration<House>
	{
		public HouseMap()
		{
			this.ToTable("House");
			this.HasKey(b => b.Id);
            this.Property(e => e.AcadmicYearId).IsRequired();
            this.Property(e => e.Description).IsOptional();
            this.Property(e => e.Name).IsRequired();

            EntityTracker.TrackAllProperties<House>().Except(x => x.Picture).And(x => x.Students).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
