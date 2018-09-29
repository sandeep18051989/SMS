using EF.Core.Data;
using EF.Data.Configuration;
using System.Data.Entity.ModelConfiguration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class UserMap : CMSEntityTypeConfiguration<User>
	{
		public UserMap()
		{
			this.ToTable("Users");
			this.HasKey(u => u.Id);
			this.Property(u => u.UserName).HasMaxLength(400);
			this.Property(u => u.Password).HasMaxLength(30);
			// Relationships
			this.HasMany(u => u.Roles)
				 .WithMany()
				 .Map(m => m.ToTable("User_Roles_Map").MapLeftKey("UserId").MapRightKey("RoleId"));

			EntityTracker.TrackAllProperties<User>().Except(x => x.Roles).And(x => x.ModifiedOn).And(x => x.CreatedOn);

		}
	}
}
