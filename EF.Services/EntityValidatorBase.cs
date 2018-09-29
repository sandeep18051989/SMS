using System.Linq;
using FluentValidation;

namespace EF.Services
{
    public abstract class EntityValidatorBase<T> : AbstractValidator<T> where T : class
    {
        protected EntityValidatorBase()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
            
        }
    }
}