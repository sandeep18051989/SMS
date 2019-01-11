using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class Employee : BaseEntity
	{
		public string EmpFName { get; set; }
		public string EmpMName { get; set; }
		public string EmpLName { get; set; }
		public string FatherFName { get; set; }
		public string FatherMName { get; set; }
		public string FatherLName { get; set; }
		public string MotherFName { get; set; }
		public string MotherMName { get; set; }
		public string MotherLName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public double Weight { get; set; }
		public double Height { get; set; }
		public int? ReligionId { get; set; }
		public string MarriedStatus { get; set; }
		public DateTime? JoiningDate { get; set; }
		public int DesignationId { get; set; }
		public int? QualificationId { get; set; }
		public string Sex { get; set; }
		public int? CasteId { get; set; }
		public int ContractTypeId { get; set; }
		public string BGroup { get; set; }
		public string BirthMark { get; set; }
		public string BusNumber { get; set; }
		public string RouteNumber { get; set; }
		public string AadharCardNo { get; set; }
		public string Pre_Institute_Name { get; set; }
		public string Pre_Institute_Address { get; set; }
		public bool? BusFacility { get; set; }
		public string E_Phisician_Name { get; set; }
		public string E_Phisician_Address { get; set; }
		public string E_Phisician_Phone { get; set; }
		public string Father_Occupation { get; set; }
		public string Email { get; set; }
		public string Father_Office_Address { get; set; }
		public string Contact1 { get; set; }
		public string Contact2 { get; set; }
		public string Occupation_Spouse { get; set; }
		public string Edu_Spouse { get; set; }
		public string Temporary_Address { get; set; }
		public string Permanent_Address { get; set; }
		public string TalukaPer { get; set; }
		public string DistrictPer { get; set; }
		public int PinPer { get; set; }
		public string TalukaTemp { get; set; }
		public string DistrictTemp { get; set; }
		public int PinTemp { get; set; }
		public string Username { get; set; }
		public string Emergency_Email { get; set; }
		public string Emergency_Contact { get; set; }
		public string DD { get; set; }
		public string MM { get; set; }
		public string YYYY { get; set; }
		public int EmployeePictureId { get; set; }
		public int FatherPictureId { get; set; }
		public int MotherPictureId { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public int ContractStatusId { get; set; }
		public DateTime? ContractStartDate { get; set; }
		public DateTime? ContractEndDate { get; set; }
		public virtual Religion Religion { get; set; }
		public virtual Designation Designation { get; set; }
		public virtual Picture EmployeePicture { get; set; }

		[NotMapped]
		public ContractStatus ContractStatus
		{
			get
			{
				return (ContractStatus)ContractStatusId;
			}
			set
			{
				ContractStatusId = (int)value;
			}
		}
		[NotMapped]
		public ContractType ContractType
		{
			get
			{
				return (ContractType)ContractTypeId;
			}
			set
			{
				ContractTypeId = (int)value;
			}
		}

		#region Navigation Properties
		#endregion
	}
}
