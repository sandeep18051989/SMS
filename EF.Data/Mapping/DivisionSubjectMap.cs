using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class DivisionSubjectMap : CMSEntityTypeConfiguration<DivisionSubject>
	{
		public DivisionSubjectMap()
		{
			ToTable("Division_Subject_Mapping");
			HasKey(b => b.Id);
			Property(b => b.DivisionId).IsRequired();
			Property(b => b.SubjectId).IsRequired();

			HasRequired(all => all.Subject).WithMany(e => e.ClassDivisionSubjects).HasForeignKey(all => all.SubjectId);
			HasRequired(all => all.Division).WithMany(e => e.Subjects).HasForeignKey(all => all.DivisionId);

			EntityTracker.TrackAllProperties<DivisionSubject>().Except(x => x.Subject).And(x => x.Division).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
