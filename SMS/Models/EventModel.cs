using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMS.Models
{
	[Validator(typeof(EventModelValidator))]
	public partial class EventModel : BaseEntityModel
	{
		public EventModel()
		{
			AvailableAcadmicYears = new List<SelectListItem>();
		}
		public string Title { get; set; }
		public string SystemName { get; set; }
		public int AcadmicYearId { get; set; }
		public string AcadmicYear { get; set; }
		[UIHint("DateRange")]
		public DateTime? StartDate { get; set; }
		[UIHint("DateRange")]
		public DateTime? EndDate { get; set; }

        public string HeadLine { get; set; }
		[UIHint("HtmlEditor")]
		public string Description { get; set; }
		public bool IsActive { get; set; }
		public bool IsApproved { get; set; }
		public bool IsClosed { get; set; }
		public string Venue { get; set; }
		public string Url { get; set; }
		[UIHint("Picture")]
		public int PictureId { get; set; }
		[UIHint("Video")]
		public int VideoId { get; set; }

		public IList<SelectListItem> AvailableAcadmicYears { get; set; }

	}

	public partial class InsertPicturesModel : BaseEntityModel
	{
		[UIHint("Picture")]
		public int PictureId { get; set; }
		public int DisplayOrder { get; set; }
		public string Url { get; set; }
		public string PictureSrc { get; set; }
		public decimal Width { get; set; }
		public decimal Height { get; set; }
		public decimal Size { get; set; }
		public bool IsThumb { get; set; }
		public bool IsLogo { get; set; }
		public bool IsDefault { get; set; }
		[UIHint("DateRange")]
		public DateTime? PicStartDate { get; set; }
		[UIHint("DateRange")]
		public DateTime? PicEndDate { get; set; }
		public string AlternateText { get; set; }
	}

	public partial class InsertVideoModel : BaseEntityModel
	{
		[UIHint("Video")]
		public int VideoId { get; set; }
		public int DisplayOrder { get; set; }
		public string Url { get; set; }
		public string VideoSrc { get; set; }
		public decimal Size { get; set; }
		[UIHint("DateRange")]
		public DateTime? VidStartDate { get; set; }
		[UIHint("DateRange")]
		public DateTime? VidEndDate { get; set; }
	}

	public partial class EventListModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string SystemName { get; set; }
		public string AcadmicYear { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsApproved { get; set; }
		public bool IsClosed { get; set; }
		public string Venue { get; set; }
		public string Url { get; set; }
		public int VideosCount { get; set; }
		public int PicturesCount { get; set; }
		public int CommentsCount { get; set; }
		public int ReactionsCount { get; set; }
        public String Headline { get; set; }
    }

}