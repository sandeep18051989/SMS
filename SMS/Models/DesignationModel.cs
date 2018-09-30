using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(DesignationModelValidator))]
	public partial class DesignationModel : BaseEntityModel
	{
		public string DesignationName { get; set; }
	}
}
