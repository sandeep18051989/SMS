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
            this.Property(b => b.IName).IsRequired();
            this.Property(b => b.IPurchaseDate).IsOptional();
            this.Property(b => b.IQuantity).IsRequired();
            this.Property(b => b.IRate).IsRequired();
            this.Property(b => b.ITax).IsOptional();
            this.Property(b => b.ITotal).IsOptional();
            this.Property(b => b.ProductId).IsOptional();
            this.Property(b => b.VendorId).IsOptional();

            this.HasOptional(all => all.Product).WithMany().HasForeignKey(all => all.ProductId);

			EntityTracker.TrackAllProperties<Purchase>().Except(x => x.Product).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
