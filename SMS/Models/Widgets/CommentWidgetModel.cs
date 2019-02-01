using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMS.Models.Widgets
{
    public partial class CommentWidgetModel
    {
        public CommentWidgetModel()
        {
            Replies = new List<ReplyWidgetModel>();
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public int DisplayOrder { get; set; }
        public string AcadmicYear { get; set; }
        [AllowHtml]
        public string CommentHtml { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public string ProfilePictureSrc { get; set; }
        public DateTime? CreatedOn { get; set; }

        public IList<ReplyWidgetModel> Replies { get; set; }
    }
}