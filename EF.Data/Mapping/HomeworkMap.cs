using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class HomeworkMap : CMSEntityTypeConfiguration<Homework>
	{

		public HomeworkMap()
		{
			this.ToTable("Homework");
			this.HasKey(b => b.Id);
            this.Property(b => b.AcadmicYearId).IsRequired();
            this.Property(b => b.Description).IsOptional();
            this.Property(b => b.Name).IsRequired();

			EntityTracker.TrackAllProperties<Homework>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
