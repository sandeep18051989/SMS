using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class VendorMap : CMSEntityTypeConfiguration<Vendor>
	{
		public VendorMap()
		{
			this.ToTable("Vendor");
			this.HasKey(b => b.Id);
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.Address).IsOptional();
            this.Property(b => b.Date).IsOptional();
            this.Property(b => b.MobileContact).IsOptional();
            this.Property(b => b.OfficeContact).IsOptional();
            this.Property(b => b.RegNumber).IsOptional();

            this.HasRequired(all => all.AcadmicYear).WithMany().HasForeignKey(all => all.AcadmicYearId);
			EntityTracker.TrackAllProperties<Vendor>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
