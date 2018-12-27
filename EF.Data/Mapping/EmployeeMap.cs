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
            this.Property(b => b.EmpFName).HasMaxLength(100).IsRequired();
            this.Property(b => b.FatherMName).HasMaxLength(100).IsOptional();
            this.Property(b => b.MotherMName).HasMaxLength(100).IsOptional();

            this.Property(b => b.EmpMName).HasMaxLength(100).IsOptional();
			this.Property(b => b.FatherMName).HasMaxLength(100).IsOptional();
			this.Property(b => b.MotherMName).HasMaxLength(100).IsOptional();

            this.Property(b => b.EmpLName).HasMaxLength(100).IsOptional();
            this.Property(b => b.FatherLName).HasMaxLength(100).IsOptional();
            this.Property(b => b.MotherLName).HasMaxLength(100).IsOptional();

            this.Property(b => b.BGroup).IsOptional();
			this.Property(b => b.DateOfBirth).IsRequired();
			this.Property(b => b.Weight).IsOptional();
			this.Property(b => b.Height).IsOptional();
            this.Property(b => b.QualificationId).IsOptional();

            this.Property(b => b.CasteId).IsOptional();
			this.Property(b => b.BusFacility).IsOptional();
            this.Property(b => b.BusNumber).IsOptional();
            this.Property(b => b.RouteNumber).IsOptional();
            this.Property(b => b.Pre_Institute_Name).IsOptional();
			this.Property(b => b.Pre_Institute_Address).IsOptional();

            this.Property(b => b.ContractStatusId).IsOptional();
            this.Property(b => b.ContractTypeId).IsOptional();
            this.Property(b => b.ContractStartDate).IsOptional();
            this.Property(b => b.ContractEndDate).IsOptional();
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
			this.Property(b => b.EmployeePictureId).IsRequired();
			this.Property(b => b.ReligionId).IsOptional();
            this.Property(b => b.DD).IsOptional();
            this.Property(b => b.MM).IsOptional();
            this.Property(b => b.YYYY).IsOptional();
            this.Property(b => b.DesignationId).IsRequired();
            this.Property(b => b.Contact1).HasMaxLength(10).IsRequired();
            this.Property(b => b.AadharCardNo).HasMaxLength(16).IsRequired();
            this.Property(b => b.JoiningDate).IsRequired();
            this.Property(b => b.Username).HasMaxLength(20).IsRequired();

            this.HasRequired(all => all.Designation).WithMany().HasForeignKey(all => all.DesignationId);
            this.HasOptional(all => all.EmployeePicture).WithMany().HasForeignKey(all => all.EmployeePictureId);

			EntityTracker.TrackAllProperties<Employee>().Except(x => x.Religion).And(x => x.Designation).And(x => x.EmployeePicture).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
