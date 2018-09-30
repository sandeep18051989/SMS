using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(ProductValidator))]
	public partial class ProductModel : BaseEntityModel
	{
		public ProductModel()
		{
			Videos = new List<VideoModel>();
			Pictures = new List<ProductPictureModel>();
			Comments = new List<CommentModel>();
			Files = new List<FilesModel>();
			postCommentModel = new PostCommentsModel();
			Reactions = new List<ReactionModel>();
			ProductCategory = new ProductCategoryModel();
			Vendor = new VendorModel();
		}
		public string Name { get; set; }
		public string SystemName { get; set; }
		[AllowHtml]
		public string Description { get; set; }
		public string Url { get; set; }
		public string SeoName { get; set; }
		public bool IsInValidState { get; set; }
		public PostCommentsModel postCommentModel { get; set; }
		public ProductCategoryModel ProductCategory { get; set; }
		public IList<VideoModel> Videos { get; set; }
		public IList<ProductPictureModel> Pictures { get; set; }
		public IList<FilesModel> Files { get; set; }
		public IList<CommentModel> Comments { get; set; }
		public IList<ReactionModel> Reactions { get; set; }

		public VendorModel Vendor { get; set; }

	}

	[Validator(typeof(PostCommentModelValidator))]
	public partial class PostCommentsModel : BaseEntityModel
	{
		public PostCommentsModel()
		{
			postReplyModel = new PostReplyModel();
		}
		public int CommentId { get; set; }
		public int EntityId { get; set; }
		public string Type { get; set; }
		public int DisplayOrder { get; set; }
		[AllowHtml]
		[UIHint("HtmlEditor")]
		public string CommentHtml { get; set; }
		public string Username { get; set; }
		public PostReplyModel postReplyModel { get; set; }

		[Validator(typeof(PostReplyModelValidator))]
		public class PostReplyModel : BaseEntityModel
		{
			public int EntityId { get; set; }
			public int CommentId { get; set; }
			[AllowHtml]
			[UIHint("HtmlEditor")]
			public string ReplyHtml { get; set; }

			public string Type { get; set; }
			public int DisplayOrder { get; set; }
			public bool IsModified { get; set; }

			public string Username { get; set; }

		}

	}
}