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
        }
        public int Id { get; set; }
        public string ShortName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
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
        public IList<CommentWidgetModel> Comments { get; set; }
        public IList<ReactionModel> Reactions { get; set; }
        public IList<NewsPictureModel> Pictures { get; set; }
        public IList<NewsVideoModel> Videos { get; set; }

    }
}