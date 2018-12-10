using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class DivisionMap : CMSEntityTypeConfiguration<Division>
	{
		public DivisionMap()
		{
			this.ToTable("Division");
			this.HasKey(b => b.Id);
            this.Property(b => b.Description).IsOptional();
            this.Property(b => b.Name).IsRequired();

            EntityTracker.TrackAllProperties<Division>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
