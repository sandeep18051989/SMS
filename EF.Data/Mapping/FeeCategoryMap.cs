using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class FeeCategoryMap : CMSEntityTypeConfiguration<FeeCategory>
	{
		public FeeCategoryMap()
		{
			ToTable("FeeCategory");
			HasKey(b => b.Id);
			Property(b => b.AcadmicYearId).IsRequired();
			Property(b => b.CategoryId).IsRequired();
			Property(b => b.CategoryName).HasMaxLength(100).IsOptional();
			Property(b => b.ClassDivisionId).IsOptional();
			Property(b => b.FeeAmount).IsRequired();
			Property(b => b.PeriodFrom).IsOptional();
			Property(b => b.PeriodTo).IsOptional();

			HasRequired(all => all.Category).WithMany().HasForeignKey(all => all.CategoryId);
			HasOptional(all => all.ClassDivision).WithMany().HasForeignKey(all => all.ClassDivisionId);

			EntityTracker.TrackAllProperties<FeeCategory>().Except(x => x.Category).And(x => x.ClassDivision).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
