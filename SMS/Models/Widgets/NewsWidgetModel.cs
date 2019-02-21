using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMS.Models.Widgets
{
    public partial class NewsWidgetModel
    {
        public NewsWidgetModel()
        {
            Comments = new List<CommentWidgetModel>();
            Reactions = new List<ReactionModel>();
            Pictures = new List<NewsPictureModel>();
            Videos = new List<NewsVideoModel>();
            Student = new StudentModel();
            Teacher = new TeacherModel();
            LatestPosts = new List<NewsWidgetModel>();
            OlderPosts = new List<NewsWidgetModel>();
            User = new UserModel();
        }
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public UserModel User { get; set; }
        public bool IsStudent { get; set; }
        public bool IsTeacher { get; set; }

        [AllowHtml]
        public string Description { get; set; }
        public string SystemName { get; set; }
        public string Author { get; set; }
        public int NewsStatusId { get; set; }
        public string DefaultPictureSrc { get; set; }
        public string DefaultVideoSrc { get; set; }
        public bool HasDefaultPicture { get; set; }
        public bool HasDefaultVideo { get; set; }
        public string Status { get; set; }
        public StudentModel Student { get; set; }
        public TeacherModel Teacher { get; set; }

        public bool IsAuthenticated { get; set; }
        public IList<CommentWidgetModel> Comments { get; set; }
        public IList<ReactionModel> Reactions { get; set; }
        public IList<NewsPictureModel> Pictures { get; set; }
        public IList<NewsVideoModel> Videos { get; set; }

        public IList<NewsWidgetModel> LatestPosts { get; set; }
        public IList<NewsWidgetModel> OlderPosts { get; set; }

    }
}