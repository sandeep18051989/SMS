using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class DivisionHomeworkMap : CMSEntityTypeConfiguration<DivisionHomework>
	{

		public DivisionHomeworkMap()
		{
			this.ToTable("Division_Homework_Mapping");
			this.HasKey(b => b.Id);
            this.Property(b => b.DivisionId).IsRequired();
            this.Property(b => b.HomeworkId).IsRequired();
            this.Property(b => b.EndDate).IsOptional();
            this.Property(b => b.StartDate).IsOptional();

            // Relationships
            this.HasMany(e => e.Comments).WithMany(c => c.DivisionHomeworks).Map(m => m.ToTable("Division_Homework_Comment_Map").MapLeftKey("DivisionHomeworkId").MapRightKey("CommentId"));

            this.HasRequired(ca => ca.Division).WithMany(e => e.DivisionHomeworks).HasForeignKey(ca => ca.DivisionId);
			this.HasRequired(ca => ca.Homework).WithMany(e => e.DivisionHomeworks).HasForeignKey(ca => ca.HomeworkId);

			EntityTracker.TrackAllProperties<DivisionHomework>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.Division).And(x => x.Homework);

		}
	}
}
