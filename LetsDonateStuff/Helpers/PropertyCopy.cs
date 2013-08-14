using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Reflection;

namespace LetsDonateStuff.Helpers
{
    public static class PropertyCopy<TTarget> where TTarget : class, new()
    {
        /// <summary>
        /// Copies all readable properties from the source to a new instance
        /// of TTarget.
        /// </summary>
        public static TTarget Copy<TSource>(TSource source) where TSource : class
        {
            var target = new TTarget();
            Copy<TSource>(source, target);
            return target;
        }

        public static void Copy<TSource>(TSource source, TTarget target) where TSource : class
        {
            PropertyCopier<TSource>.Copy(source, target);
        }

        /// <summary>
        /// Static class to efficiently store the compiled delegate which can
        /// do the copying. We need a bit of work to ensure that exceptions are
        /// appropriately propagated, as the exception is generated at type initialization
        /// time, but we wish it to be thrown as an ArgumentException.
        /// </summary>
        static class PropertyCopier<TSource> where TSource : class
        {
            static readonly Action<TSource, TTarget> copier;
            static readonly Exception initializationException;

            internal static void Copy(TSource source, TTarget target)
            {
                if (initializationException != null)
                    throw initializationException;
                if (source == null)
                    throw new ArgumentNullException("source");
                copier(source, target);
            }

            static PropertyCopier()
            {
                try
                {
                    copier = BuildCopier();
                    initializationException = null;
                }
                catch (Exception e)
                {
                    copier = null;
                    initializationException = e;
                }
            }

            static Action<TSource, TTarget> BuildCopier()
            {
                ParameterExpression targetParameter = Expression.Parameter(typeof(TTarget), "target");
                ParameterExpression sourceParameter = Expression.Parameter(typeof(TSource), "source");
                var statements = new List<Expression>();
                foreach (PropertyInfo sourceProperty in typeof(TSource).GetProperties())
                {
                    if (!sourceProperty.CanRead)
                        continue;
                    PropertyInfo targetProperty = typeof(TTarget).GetProperty(sourceProperty.Name);
                    if (targetProperty == null || !targetProperty.CanWrite)
                        continue;
                    if (!targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                        throw new ArgumentException("Property " + sourceProperty.Name + " has an incompatible type in " + typeof(TTarget).FullName);

                    var toProperty = Expression.Property(targetParameter, targetProperty);
                    var fromProperty = Expression.Property(sourceParameter, sourceProperty);
                    statements.Add(Expression.Assign(toProperty, fromProperty));
                }
                return Expression.Lambda<Action<TSource,TTarget>>(Expression.Block(statements), sourceParameter, targetParameter).Compile();
            }
        }
    }
}