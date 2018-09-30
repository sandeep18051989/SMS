using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class PurchaseMap : CMSEntityTypeConfiguration<Purchase>
	{
		public PurchaseMap()
		{
			this.ToTable("Purchase");
			this.HasKey(b => b.Id);
			this.HasRequired(all => all.Product).WithMany().HasForeignKey(all => all.ProductId);
			//this.HasRequired(all => all.Vendor).WithMany().HasForeignKey(all => all.VendorId);

			EntityTracker.TrackAllProperties<Purchase>().Except(x => x.Product).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
