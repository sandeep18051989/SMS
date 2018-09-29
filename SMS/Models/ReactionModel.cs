using System;
using EF.Services;
using SMS.Areas.Admin.Models;

namespace SMS.Models
{
	public partial class ReactionModel : BaseEntityModel
	{
		public ReactionModel()
		{
			Product = new ProductModel();;
			Event = new EventModel();
			Blog = new BlogModel(); ;
			News = new NewsModel();
			Picture = new PictureModel(); ;
			Video = new VideoModel();
			Comment = new CommentModel();
			Reply = new ReplyModel();
		}
		public string Username { get; set; }
		public bool? IsLike { get; set; }

		public bool? IsDislike { get; set; }

		public bool? IsAngry { get; set; }

		public bool? IsHappy { get; set; }

		public bool? IsSad { get; set; }

		public bool? IsLol { get; set; }

		public int? Rating { get; set; }

		public int? BlogId { get; set; }
		public int? ProductId { get; set; }
		public int? EventId { get; set; }
		public int? PictureId { get; set; }
		public int? VideoId { get; set; }
		public int? NewsId { get; set; }
		public int? CommentId { get; set; }
		public int? ReplyId { get; set; }

		public ProductModel Product { get; set; }
		public EventModel Event { get; set; }
		public BlogModel Blog { get; set; }
		public NewsModel News { get; set; }
		public PictureModel Picture { get; set; }
		public VideoModel Video { get; set; }
		public CommentModel Comment { get; set; }
		public ReplyModel Reply { get; set; }
	}
}
