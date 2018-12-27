using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class FeeCategoryMap : CMSEntityTypeConfiguration<FeeCategory>
	{
		public FeeCategoryMap()
		{
			this.ToTable("FeeCategory");
			this.HasKey(b => b.Id);
            this.Property(b => b.AcadmicYearId).IsRequired();
            this.Property(b => b.CategoryId).IsRequired();
            this.Property(b => b.CategoryName).HasMaxLength(100).IsOptional();
            this.Property(b => b.ClassDivisionId).IsOptional();
            this.Property(b => b.FeeAmount).IsRequired();
            this.Property(b => b.PeriodFrom).IsOptional();
            this.Property(b => b.PeriodTo).IsOptional();

            this.HasRequired(all => all.Category).WithMany().HasForeignKey(all => all.CategoryId);
			this.HasOptional(all => all.ClassDivision).WithMany().HasForeignKey(all => all.ClassDivisionId);

			EntityTracker.TrackAllProperties<FeeCategory>().Except(x => x.Category).And(x => x.ClassDivision).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
