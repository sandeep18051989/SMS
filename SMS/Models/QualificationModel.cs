using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(QualificationModelValidator))]
	public partial class QualificationModel : BaseEntityModel
	{
		public string QualificationName { get; set; }
        public string Description { get; set; }
    }
}
