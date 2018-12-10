using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class SchoolMap : CMSEntityTypeConfiguration<School>
	{
		public SchoolMap()
		{
			this.ToTable("School");
			this.HasKey(ui => ui.Id);
            this.Property(b => b.AcadmicYearId).IsRequired();
            this.Property(b => b.AffiliationNumber).IsOptional();
            this.Property(b => b.City).IsOptional();
            this.Property(b => b.State).IsOptional();
            this.Property(b => b.Country).IsOptional();
            this.Property(b => b.CoverPictureId).IsOptional();
            this.Property(b => b.FacebookLink).IsOptional();
            this.Property(b => b.FreelancerLink).IsOptional();
            this.Property(b => b.FullName).IsRequired();
            this.Property(b => b.GooglePlusLink).IsOptional();
            this.Property(b => b.GuruLink).IsOptional();
            this.Property(b => b.InstagramLink).IsOptional();
            this.Property(b => b.Landmark).IsOptional();
            this.Property(b => b.Latitude).IsOptional();
            this.Property(b => b.LinkedInLink).IsOptional();
            this.Property(b => b.Longitude).IsOptional();
            this.Property(b => b.PInterestLink).IsOptional();
            this.Property(b => b.ProfilePictureId).IsOptional();
            this.Property(b => b.RegistrationNumber).IsOptional();
            this.Property(b => b.Street1).IsOptional();
            this.Property(b => b.Street2).IsOptional();
            this.Property(b => b.TweeterLink).IsOptional();
            this.Property(b => b.UpworkLink).IsOptional();
            this.Property(b => b.UserName).IsOptional();
            this.Property(b => b.ZipCode).IsOptional();
            this.Property(b => b.Email).IsRequired();

            //relationship  
            this.HasRequired(cust => cust.User).WithMany().HasForeignKey(cust => cust.UserId);
			EntityTracker.TrackAllProperties<School>().Except(x => x.User).And(x => x.AcadmicYearId).And(x => x.ModifiedOn).And(x => x.CreatedOn);
		}
	}
}
