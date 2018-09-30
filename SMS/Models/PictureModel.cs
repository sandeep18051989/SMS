using EF.Core.Data;
using EF.Services;
using System;

namespace SMS.Models
{
	public partial class PictureModel : BaseEntityModel
	{
		public string Url { get; set; }
		public string Src { get; set; }
		public decimal Width { get; set; }
		public decimal Height { get; set; }
		public decimal Size { get; set; }
		public bool IsThumb { get; set; }
		public bool IsLogo { get; set; }
		public bool IsActive { get; set; }
		public string AlternateText { get; set; }
		public bool CaptionOff { get; set; }
	}

	public partial class BlogPictureModel : BaseEntityModel
	{
		public BlogPictureModel()
		{
			Picture = new PictureModel();
			Blog = new BlogModel();
		}

		public int PictureId { get; set; }
		public bool IsDefault { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime? PicStartDate { get; set; }
		public DateTime? PicEndDate { get; set; }
		public BlogModel Blog { get; set; }
		public PictureModel Picture { get; set; }

	}

    public partial class EventPictureModel : BaseEntityModel
    {
        public EventPictureModel()
        {
            Picture = new PictureModel();
            Event = new EventModel();
        }

        public int EventId { get; set; }
        public int PictureId { get; set; }
        public bool IsDefault { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime? PicStartDate { get; set; }
        public DateTime? PicEndDate { get; set; }
        public EventModel Event { get; set; }
        public PictureModel Picture { get; set; }

    }

    public partial class NewsPictureModel : BaseEntityModel
    {
        public NewsPictureModel()
        {
            Picture = new PictureModel();
            News = new NewsModel();
        }

        public int PictureId { get; set; }
        public int NewsId { get; set; }
        public bool IsDefault { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime? PicStartDate { get; set; }
        public DateTime? PicEndDate { get; set; }
        public NewsModel News { get; set; }
        public PictureModel Picture { get; set; }

    }

    public partial class ProductPictureModel : BaseEntityModel
    {
        public ProductPictureModel()
        {
            Picture = new PictureModel();
            Product = new ProductModel();
        }

        public int PictureId { get; set; }
        public int ProductId { get; set; }
        public bool IsDefault { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime? PicStartDate { get; set; }
        public DateTime? PicEndDate { get; set; }
        public ProductModel Product { get; set; }
        public PictureModel Picture { get; set; }

    }
}