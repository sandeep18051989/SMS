using EF.Core.Data;
using System;
using EF.Services;

namespace SMS.Models
{
	public partial class VideoModel : BaseEntityModel
	{
		public string Url { get; set; }
		public string VideoSrc { get; set; }
		public decimal Size { get; set; }
		public bool IsActive { get; set; }
	}

	public partial class EventVideoModel : BaseEntityModel
	{
		public EventVideoModel()
		{
			Video = new VideoModel();
			Event = new EventModel();
		}

		public int EventId { get; set; }
		public int VideoId { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime? VidStartDate { get; set; }
		public DateTime? VidEndDate { get; set; }
		public EventModel Event { get; set; }
		public VideoModel Video { get; set; }

	}

	public partial class ProductVideoModel : BaseEntityModel
	{
		public ProductVideoModel()
		{
			Video = new VideoModel();
			Product = new ProductModel();
		}

		public int EventId { get; set; }
		public int VideoId { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime? VidStartDate { get; set; }
		public DateTime? VidEndDate { get; set; }
		public ProductModel Product { get; set; }
		public VideoModel Video { get; set; }

	}

	public partial class NewsVideoModel : BaseEntityModel
	{
		public NewsVideoModel()
		{
			Video = new VideoModel();
			News = new NewsModel();
		}

		public int EventId { get; set; }
		public int VideoId { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime? VidStartDate { get; set; }
		public DateTime? VidEndDate { get; set; }
		public NewsModel News { get; set; }
		public VideoModel Video { get; set; }

	}

	public partial class EventVideoListModel
	{
		public int Id { get; set; }
		public int EventId { get; set; }
		public int VideoId { get; set; }
		public int DisplayOrder { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public string VideoSrc { get; set; }
	}
}