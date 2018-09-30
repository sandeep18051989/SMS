using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(ReligionModelValidator))]
	public partial class ReligionModel : BaseEntityModel
	{
		public string Name { get; set; }

        public string Description { get; set; }
    }
}
