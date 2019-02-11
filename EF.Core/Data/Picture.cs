using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Picture : BaseEntity
	{
        private ICollection<BlogPicture> _Blog;
        private ICollection<ProductPicture> _Products;
        private ICollection<EventPicture> _Events;
        private ICollection<Slider> _Sliders;
        private ICollection<NewsPicture> _News;
        private ICollection<Reaction> _Reactions;
		public string PictureSrc { get; set; }
		public string Url { get; set; }
		public decimal Width { get; set; }
		public decimal Height { get; set; }
		public decimal Size { get; set; }
		public bool IsThumb { get; set; }
		public int DisplayOrder { get; set; }
		public bool IsLogo { get; set; }
	    public bool IsOpenResource { get; set; }
        public bool IsActive { get; set; }
		public string AlternateText { get; set; }
		public int AcadmicYearId { get; set; }

		#region Navigation Properties
		public virtual ICollection<BlogPicture> Blogs
		{
			get { return _Blog ?? (_Blog = new List<BlogPicture>()); }
			protected set { _Blog = value; }
		}

		public virtual ICollection<ProductPicture> Products
		{
			get { return _Products ?? (_Products = new List<ProductPicture>()); }
			protected set { _Products = value; }
		}

		public virtual ICollection<EventPicture> Events
		{
			get { return _Events ?? (_Events = new List<EventPicture>()); }
			protected set { _Events = value; }
		}

		public virtual ICollection<NewsPicture> News
		{
			get { return _News ?? (_News = new List<NewsPicture>()); }
			protected set { _News = value; }
		}
		public virtual ICollection<Reaction> Reactions
		{
			get { return _Reactions ?? (_Reactions = new List<Reaction>()); }
			protected set { Reactions = value; }
		}

        public virtual ICollection<Slider> Sliders
        {
            get { return _Sliders ?? (_Sliders = new List<Slider>()); }
            protected set { _Sliders = value; }
        }

        #endregion

    }
}
