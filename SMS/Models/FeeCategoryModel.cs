using System;
using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(FeeCategoryModelValidator))]
	public partial class FeeCategoryModel : BaseEntityModel
	{
		public FeeCategoryModel()
		{
			Category = new CategoryModel();
			Class = new ClassModel();
		}
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public int ClassId { get; set; }
		public double FeeAmount { get; set; }
		public DateTime PeriodFrom { get; set; }
		public DateTime PeriodTo { get; set; }
		public CategoryModel Category { get; set; }
		public ClassModel Class { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }
	}
}
