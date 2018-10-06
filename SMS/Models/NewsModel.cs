using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EF.Core.Data;
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
			Videos = new List<VideoModel>();
			Pictures = new List<NewsPictureModel>();
			Comments = new List<CommentModel>();
			Users = new List<UserModel>();
			Files = new List<FilesModel>();
			AvailableStatuses = new List<SelectListItem>();
			InsertPictureModel = new InsertPicturesModel();
			InsertVideoModel = new InsertVideoModel();
		}
		public IList<VideoModel> Videos { get; set; }
		public IList<NewsPictureModel> Pictures { get; set; }
		public IList<CommentModel> Comments { get; set; }
		public IList<UserModel> Users { get; set; }
		public IList<FilesModel> Files { get; set; }
		public IList<SelectListItem> AvailableStatuses { get; set; }
		public string ShortName { get; set; }
		[UIHint("DateRange")]
		public DateTime? StartDate { get; set; }
		[UIHint("DateRange")]
		public DateTime? EndDate { get; set; }
		public DateTime? Date { get; set; }
		[UIHint("HtmlEditor")]
		public string Description { get; set; }
		public string SeoUrl { get; set; }
		public bool IsActive { get; set; }
		public string Author { get; set; }
		public int NewsStatusId { get; set; }
		public string SystemName { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }

		public InsertPicturesModel InsertPictureModel { get; set; }
		public InsertVideoModel InsertVideoModel { get; set; }

	}

	public partial class NewsListModel
	{
		public int Id { get; set; }
		public string ShortName { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public string Date { get; set; }
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
