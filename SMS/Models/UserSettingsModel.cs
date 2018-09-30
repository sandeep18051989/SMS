using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(UserSettingsModelValidator))]
    public class UserSettingsModel  : BaseEntityModel
    {
        public string Result { get; set; }
        public string ActiveSettings { get; set; }

    }
}