using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class School : BaseEntity, ISlugSupported
	{
		public string FullName { get; set; }
		public string UserName { get; set; }
		public int SuperAdministratorId { get; set; }
		public bool IsApproved { get; set; }
		public string AffiliationNumber { get; set; }
		public Guid SchoolGuid { get; set; }
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
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		[NotMapped]
		public string AcadmicYearName { get; set; }
		public virtual User User { get; set; }

	}
}
