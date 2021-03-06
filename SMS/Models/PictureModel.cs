﻿using EF.Core.Data;
using EF.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMS.Models
{
	public partial class PictureModel : BaseEntityModel
	{
        public PictureModel()
        {
            AvailableAcadmicYears = new List<SelectListItem>();
        }
		public string Url { get; set; }
		public string PictureSrc { get; set; }
		public decimal Width { get; set; }
		public decimal Height { get; set; }
		public decimal Size { get; set; }
		public bool IsThumb { get; set; }
		public bool IsLogo { get; set; }
		public bool IsActive { get; set; }
		public string AlternateText { get; set; }
	    public string UploadedBy { get; set; }
        public bool CaptionOff { get; set; }
	    public bool IsOpenResource { get; set; }
        public string CreatedDateString { get; set; }

        public int DisplayOrder { get; set; }
        public int AcadmicYearId { get; set; }

        public IList<SelectListItem> AvailableAcadmicYears { get; set; }
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
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public NewsModel News { get; set; }
		public PictureModel Picture { get; set; }

	}

	public partial class PictureListModel
	{
		public int Id { get; set; }
		public int EventId { get; set; }
		public int BlogId { get; set; }
		public int NewsId { get; set; }
		public int ProductId { get; set; }
		public int PictureId { get; set; }
		public bool IsDefault { get; set; }
		public int DisplayOrder { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
        public string PictureSrc { get; set; }
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