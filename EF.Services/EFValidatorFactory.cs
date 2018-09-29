using System;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Validators;

namespace EF.Services
{
	public class EFValidatorFactory : IValidatorFactory
	{
		public IValidator GetValidator(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return DependencyResolver.Current.GetService(typeof(IValidator<>).MakeGenericType(type)) as IValidator;
		}

		public IValidator<T> GetValidator<T>()
		{
			return DependencyResolver.Current.GetService<IValidator<T>>();
		}
	}
}