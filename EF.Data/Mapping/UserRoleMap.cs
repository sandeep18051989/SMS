using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class UserRoleMap : CMSEntityTypeConfiguration<UserRole>
	{
		public UserRoleMap()
		{
			this.ToTable("UserRole");
			this.HasKey(ro => ro.Id);
			// Relationships
			this.HasMany(ro => ro.PermissionRecords).WithMany(per => per.PermissionRoles).Map(m => m.ToTable("UserRole_Permission_Map").MapLeftKey("UserRoleId").MapRightKey("PermissionRecordId"));

			EntityTracker.TrackAllProperties<UserRole>().Except(x => x.ModifiedOn).And(x => x.CreatedOn);

		}
	}
}
