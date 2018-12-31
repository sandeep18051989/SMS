using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class StudentMap : CMSEntityTypeConfiguration<Student>
	{
		public StudentMap()
		{
			ToTable("Student");
			HasKey(b => b.Id);
			Property(b => b.MName).IsOptional();
			Property(b => b.FatherMName).IsOptional();
			Property(b => b.MotherMName).IsOptional();
			Property(b => b.BGroup).IsOptional();
			Property(b => b.DateOfBirth).IsOptional();
			Property(b => b.Weight).IsOptional();
			Property(b => b.Height).IsOptional();

			Property(b => b.ReligionId).IsOptional();
			Property(b => b.CasteId).IsOptional();
			Property(b => b.BusFacility).IsOptional();
			Property(b => b.MotherTounge).IsOptional();

			Property(b => b.Disease).IsOptional();
			Property(b => b.BusNumber).IsOptional();
			Property(b => b.RouteNumber).IsOptional();
			Property(b => b.Pre_Institute_Name).IsOptional();
			Property(b => b.Pre_Institute_Address).IsOptional();

			Property(b => b.E_Phisician_Name).IsOptional();
			Property(b => b.E_Phisician_Address).IsOptional();
			Property(b => b.E_Phisician_Phone).IsOptional();
			Property(b => b.Father_Occupation).IsOptional();
			Property(b => b.Father_Education).IsOptional();

			Property(b => b.Father_BGroup).IsOptional();
			Property(b => b.EmailAddress).IsOptional();
			Property(b => b.Father_Office_Address).IsOptional();
			Property(b => b.Father_Contact).IsOptional();
			Property(b => b.Mother_Occupation).IsOptional();

			Property(b => b.Mother_Office_Address).IsOptional();
			Property(b => b.Mother_Contact).IsOptional();
			Property(b => b.Temporary_Address).IsOptional();
			Property(b => b.TalukaPer).IsOptional();
			Property(b => b.TalukaTemp).IsOptional();

			Property(b => b.DistrictTemp).IsOptional();
			Property(b => b.PinTemp).IsOptional();

			Property(b => b.FatherPictureId).IsOptional();
			Property(b => b.MotherPictureId).IsOptional();
			Property(b => b.FatherNationality).IsOptional();
			Property(b => b.MotherNationality).IsOptional();
			Property(b => b.PersonalityStatusId).IsOptional();

			Property(b => b.ReferredBy).IsOptional();
			Property(b => b.Remarks).IsOptional();
			Property(b => b.CoverPictureId).IsOptional();
			Property(b => b.FacebookLink).IsOptional();
			Property(b => b.TweeterLink).IsOptional();

			Property(b => b.InstagramLink).IsOptional();
			Property(b => b.GooglePlusLink).IsOptional();
			Property(b => b.PInterestLink).IsOptional();
			Property(b => b.LinkedInLink).IsOptional();
			Property(b => b.Hi5Link).IsOptional();

			Property(b => b.IsPhoneVerified).IsOptional();
			Property(b => b.IsEmailVerified).IsOptional();
			Property(b => b.HouseId).IsOptional();
			Property(b => b.IsDeleted).IsOptional();
			Property(b => b.TweeterLink).IsOptional();
			Property(b => b.ClassRoomDivisionId).IsRequired();
			this.Property(b => b.ImpersonateId).IsRequired();

			HasRequired(all => all.ClassRoomDivision).WithMany().HasForeignKey(all => all.ClassRoomDivisionId);
			HasOptional(all => all.House).WithMany().HasForeignKey(all => all.HouseId);

			// Relationships
			HasMany(pro => pro.Files).WithMany(p => p.Students).Map(m => m.ToTable("Student_File_Map").MapLeftKey("StudentId").MapRightKey("FileId"));
			EntityTracker.TrackAllProperties<Student>().Except(x => x.ClassRoomDivision).And(x => x.House).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
