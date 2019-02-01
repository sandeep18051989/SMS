using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(TeacherModelValidator))]
	public partial class TeacherModel : BaseEntityModel
	{
		public TeacherModel()
		{
			Files = new List<FilesModel>();
			Subjects = new List<SubjectModel>();
			ClassRoomDivisions = new List<ClassRoomDivisionModel>();
			AvailableAcadmicYears = new List<SelectListItem>();
			AvailableEmployees = new List<SelectListItem>();
			AvailableQualifications = new List<SelectListItem>();
			AvailablePersonalityStatuses = new List<SelectListItem>();
		}
		public string Name { get; set; }
		public string PictureSrc { get; set; }
		public string Url { get; set; }
		public string SystemName { get; set; }
		public int EmployeeId { get; set; }
		public int QualificationId { get; set; }
		public string Username { get; set; }
		[UIHint("Picture")]
		public int ProfilePictureId { get; set; }
		[UIHint("Picture")]
		public int CoverPictureId { get; set; }
		public string FacebookLink { get; set; }
		public string TweeterLink { get; set; }
		public string InstagramLink { get; set; }
		public string GooglePlusLink { get; set; }
		public string PInterestLink { get; set; }
		public string LinkedInLink { get; set; }
		public string UpworkLink { get; set; }
		public string GuruLink { get; set; }
		public string FreelancerLink { get; set; }
		public string Hi5Link { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public string AcadmicYear { get; set; }
		public int ImpersonateId { get; set; }
		[UIHint("File")]
		public int FileId { get; set; }
		public int? PersonalityStatusId { get; set; }

		public bool? IsPhoneVerified { get; set; }
		public bool? IsEmailVerified { get; set; }

		public IList<FilesModel> Files { get; set; }
		public IList<SubjectModel> Subjects { get; set; }
		public IList<ClassRoomDivisionModel> ClassRoomDivisions { get; set; }
		public IList<SelectListItem> AvailableEmployees { get; set; }
		public IList<SelectListItem> AvailableQualifications { get; set; }
		public IList<SelectListItem> AvailableAcadmicYears { get; set; }
		public IList<SelectListItem> AvailablePersonalityStatuses { get; set; }

	}
}
