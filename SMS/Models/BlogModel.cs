using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using SMS.Models.Widgets;

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
        public bool IsApproved { get; set; }
        public int CommentsCount { get; set; }
        public int PicturesCount { get; set; }
        public int ReactionsCount { get; set; }
        public int VideosCount { get; set; }
	}

	public partial class BlogListWidgetModel
    {
        public BlogListWidgetModel()
        {
            Blogs = new List<BlogWidgetModel>();
            Users = new List<UserModel>();
            Subjects = new Dictionary<string, int>();
            PagingFilteringContext = new PagingFilteringModel();
        }
        public IList<BlogWidgetModel> Blogs { get; set; }
        public IList<UserModel> Users { get; set; }
        public PagingFilteringModel PagingFilteringContext { get; set; }
        public IDictionary<string, int> Subjects { get; set; }
    }

}