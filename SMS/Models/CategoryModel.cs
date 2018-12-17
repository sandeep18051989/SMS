using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(CategoryModelValidator))]
	public partial class CategoryModel : BaseEntityModel
	{
		public string Name { get; set; }
        public bool IsActive { get; set; }
        public string CreatedOnString { get; set; }
        public string ModifiedOnString { get; set; }

    }
}
