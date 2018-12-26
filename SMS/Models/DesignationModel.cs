using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMS.Models
{
	[Validator(typeof(DesignationModelValidator))]
	public partial class DesignationModel : BaseEntityModel
	{
		public string Name { get; set; }
        [AllowHtml]
        [UIHint("HtmlEditor")]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
