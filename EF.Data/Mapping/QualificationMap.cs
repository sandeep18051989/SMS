using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class QualificationMap : CMSEntityTypeConfiguration<Qualification>
	{
		public QualificationMap()
		{
			this.ToTable("Qualification");
			this.HasKey(b => b.Id);
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.Description).IsOptional();

            EntityTracker.TrackAllProperties<Qualification>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
