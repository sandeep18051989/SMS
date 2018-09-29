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

			//relationship  
			this.HasRequired(cust => cust.User).WithMany().HasForeignKey(cust => cust.UserId);
			EntityTracker.TrackAllProperties<School>().Except(x => x.User).And(x => x.AcadmicYearId).And(x => x.ModifiedOn).And(x => x.CreatedOn);
		}
	}
}
