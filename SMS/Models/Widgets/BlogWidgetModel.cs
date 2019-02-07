using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMS.Models.Widgets
{
    public partial class BlogWidgetModel
    {
		public BlogWidgetModel()
		{
            Comments = new List<CommentWidgetModel>();
            Reactions = new List<ReactionModel>();
            Pictures = new List<BlogPictureModel>();
            Videos = new List<BlogVideoModel>();
            Student = new StudentModel();
            Teacher = new TeacherModel();
            LatestPosts = new List<BlogWidgetModel>();
            OlderPosts = new List<BlogWidgetModel>();
            PopularPosts = new List<BlogWidgetModel>();
            Subjects = new Dictionary<string, int>();
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string SystemName { get; set; }
        public string Name { get; set; }
        public string AcadmicYear { get; set; }
		[AllowHtml]
		public string BlogHtml { get; set; }
        public string IpAddress { get; set; }
        public bool IsActive { get; set; }
		public bool IsApproved { get; set; }
		public string DefaultPictureSrc { get; set; }
		public string DefaultVideoSrc { get; set; }
        public bool HasDefaultPicture { get; set; }
        public bool HasDefaultVideo { get; set; }
        public bool IsStudent { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsAuthenticated { get; set; }
        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public StudentModel Student { get; set; }
        public TeacherModel Teacher { get; set; }

        public UserModel User { get; set; }
        public IList<CommentWidgetModel> Comments { get; set; }
        public IList<ReactionModel> Reactions { get; set; }
        public IList<BlogPictureModel> Pictures { get; set; }
        public IList<BlogVideoModel> Videos { get; set; }

        public IList<BlogWidgetModel> LatestPosts { get; set; }
        public IList<BlogWidgetModel> OlderPosts { get; set; }
        public IList<BlogWidgetModel> PopularPosts { get; set; }
        public IDictionary<string, int> Subjects { get; set; }
    }
}