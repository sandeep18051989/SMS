using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(MessageGroupModelValidator))]
	public partial class MessageGroupModel : BaseEntityModel
	{
		public string GroupName { get; set; }
	}
}
