using System.Collections.Generic;
using EF.Core.Data;
using EF.Services;

namespace SMS.Models
{
	public partial class UserInfoModel : BaseEntityModel
    {
        public UserInfoModel()
        {
            user = new User();
            ProductsUploadedList = new List<Product>();
            EventsUploadedList = new List<Event>();
            BlogsUploadedList = new List<Blog>();
            CommentsList = new List<Comment>();
            Activities = new List<AuditModel>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int CityId { get; set; }
        public string Phone { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsEmailVerified { get; set; }
        public string Hobbies { get; set; }
        public string BriefIntroduction { get; set; }
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

        public string Hi5Link { get; set; }

        public IList<Product> ProductsUploadedList { get; set; }
        public IList<Event> EventsUploadedList { get; set; }
        public IList<Blog> BlogsUploadedList { get; set; }
        public IList<Comment> CommentsList { get; set; }
        public IList<AuditModel> Activities { get; set; }
        public User user { get; set; }

    }
}