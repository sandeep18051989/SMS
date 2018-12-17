using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class AssessmentMap : CMSEntityTypeConfiguration<Assessment>
	{
		public AssessmentMap()
		{
			this.ToTable("Assessment");
			this.HasKey(b => b.Id);
            this.Property(b => b.AcadmicYearId).IsRequired();
            this.Property(b => b.MaxMarks).IsRequired();
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.PassingMarks).IsRequired();
            this.Property(b => b.StartTime).IsOptional();
            this.Property(b => b.EndTime).IsOptional();
            EntityTracker.TrackAllProperties<Assessment>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);
		}
	}
}
