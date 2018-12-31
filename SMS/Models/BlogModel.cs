using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(BlogModelValidator))]
	public partial class BlogModel : BaseEntityModel
	{
		public BlogModel()
		{
			AvailableAcadmicYears = new List<SelectListItem>();
		}
		public IList<SelectListItem> AvailableAcadmicYears { get; set; }

		[AllowHtml]
		[UIHint("HtmlEditor")]
		public string Subject { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public string Url { get; set; }
		public string SystemName { get; set; }
		[AllowHtml]
		[UIHint("HtmlEditor")]
		public string BlogHtml { get; set; }
		public string IpAddress { get; set; }

		public int AcadmicYearId { get; set; }
		[UIHint("Picture")]
		public int PictureId { get; set; }
		[UIHint("Video")]
		public int VideoId { get; set; }
		public string AcadmicYear { get; set; }

	}

	public partial class BlogListModel
	{
		public int Id { get; set; }
		public string Subject { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string SystemName { get; set; }
		public string BlogHtml { get; set; }
		public bool IsActive { get; set; }
		public string AcadmicYear { get; set; }
		public int AcadmicYearId { get; set; }
		public string IpAddress { get; set; }
		public int VideosCount { get; set; }
		public int PicturesCount { get; set; }
		public int CommentsCount { get; set; }
		public int ReactionsCount { get; set; }
	}

}