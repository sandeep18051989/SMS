﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Product : BaseEntity, ISlugSupported
	{
		#region Collections
		[NotMapped]
		public virtual ICollection<ProductPicture> _Pictures { get; set; }
		[NotMapped]
		public virtual ICollection<ProductVideo> _Videos { get; set; }
		[NotMapped]
		public virtual ICollection<Comment> _Comments { get; set; }
		[NotMapped]
		public virtual ICollection<File> _Files { get; set; }
		[NotMapped]
		public virtual ICollection<Reaction> _Reactions { get; set; }
		[NotMapped]
		public virtual ICollection<ProductCategoryMapping> _ProductCategories { get; set; }
		#endregion

		public int? VendorId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int? AcadmicYearId { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public DateTime? AvailableStartDate { get; set; }
        public DateTime? AvailableEndDate { get; set; }
        public bool IsUpcoming { get; set; }
        public bool MarkAsNew { get; set; }
        public DateTime? MarkAsNewStartDate { get; set; }
        public DateTime? MarkAsNewEndDate { get; set; }
        public double? OldPrice { get; set; }
        public double Price { get; set; }
        public double BasePrice { get; set; }
        public bool DisableBuyButton { get; set; }
        public int StockQuantity { get; set; }

		#region Navigation Properties
		public virtual ICollection<ProductPicture> Pictures
		{
			get { return _Pictures ?? (_Pictures = new List<ProductPicture>()); }
			protected set { _Pictures = value; }
		}
		public virtual ICollection<ProductVideo> Videos
		{
			get { return _Videos ?? (_Videos = new List<ProductVideo>()); }
			protected set { _Videos = value; }
		}
		public virtual ICollection<Comment> Comments
		{
			get { return _Comments ?? (_Comments = new List<Comment>()); }
			protected set { _Comments = value; }
		}

		public virtual ICollection<ProductCategoryMapping> ProductCategories
		{
			get { return _ProductCategories ?? (_ProductCategories = new List<ProductCategoryMapping>()); }
			protected set { _ProductCategories = value; }
		}
		public virtual ICollection<File> Files
		{
			get { return _Files ?? (_Files = new List<File>()); }
			protected set { _Files = value; }
		}
		public virtual ICollection<Reaction> Reactions
		{
			get { return _Reactions ?? (_Reactions = new List<Reaction>()); }
			protected set { Reactions = value; }
		}
		#endregion

	}
}
