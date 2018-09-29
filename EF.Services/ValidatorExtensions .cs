using EF.Services;
using FluentValidation;

namespace EF.Services
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, decimal> IsDecimal<T>(this IRuleBuilder<T, decimal> ruleBuilder, decimal maxValue)
        {
            return ruleBuilder.SetValidator(new DecimalPropertyValidator(maxValue));
        }
    }
}
