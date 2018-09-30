using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class EmployeeMap : CMSEntityTypeConfiguration<Employee>
	{

		public EmployeeMap()
		{
			this.ToTable("Employee");
			this.HasKey(b => b.Id);
			this.Property(b => b.EmpMName).IsOptional();
			this.Property(b => b.FatherMName).IsOptional();
			this.Property(b => b.MotherMName).IsOptional();
			this.Property(b => b.BGroup).IsOptional();
			this.Property(b => b.DateOfBirth).IsOptional();
			this.Property(b => b.Weight).IsOptional();
			this.Property(b => b.Height).IsOptional();

			this.Property(b => b.Caste).IsOptional();
			this.Property(b => b.Bus_NoSchool).IsOptional();
			this.Property(b => b.Pre_Institute_Name).IsOptional();
			this.Property(b => b.Pre_Institute_Address).IsOptional();

			this.Property(b => b.E_Phisician_Name).IsOptional();
			this.Property(b => b.E_Phisician_Address).IsOptional();
			this.Property(b => b.E_Phisician_Phone).IsOptional();
			this.Property(b => b.Father_Occupation).IsOptional();
			this.Property(b => b.Father_Office_Address).IsOptional();
			this.Property(b => b.Temporary_Address).IsOptional();
			this.Property(b => b.TalukaPer).IsOptional();
			this.Property(b => b.TalukaTemp).IsOptional();
			this.Property(b => b.DistrictTemp).IsOptional();
			this.Property(b => b.PinTemp).IsOptional();
			this.Property(b => b.FatherPictureId).IsOptional();
			this.Property(b => b.MotherPictureId).IsOptional();
			this.Property(b => b.EmployeePictureId).IsOptional();
			this.Property(b => b.ReliogionId).IsOptional();

			this.HasRequired(all => all.Allowance).WithMany().HasForeignKey(all => all.AllowanceId);
			this.HasOptional(all => all.Religion).WithMany().HasForeignKey(all => all.ReliogionId);
			this.HasOptional(all => all.EmployeePicture).WithMany().HasForeignKey(all => all.EmployeePictureId);

			// Relationships
			this.HasMany(u => u.Classes)
				 .WithMany()
				 .Map(m => m.ToTable("Employee_Classes_Map").MapLeftKey("EmpId").MapRightKey("ClassId"));

			EntityTracker.TrackAllProperties<Employee>().Except(x => x.Religion).And(x => x.Allowance).And(x => x.Classes).And(x => x.EmployeePicture).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
