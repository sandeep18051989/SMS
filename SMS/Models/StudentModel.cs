using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
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
			Files = new List<FilesModel>();
			StudentPicture = new PictureModel();
			FatherPicture = new PictureModel();
			MotherPicture = new PictureModel();
			AvailableCastes = new List<SelectListItem>();
			AvailableReligions = new List<SelectListItem>();
			AvailableHouses = new List<SelectListItem>();
		}
		public Guid StudentUniqueId { get; set; }
		public string FName { get; set; }
		public string IdentityNumber { get; set; }
		public string MName { get; set; }
		public string PictureSrc { get; set; }
		public string LName { get; set; }
		public string FatherFName { get; set; }
		public string FatherMName { get; set; }
		public string FatherLName { get; set; }
		public string MotherFName { get; set; }
		public string MotherMName { get; set; }
		public string MotherLName { get; set; }
		[UIHint("Date")]
		public DateTime? DateOfBirth { get; set; }
		public double? Weight { get; set; }
		public double? Height { get; set; }
		public string RelName { get; set; }
		public int? CasteId { get; set; }
		public int? ReligionId { get; set; }
		[UIHint("File")]
		public int? FileId { get; set; }
		public int? HouseId { get; set; }
		public bool BusFacility { get; set; }
		public bool IsActive { get; set; }
		public string MotherTounge { get; set; }
		public string BirthMark { get; set; }
		public string Disease { get; set; }
		public string BusNumber { get; set; }
		public string RouteNumber { get; set; }
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
		public string Contact1 { get; set; }
		public string Mother_Occupation { get; set; }
		public string Me_Mail { get; set; }
		public string Mother_Office_Address { get; set; }
		public string Mother_Contact { get; set; }
		public string Temporary_Address { get; set; }
		public string Permanent_Address { get; set; }
		public string TalukaPer { get; set; }
		public string DistrictPer { get; set; }
		public int? PinPer { get; set; }
		public string TalukaTemp { get; set; }
		public string DistrictTemp { get; set; }
		public int? PinTemp { get; set; }
		public string Username { get; set; }
		public string Sex { get; set; }
		public string SeoUrl { get; set; }

		[UIHint("Picture")]
		public int CoverPictureId { get; set; }
		public string FacebookLink { get; set; }
		public string TweeterLink { get; set; }
		public string InstagramLink { get; set; }
		public string GooglePlusLink { get; set; }
		public string PInterestLink { get; set; }
		public string LinkedInLink { get; set; }
		public string Hi5Link { get; set; }

		[UIHint("Date")]
		public DateTime? AdmissionDate { get; set; }
		[UIHint("Picture")]
		public int PictureId { get; set; }
		[UIHint("Picture")]
		public int FatherPictureId { get; set; }
		[UIHint("Picture")]
		public int MotherPictureId { get; set; }
		public int ImpersonateId { get; set; }
		public string SystemName { get; set; }
		public IList<FilesModel> Files { get; set; }
		public PictureModel StudentPicture { get; set; }
		public PictureModel FatherPicture { get; set; }
		public PictureModel MotherPicture { get; set; }
		public IList<SelectListItem> AvailableCastes { get; set; }
		public IList<SelectListItem> AvailableReligions { get; set; }
		public IList<SelectListItem> AvailableHouses { get; set; }
	}

	public partial class StudentListModel
	{
		public StudentListModel()
		{
			Students = new List<StudentDataTable>();
		}

		public IList<StudentDataTable> Students { get; set; }
	}

	public partial class FileListModel
	{
		public int Id { get; set; }
		public int StudentId { get; set; }
		public int TeacherId { get; set; }
		public string Title { get; set; }
		public string Type { get; set; }
		public string FileSrc { get; set; }
	}

	public partial class StudentDataTable
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string IdentityNumber { get; set; }
		public string FatherName { get; set; }
		public string MotherName { get; set; }
		public string DateOfBirth { get; set; }
		public bool BusFacility { get; set; }
		public bool IsActive { get; set; }
		public string EmailAddress { get; set; }
		public string FatherContact { get; set; }
		public string Username { get; set; }
		public string Sex { get; set; }
		public string Url { get; set; }
		public string AdmissionDate { get; set; }
		public string SystemName { get; set; }
		public string PictureSrc { get; set; }

	}
}
