using EF.Services;
using SMS.Models;

namespace SMS.Validations
{
	public class UserSettingsModelValidator : EntityValidatorBase<UserSettingsModel>
    {
        public UserSettingsModelValidator()
        {
        }
    }
}