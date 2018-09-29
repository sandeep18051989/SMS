using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class BookIssueMap : CMSEntityTypeConfiguration<BookIssue>
	{
		public BookIssueMap()
		{
			this.ToTable("BookIssue");
			this.HasKey(b => b.Id);
			this.HasRequired(all => all.Book).WithMany().HasForeignKey(all => all.BookId);
			this.HasRequired(all => all.Employee).WithMany().HasForeignKey(all => all.LibrarianId);
			this.HasRequired(all => all.Student).WithMany().HasForeignKey(all => all.StudentId);

			EntityTracker.TrackAllProperties<BookIssue>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.Book).And(x => x.Employee).And(x => x.Student);

		}
	}
}
