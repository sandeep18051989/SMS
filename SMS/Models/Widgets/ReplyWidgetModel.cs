using System;
using System.Web.Mvc;

namespace SMS.Models.Widgets
{
    public partial class ReplyWidgetModel
    {
		public ReplyWidgetModel()
		{
		}
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string TeacherName { get; set; }
        [AllowHtml]
        public string ReplyHtml { get; set; }
        public string EmployeeName { get; set; }
        public int DisplayOrder { get; set; }
		public bool IsActive { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public string ProfilePictureSrc { get; set; }
    }
}