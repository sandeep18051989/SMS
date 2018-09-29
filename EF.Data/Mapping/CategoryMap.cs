using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class CategoryMap : CMSEntityTypeConfiguration<Category>
	{
		public CategoryMap()
		{
			this.ToTable("Category");
			this.HasKey(b => b.Id);

			// Relationships
			this.HasMany(pro => pro.Castes).WithMany(p => p.Categories).Map(m => m.ToTable("Category_Caste_Map").MapLeftKey("CategoryId").MapRightKey("CasteId"));
			EntityTracker.TrackAllProperties<Category>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
