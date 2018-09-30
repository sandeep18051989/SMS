using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class StudentClassDivisionMap : CMSEntityTypeConfiguration<StudentClassDivision>
	{
		public StudentClassDivisionMap()
		{
			this.ToTable("Student_Class_Division_Mapping");
			this.HasKey(b => b.Id);
			//this.HasRequired(all => all.Student).WithMany().HasForeignKey(all => all.StudentId);
			this.HasRequired(all => all.Class).WithMany().HasForeignKey(all => all.ClassId);
			//this.HasRequired(all => all.Division).WithMany().HasForeignKey(all => all.DivisionId);

			EntityTracker.TrackAllProperties<StudentClassDivision>().Except(x => x.Class).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
