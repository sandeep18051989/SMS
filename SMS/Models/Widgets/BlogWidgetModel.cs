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
        }
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
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

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public IList<CommentWidgetModel> Comments { get; set; }
        public IList<ReactionModel> Reactions { get; set; }

        public IList<BlogPictureModel> Pictures { get; set; }
        public IList<BlogVideoModel> Videos { get; set; }

    }
}