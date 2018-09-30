using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class TeacherMap : CMSEntityTypeConfiguration<Teacher>
	{
		public TeacherMap()
		{
			this.ToTable("Teacher");
			this.HasKey(b => b.Id);
			this.Property(b => b.FacebookLink).IsOptional();
			this.Property(b => b.TweeterLink).IsOptional();

			this.Property(b => b.InstagramLink).IsOptional();
			this.Property(b => b.GooglePlusLink).IsOptional();
			this.Property(b => b.PInterestLink).IsOptional();
			this.Property(b => b.LinkedInLink).IsOptional();
			this.Property(b => b.Hi5Link).IsOptional();
			this.Property(b => b.IsPhoneVerified).IsOptional();
			this.Property(b => b.IsEmailVerified).IsOptional();

			this.Property(b => b.PersonalityStatusId).IsOptional();
			this.Property(b => b.Description).IsOptional();
			this.Property(b => b.CoverPictureId).IsOptional();

			this.HasRequired(all => all.Qualification).WithMany().HasForeignKey(all => all.QualificationId);

			// Relationships
			this.HasMany(u => u.Divisions)
				 .WithMany()
				 .Map(m => m.ToTable("Teacher_Division_Mapping").MapLeftKey("TeacherId").MapRightKey("DivisionId"));
			this.HasMany(u => u.Subjects)
				 .WithMany()
				 .Map(m => m.ToTable("Teacher_Subject_Mapping").MapLeftKey("TeacherId").MapRightKey("SubjectId"));

			this.HasMany(u => u.Files)
				 .WithMany()
				 .Map(m => m.ToTable("Teacher_File_Mapping").MapLeftKey("TeacherId").MapRightKey("FileId"));
			EntityTracker.TrackAllProperties<Teacher>().Except(x => x.Subjects).And(x => x.Divisions).And(x => x.Qualification).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
