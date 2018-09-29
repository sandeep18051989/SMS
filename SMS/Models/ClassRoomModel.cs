using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(ClassRoomModelValidator))]
	public partial class ClassRoomModel : BaseEntityModel
	{
		public string Number { get; set; }
		public string Description { get; set; }
	}
}
