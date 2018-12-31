using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class TeacherMap : CMSEntityTypeConfiguration<Teacher>
	{
		public TeacherMap()
		{
			ToTable("Teacher");
			HasKey(b => b.Id);
			Property(b => b.FacebookLink).IsOptional();
			Property(b => b.TweeterLink).IsOptional();

			Property(b => b.InstagramLink).IsOptional();
			Property(b => b.GooglePlusLink).IsOptional();
			Property(b => b.PInterestLink).IsOptional();
			Property(b => b.LinkedInLink).IsOptional();
			Property(b => b.Hi5Link).IsOptional();
			Property(b => b.IsPhoneVerified).IsOptional();
			Property(b => b.IsEmailVerified).IsOptional();

			Property(b => b.PersonalityStatusId).IsOptional();
			Property(b => b.Description).IsOptional();
			Property(b => b.CoverPictureId).IsOptional();
			Property(b => b.Name).HasMaxLength(100).IsRequired();
			Property(b => b.Username).HasMaxLength(100).IsRequired();
			Property(b => b.ProfilePictureId).IsRequired();
			Property(b => b.EmployeeId).IsRequired();
			Property(b => b.AcadmicYearId).IsRequired();
			this.Property(b => b.ImpersonateId).IsRequired();

			HasRequired(all => all.Qualification).WithMany().HasForeignKey(all => all.QualificationId);

			// Relationships
			HasMany(u => u.ClassRoomDivisions)
				 .WithMany(z => z.Teachers)
				 .Map(m => m.ToTable("Teacher_Class_Room_Division_Mapping").MapLeftKey("TeacherId").MapRightKey("DivisionId"));

			HasMany(u => u.Subjects)
				 .WithMany(z => z.Teachers)
				 .Map(m => m.ToTable("Teacher_Subject_Mapping").MapLeftKey("TeacherId").MapRightKey("SubjectId"));

			HasMany(u => u.Files)
				 .WithMany(z => z.Teachers)
				 .Map(m => m.ToTable("Teacher_File_Mapping").MapLeftKey("TeacherId").MapRightKey("FileId"));

			EntityTracker.TrackAllProperties<Teacher>().Except(x => x.Subjects).And(x => x.ClassRoomDivisions).And(x => x.Qualification).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
