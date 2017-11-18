using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CAPNet
{
    /// <summary>
    /// Entity validator extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IEnumerable<Error> GetErrorsFromAllEntityValidators<T>(this T entity)
        {
            var validatorType = typeof(IValidator<T>);
            var validatorAssembly = validatorType.GetTypeInfo().Assembly;

            var entityValidators =
                from typeInfo in validatorAssembly.DefinedTypes
                where typeInfo.ImplementedInterfaces.Contains(typeof(IValidator<T>))
                select (IValidator<T>)Activator.CreateInstance(typeInfo.AsType(), entity);

            return from validator in entityValidators
                   from error in validator.Errors
                   select error;
        }
    }
}