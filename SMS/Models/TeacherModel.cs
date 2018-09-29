using System.Collections.Generic;
using System.Web.Mvc;
using EF.Core.Data;
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
			Classes = new List<ClassModel>();
			Divisions = new List<DivisionModel>();
			Employee = new EmployeeModel();
			Qualification = new QualificationModel();
			AvailableEmployees = new List<SelectListItem>();
			AvailableQualifications = new List<SelectListItem>();
		}
		public string Name { get; set; }
		public string SystemName { get; set; }
		public int EmployeeId { get; set; }
		public int QualificationId { get; set; }
		public int PictureId { get; set; }
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
		public string Username { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }

		public IList<FilesModel> Files { get; set; }
		public IList<SubjectModel> Subjects { get; set; }
		public IList<ClassModel> Classes { get; set; }
		public IList<DivisionModel> Divisions { get; set; }
		public EmployeeModel Employee { get; set; }
		public QualificationModel Qualification { get; set; }
		public PictureModel Picture { get; set; }
		public IList<SelectListItem> AvailableEmployees { get; set; }
		public IList<SelectListItem> AvailableQualifications { get; set; }

	}
}
