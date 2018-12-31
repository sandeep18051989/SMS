using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class EmployeeMap : CMSEntityTypeConfiguration<Employee>
	{

		public EmployeeMap()
		{
			ToTable("Employee");
			HasKey(b => b.Id);
			Property(b => b.EmpFName).HasMaxLength(100).IsRequired();
			Property(b => b.FatherMName).HasMaxLength(100).IsOptional();
			Property(b => b.MotherMName).HasMaxLength(100).IsOptional();

			Property(b => b.EmpMName).HasMaxLength(100).IsOptional();
			Property(b => b.FatherMName).HasMaxLength(100).IsOptional();
			Property(b => b.MotherMName).HasMaxLength(100).IsOptional();

			Property(b => b.EmpLName).HasMaxLength(100).IsOptional();
			Property(b => b.FatherLName).HasMaxLength(100).IsOptional();
			Property(b => b.MotherLName).HasMaxLength(100).IsOptional();

			Property(b => b.BGroup).IsOptional();
			Property(b => b.DateOfBirth).IsRequired();
			Property(b => b.Weight).IsOptional();
			Property(b => b.Height).IsOptional();
			Property(b => b.QualificationId).IsOptional();

			Property(b => b.CasteId).IsOptional();
			Property(b => b.BusFacility).IsOptional();
			Property(b => b.BusNumber).IsOptional();
			Property(b => b.RouteNumber).IsOptional();
			Property(b => b.Pre_Institute_Name).IsOptional();
			Property(b => b.Pre_Institute_Address).IsOptional();

			Property(b => b.ContractStatusId).IsOptional();
			Property(b => b.ContractTypeId).IsOptional();
			Property(b => b.ContractStartDate).IsOptional();
			Property(b => b.ContractEndDate).IsOptional();
			Property(b => b.E_Phisician_Name).IsOptional();
			Property(b => b.E_Phisician_Address).IsOptional();
			Property(b => b.E_Phisician_Phone).IsOptional();
			Property(b => b.Father_Occupation).IsOptional();
			Property(b => b.Father_Office_Address).IsOptional();
			Property(b => b.Temporary_Address).IsOptional();
			Property(b => b.TalukaPer).IsOptional();
			Property(b => b.TalukaTemp).IsOptional();
			Property(b => b.DistrictTemp).IsOptional();
			Property(b => b.PinTemp).IsOptional();
			Property(b => b.FatherPictureId).IsOptional();
			Property(b => b.MotherPictureId).IsOptional();
			Property(b => b.EmployeePictureId).IsRequired();
			Property(b => b.ReligionId).IsOptional();
			Property(b => b.DD).IsOptional();
			Property(b => b.MM).IsOptional();
			Property(b => b.YYYY).IsOptional();
			Property(b => b.DesignationId).IsRequired();
			Property(b => b.Contact1).HasMaxLength(10).IsRequired();
			Property(b => b.AadharCardNo).HasMaxLength(16).IsRequired();
			Property(b => b.JoiningDate).IsRequired();
			Property(b => b.Username).HasMaxLength(20).IsRequired();

			HasRequired(all => all.Designation).WithMany().HasForeignKey(all => all.DesignationId);
			HasRequired(all => all.EmployeePicture).WithMany().HasForeignKey(all => all.EmployeePictureId);

			EntityTracker.TrackAllProperties<Employee>().Except(x => x.Religion).And(x => x.Designation).And(x => x.EmployeePicture).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
