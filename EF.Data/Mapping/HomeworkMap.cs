using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class HomeworkMap : CMSEntityTypeConfiguration<Homework>
	{

		public HomeworkMap()
		{
			this.ToTable("Homework");
			this.HasKey(b => b.Id);
            this.Property(b => b.AcadmicYearId).IsRequired();
            this.Property(b => b.Description).IsOptional();
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.StudentApprovalStatusId).IsOptional();
            this.Property(b => b.TeacherApprovalStatusId).IsOptional();

            this.HasMany(pro => pro.Comments).WithMany(p => p.Homeworks).Map(m => m.ToTable("Homework_Comment_Map").MapLeftKey("HomeworkId").MapRightKey("CommentId"));

			EntityTracker.TrackAllProperties<Homework>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
