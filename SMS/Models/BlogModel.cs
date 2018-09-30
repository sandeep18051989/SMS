using System;
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
			Videos = new List<VideoModel>();
			Pictures = new List<BlogPictureModel>();
			Comments = new List<CommentModel>();
			Files = new List<FilesModel>();
			Reactions = new List<ReactionModel>();
			postCommentModel = new PostCommentsModel();
		}

		public string Subject { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string SystemName { get; set; }
		public string BlogHtml { get; set; }
		public string IpAddress { get; set; }

		public PostCommentsModel postCommentModel { get; set; }
		public IList<VideoModel> Videos { get; set; }
		public IList<ReactionModel> Reactions { get; set; }
		public IList<BlogPictureModel> Pictures { get; set; }
		public IList<FilesModel> Files { get; set; }
		public IList<CommentModel> Comments { get; set; }

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
}