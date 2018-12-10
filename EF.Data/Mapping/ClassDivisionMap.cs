using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class ClassDivisionMap : CMSEntityTypeConfiguration<ClassDivision>
	{
		public ClassDivisionMap()
		{
			this.ToTable("ClassDivision");
			this.HasKey(e => e.Id);
			this.Property(e => e.ClassId).IsRequired();
			this.Property(e => e.ClassRoomId).IsOptional();
            this.Property(e => e.DivisionId).IsRequired();
            // Relationships
            //this.HasOptional(e => e.ClassRoom).WithMany(p => p.Classes).HasForeignKey(e => e.ClassId);
            //this.HasOptional(e => e.ClassRoom).WithMany(p => p.Divisions).HasForeignKey(e => e.DivisionId);
            this.HasOptional(e => e.Class).WithMany(p => p.Divisions).HasForeignKey(e => e.DivisionId);
            this.HasOptional(e => e.Division).WithMany(p => p.Classes).HasForeignKey(e => e.ClassId);

            EntityTracker.TrackAllProperties<ClassDivision>().Except(x => x.ModifiedOn).And(x => x.CreatedOn).And(x => x.ClassRoom).And(x => x.Division).And(x => x.Class);
		}
	}
}
