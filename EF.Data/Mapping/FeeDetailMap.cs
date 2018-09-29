using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class FeeDetailMap : CMSEntityTypeConfiguration<FeeDetail>
	{
		public FeeDetailMap()
		{
			this.ToTable("Fee_Detail");
			this.HasKey(b => b.Id);
			this.HasRequired(all => all.FeeCategoryStructure).WithMany().HasForeignKey(all => all.FeeCategoryStructureId);
			this.HasRequired(all => all.Student).WithMany().HasForeignKey(all => all.StudentId);
			this.HasRequired(all => all.Employee).WithMany().HasForeignKey(all => all.CashierId);

			EntityTracker.TrackAllProperties<FeeDetail>().Except(x => x.Student).And(x => x.FeeCategoryStructure).And(x => x.Employee).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
