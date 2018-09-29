using System;
using System.Collections.Generic;
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
		}
		public IList<VideoModel> Videos { get; set; }
		public IList<NewsPictureModel> Pictures { get; set; }
		public IList<CommentModel> Comments { get; set; }
		public IList<UserModel> Users { get; set; }
		public IList<FilesModel> Files { get; set; }
		public IList<SelectListItem> AvailableStatuses { get; set; }
		public string ShortName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
		public string SeoUrl { get; set; }
		public string Author { get; set; }
		public int NewsStatusId { get; set; }
		public string SystemName { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }
	}
}
