using System;
using System.Collections.Generic;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(EmployeeModelValidator))]
	public partial class EmployeeModel : BaseEntityModel
	{
		public EmployeeModel()
		{
			Classes = new List<ClassModel>();
			Religion = new ReligionModel();
			Designation = new DesignationModel();
			Qualification = new QualificationModel();
			Allowance = new AllowanceModel();
			EmployeePicture = new PictureModel();
			FatherPicture = new PictureModel();
			MotherPicture = new PictureModel();
		}
		public string EmpFName { get; set; }
		public string EmpMName { get; set; }
		public string EmpLName { get; set; }
		public string FatherFName { get; set; }
		public string FatherMName { get; set; }
		public string FatherLName { get; set; }
		public string MotherFName { get; set; }
		public string MotherMName { get; set; }
		public string MotherLName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public double Weight { get; set; }
		public double Height { get; set; }
		public int ReliogionId { get; set; }
		public string MarriedStatus { get; set; }
		public DateTime JoiningDate { get; set; }
		public int DesignationId { get; set; }
		public int QualificationId { get; set; }
		public string Sex { get; set; }
		public string ReligionName { get; set; }
		public string Caste { get; set; }
		public string BGroup { get; set; }
		public string BirthMark { get; set; }
		public string Teacher_BusNo_RNo { get; set; }
		public string Bus_NoSchool { get; set; }
		public string AadharCardNo { get; set; }
		public string Pre_Institute_Name { get; set; }
		public string Pre_Institute_Address { get; set; }
		public string Bus_Facility { get; set; }
		public string E_Phisician_Name { get; set; }
		public string E_Phisician_Address { get; set; }
		public int E_Phisician_Phone { get; set; }
		public int AllowanceId { get; set; }
		public double BasicPay { get; set; }
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
		public DateTime DDate { get; set; }
		public string Emergency_Email { get; set; }
		public string Emergency_Contact { get; set; }
		public string DD { get; set; }
		public string MM { get; set; }
		public string YY { get; set; }
		public int EmployeePictureId { get; set; }
		public int FatherPictureId { get; set; }
		public int MotherPictureId { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }

		public IList<ClassModel> Classes { get; set; }
		public ReligionModel Religion { get; set; }
		public DesignationModel Designation { get; set; }
		public QualificationModel Qualification { get; set; }
		public AllowanceModel Allowance { get; set; }
		public PictureModel EmployeePicture { get; set; }
		public PictureModel FatherPicture { get; set; }
		public PictureModel MotherPicture { get; set; }

	}
}
