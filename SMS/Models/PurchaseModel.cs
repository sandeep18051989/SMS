using System;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(PurchaseModelValidator))]
	public partial class PurchaseModel : BaseEntityModel
	{
		public PurchaseModel()
		{
			Product = new ProductModel();
			Vendor = new VendorModel();
		}
		public string IName { get; set; }
		public double IQuantity { get; set; }
		public double IRate { get; set; }
		public DateTime IPurchaseDate { get; set; }
		public double ITax { get; set; }
		public double ITotal { get; set; }
		public int VendorId { get; set; }
		public int ProductId { get; set; }
		public ProductModel Product { get; set; }
		public VendorModel Vendor { get; set; }
	}
}
