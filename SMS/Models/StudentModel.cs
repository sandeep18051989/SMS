using System;
using System.Collections.Generic;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(StudentModelValidator))]
	public partial class StudentModel : BaseEntityModel
	{
		public StudentModel()
		{
			Division_Class_Student = new Division_Class_StudentModel();
			Files = new List<FilesModel>();
			Religion = new ReligionModel();
			StudentPicture = new PictureModel();
			FatherPicture = new PictureModel();
			MotherPicture = new PictureModel();
		}
		public string FName { get; set; }
		public string IdentityNumber { get; set; }
		public string MName { get; set; }
		public string LName { get; set; }
		public string FatherFName { get; set; }
		public string FatherMName { get; set; }
		public string FatherLName { get; set; }
		public string MotherFName { get; set; }
		public string MotherMName { get; set; }
		public string MotherLName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public double Weight { get; set; }
		public double Height { get; set; }
		public string RelName { get; set; }
		public string Caste { get; set; }
		public string BusFacility { get; set; }
		public string MotherTounge { get; set; }
		public string BirthMark { get; set; }
		public string Disease { get; set; }
		public string Student_BusNo_RNo { get; set; }
		public string Bus_NoSchool { get; set; }
		public string AadharCardNo { get; set; }
		public string Pre_Institute_Name { get; set; }
		public string Pre_Institute_Address { get; set; }
		public string E_Phisician_Name { get; set; }
		public string E_Phisician_Address { get; set; }
		public string E_Phisician_Phone { get; set; }
		public string Father_Occupation { get; set; }
		public string Father_Education { get; set; }
		public string Father_BGroup { get; set; }
		public string EmailAddress { get; set; }
		public string Father_Office_Address { get; set; }
		public string Father_Contact { get; set; }
		public string Mother_Occupation { get; set; }
		public string Me_Mail { get; set; }
		public string Mother_Office_Address { get; set; }
		public string Mother_Contact { get; set; }
		public string Temporary_Address { get; set; }
		public string Permanent_Address { get; set; }
		public string TalukaPer { get; set; }
		public string DistrictPer { get; set; }
		public int PinPer { get; set; }
		public string TalukaTemp { get; set; }
		public string DistrictTemp { get; set; }
		public int PinTemp { get; set; }
		public string Username { get; set; }
		public string Sex { get; set; }
		public string SeoUrl { get; set; }
		public DateTime AdmissionDate { get; set; }
		public string FromYY { get; set; }
		public string ToYY { get; set; }
		public int Installments { get; set; }
		public int PictureId { get; set; }
		public int FatherPictureId { get; set; }
		public int MotherPictureId { get; set; }
		public int HouseId { get; set; }
		public string SystemName { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }
		public IList<FilesModel> Files { get; set; }

		public ReligionModel Religion { get; set; }
		public PictureModel StudentPicture { get; set; }
		public PictureModel FatherPicture { get; set; }
		public PictureModel MotherPicture { get; set; }
		public HouseModel House { get; set; }

		public Division_Class_StudentModel Division_Class_Student { get; set; }


	}
}
