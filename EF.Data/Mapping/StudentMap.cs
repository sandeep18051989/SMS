using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class StudentMap : CMSEntityTypeConfiguration<Student>
	{
		public StudentMap()
		{
			this.ToTable("Student");
			this.HasKey(b => b.Id);
			this.Property(b => b.MName).IsOptional();
			this.Property(b => b.FatherMName).IsOptional();
			this.Property(b => b.MotherMName).IsOptional();
			this.Property(b => b.BGroup).IsOptional();
			this.Property(b => b.DateOfBirth).IsOptional();
			this.Property(b => b.Weight).IsOptional();
			this.Property(b => b.Height).IsOptional();

			this.Property(b => b.ReligionId).IsOptional();
			this.Property(b => b.CasteId).IsOptional();
			this.Property(b => b.BusFacility).IsOptional();
			this.Property(b => b.MotherTounge).IsOptional();

			this.Property(b => b.Disease).IsOptional();
			this.Property(b => b.BusNumber).IsOptional();
			this.Property(b => b.RouteNumber).IsOptional();
			this.Property(b => b.Pre_Institute_Name).IsOptional();
			this.Property(b => b.Pre_Institute_Address).IsOptional();

			this.Property(b => b.E_Phisician_Name).IsOptional();
			this.Property(b => b.E_Phisician_Address).IsOptional();
			this.Property(b => b.E_Phisician_Phone).IsOptional();
			this.Property(b => b.Father_Occupation).IsOptional();
			this.Property(b => b.Father_Education).IsOptional();

			this.Property(b => b.Father_BGroup).IsOptional();
			this.Property(b => b.EmailAddress).IsOptional();
			this.Property(b => b.Father_Office_Address).IsOptional();
			this.Property(b => b.Father_Contact).IsOptional();
			this.Property(b => b.Mother_Occupation).IsOptional();

			this.Property(b => b.Mother_Office_Address).IsOptional();
			this.Property(b => b.Mother_Contact).IsOptional();
			this.Property(b => b.Temporary_Address).IsOptional();
			this.Property(b => b.TalukaPer).IsOptional();
			this.Property(b => b.TalukaTemp).IsOptional();

			this.Property(b => b.DistrictTemp).IsOptional();
			this.Property(b => b.PinTemp).IsOptional();

			this.Property(b => b.FatherPictureId).IsOptional();
			this.Property(b => b.MotherPictureId).IsOptional();
			this.Property(b => b.FatherNationality).IsOptional();
			this.Property(b => b.MotherNationality).IsOptional();
			this.Property(b => b.PersonalityStatusId).IsOptional();

			this.Property(b => b.ReferredBy).IsOptional();
			this.Property(b => b.Remarks).IsOptional();
			this.Property(b => b.CoverPictureId).IsOptional();
			this.Property(b => b.FacebookLink).IsOptional();
			this.Property(b => b.TweeterLink).IsOptional();

			this.Property(b => b.InstagramLink).IsOptional();
			this.Property(b => b.GooglePlusLink).IsOptional();
			this.Property(b => b.PInterestLink).IsOptional();
			this.Property(b => b.LinkedInLink).IsOptional();
			this.Property(b => b.Hi5Link).IsOptional();

			this.Property(b => b.IsPhoneVerified).IsOptional();
			this.Property(b => b.IsEmailVerified).IsOptional();
			this.Property(b => b.HouseId).IsOptional();
			this.Property(b => b.IsDeleted).IsOptional();
			this.Property(b => b.TweeterLink).IsOptional();

			this.HasRequired(all => all.Class).WithMany().HasForeignKey(all => all.ClassId);
			//this.HasOptional(all => all.Division).WithMany().HasForeignKey(all => all.DivisionId);
			this.HasOptional(all => all.House).WithMany().HasForeignKey(all => all.HouseId);

			// Relationships
			this.HasMany(pro => pro.Files).WithMany(p => p.Students).Map(m => m.ToTable("Student_File_Map").MapLeftKey("StudentId").MapRightKey("FileId"));
			EntityTracker.TrackAllProperties<Student>().Except(x => x.Class).And(x => x.House).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
