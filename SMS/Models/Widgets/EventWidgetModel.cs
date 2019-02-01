﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMS.Models.Widgets
{
    public partial class EventWidgetModel
	{
		public EventWidgetModel()
		{
            Comments = new List<CommentWidgetModel>();
            Reactions = new List<ReactionModel>();
            Pictures = new List<EventPictureModel>();
            Videos = new List<EventVideoModel>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
		public string SystemName { get; set; }
        public string Headline { get; set; }
        public string AcadmicYear { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		[AllowHtml]
		public string Description { get; set; }
		public bool IsActive { get; set; }
		public bool IsApproved { get; set; }
		public bool IsClosed { get; set; }
		public string Venue { get; set; }
		public string Url { get; set; }
		public string DefaultPictureSrc { get; set; }
		public string DefaultVideoSrc { get; set; }
        public bool HasDefaultPicture { get; set; }
        public bool HasDefaultVideo { get; set; }

        public IList<CommentWidgetModel> Comments { get; set; }
        public IList<ReactionModel> Reactions { get; set; }

        public IList<EventPictureModel> Pictures { get; set; }
        public IList<EventVideoModel> Videos { get; set; }

    }
}