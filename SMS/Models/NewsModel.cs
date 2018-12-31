using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(NewsModelValidator))]
	public partial class NewsModel : BaseEntityModel
	{
		public NewsModel()
		{
			AvailableStatuses = new List<SelectListItem>();
			AvailableAcadmicYears = new List<SelectListItem>();
		}
		public IList<SelectListItem> AvailableStatuses { get; set; }
		public IList<SelectListItem> AvailableAcadmicYears { get; set; }
		public string ShortName { get; set; }
		[UIHint("DateRange")]
		public DateTime? StartDate { get; set; }
		[UIHint("DateRange")]
		public DateTime? EndDate { get; set; }
		[AllowHtml]
		[UIHint("HtmlEditor")]
		public string Description { get; set; }
		public string SeoUrl { get; set; }
		public bool IsActive { get; set; }
		public string Author { get; set; }
		public int NewsStatusId { get; set; }
		public string SystemName { get; set; }
		public int AcadmicYearId { get; set; }
		public string AcadmicYear { get; set; }
		public string CreatedOnString { get; set; }
		public string ModifiedOnString { get; set; }
		[UIHint("Picture")]
		public int PictureId { get; set; }
		[UIHint("Video")]
		public int VideoId { get; set; }


	}

	public partial class NewsListModel
	{
		public int Id { get; set; }
		public string ShortName { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public string Description { get; set; }
		public string AcadmicYear { get; set; }
		public string SeoUrl { get; set; }
		public string Author { get; set; }
		public bool IsActive { get; set; }
		public string SystemName { get; set; }
		public int AcadmicYearId { get; set; }
		public int VideosCount { get; set; }
		public int PicturesCount { get; set; }
		public int CommentsCount { get; set; }
		public int ReactionsCount { get; set; }
	}
}
