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
            this.Property(b => b.SignaturePictureId).IsRequired();
            this.Property(b => b.LogoPictureId).IsRequired();
            this.Property(b => b.MaxMarks).IsOptional();
            this.Property(b => b.TotalQuestions).IsOptional();
            this.Property(b => b.DurationInMinutes).IsOptional();
            this.Property(b => b.Name).HasMaxLength(200).IsRequired();
            this.Property(b => b.PassingMarks).IsOptional();
            this.Property(b => b.StartTime).IsOptional();
            this.Property(b => b.EndTime).IsOptional();
            this.Property(b => b.SubjectId).IsOptional();

            EntityTracker.TrackAllProperties<Assessment>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);
		}
	}
}
