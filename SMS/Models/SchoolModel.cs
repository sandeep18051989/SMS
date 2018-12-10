using System.Collections.Generic;
using EF.Core.Data;
using EF.Services;

namespace SMS.Models
{
	public partial class SchoolModel : BaseEntityModel
	{
		public SchoolModel()
		{
			User = new UserModel();
			News = new List<NewsModel>();
			Blogs = new List<BlogModel>();
			Comments = new List<CommentModel>();
			Events = new List<EventModel>();
			ProfilePicture = new PictureModel();
			CoverPicture = new PictureModel();
		}

		public string FullName { get; set; }
		public string UserName { get; set; }

        public string Email { get; set; }
        public int SuperAdministratorId { get; set; }
		public bool IsApproved { get; set; }
		public string AffiliationNumber { get; set; }
		public string SystemName { get; set; }
		public int ProfilePictureId { get; set; }
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
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string Landmark { get; set; }
		public string Longitude { get; set; }
		public string Latitude { get; set; }
		public string ZipCode { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string RegistrationNumber { get; set; }
		public int TotalTeachers { get; set; }
		public int TotalStudents { get; set; }
		public int AcadmicYearId { get; set; }
		public string AcadmicYearName { get; set; }

	    public AcadmicYear AcadmicYear { get; set; }
        public PictureModel ProfilePicture { get; set; }
		public PictureModel CoverPicture { get; set; }
		public bool IsActive { get; set; }

		public UserModel User { get; set; }
		public IList<NewsModel> News { get; set; }
		public IList<BlogModel> Blogs { get; set; }
		public IList<CommentModel> Comments { get; set; }
		public IList<EventModel> Events { get; set; }

	}
}