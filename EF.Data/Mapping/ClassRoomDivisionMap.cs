using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class ClassRoomDivisionMap : CMSEntityTypeConfiguration<ClassRoomDivision>
	{
		public ClassRoomDivisionMap()
		{
			this.ToTable("Class_Division_ClassRoom_Mapping");
			this.HasKey(e => e.Id);

            // Relationships
            this.HasOptional(all => all.ClassRoom).WithMany().HasForeignKey(all => all.ClassRoomId);
            this.HasOptional(all => all.Class).WithMany().HasForeignKey(all => all.ClassId);
            this.HasOptional(all => all.Division).WithMany().HasForeignKey(all => all.DivisionId);
            //this.HasMany(ro => ro.MessageGroups).WithMany(per => per.ClassRoomDivisions).Map(m => m.ToTable("Class_Room_Division_MessageGroup_Map").MapLeftKey("ClassRoomDivisionMapId").MapRightKey("MessageGroupId"));

            EntityTracker.TrackAllProperties<ClassRoomDivision>().Except(x => x.ModifiedOn).And(x => x.CreatedOn).And(x => x.ClassRoom).And(x => x.Class).And(x => x.Division).And(x => x.MessageGroups);
		}
	}
}
