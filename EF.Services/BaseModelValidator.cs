using EF.Data;
using FluentValidation;
using System.Linq;
using System.Linq.Dynamic;

namespace EF.Services
{
    public abstract class BaseModelValidator<T> : AbstractValidator<T> where T : class
    {
        protected BaseModelValidator()
        {
            PostInitialize();
        }

        protected virtual void PostInitialize()
        {

        }

        protected virtual void SetDatabaseValidationRules<TObject>(IDbContext dbContext, params string[] filterStringPropertyNames)
        {
            SetStringPropertiesMaxLength<TObject>(dbContext, filterStringPropertyNames);
            SetDecimalMaxValue<TObject>(dbContext);
        }

        protected virtual void SetStringPropertiesMaxLength<TObject>(IDbContext dbContext, params string[] filterPropertyNames)
        {
            if (dbContext == null)
                return;

            var dbObjectType = typeof(TObject);

            var names = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string) && !filterPropertyNames.Contains(p.Name))
                .Select(p => p.Name).ToArray();

            var maxLength = dbContext.GetColumnsMaxLength(dbObjectType.Name, names);
            var expression = maxLength.Keys.ToDictionary(name => name, name => DynamicExpression.ParseLambda<T, string>(name, null));

            foreach (var expr in expression)
            {
                RuleFor(expr.Value).Length(0, maxLength[expr.Key]);
            }
        }

        protected virtual void SetDecimalMaxValue<TObject>(IDbContext dbContext)
        {
            if (dbContext == null)
                return;

            var dbObjectType = typeof(TObject);

            var names = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(decimal))
                .Select(p => p.Name).ToArray();

            var maxValues = dbContext.GetDecimalMaxValue(dbObjectType.Name, names);
            var expression = maxValues.Keys.ToDictionary(name => name, name => DynamicExpression.ParseLambda<T, decimal>(name, null));

            foreach (var expr in expression)
            {
                RuleFor(expr.Value).IsDecimal(maxValues[expr.Key]).WithMessage(string.Format("The value is out of range. Maximum value is {0}.99", maxValues[expr.Key] - 1));
            }
        }
    }
}