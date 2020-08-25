using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Flujo.DataAccess.FlujoRepositories.WebClient.Helpers
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> query, string propertyName, bool ascending)
        {
            ParameterExpression prm = Expression.Parameter(typeof(T), "it");

            Expression property = Expression.Property(prm, propertyName);

            Type propertyType = property.Type;

            MethodInfo method = typeof(LinqExtensions).GetMethod("IEnumerableOrderByProperty", BindingFlags.Static | BindingFlags.NonPublic)

                .MakeGenericMethod(typeof(T), propertyType);

            return (IEnumerable<T>)method.Invoke(null, new object[] { query, prm, property, ascending });
        }

        private static IEnumerable<T> IEnumerableOrderByProperty<T, P>(this IEnumerable<T> query, ParameterExpression prm, Expression property, bool ascending)
        {
            Func<IEnumerable<T>, Func<T, P>, IEnumerable<T>> orderBy = (q, p) => ascending ? q.OrderBy(p) : q.OrderByDescending(p);

            return orderBy(query, Expression.Lambda<Func<T, P>>(property, prm).Compile());

        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, bool ascending)
        {
            // create the 'it' piece of Orderby it.PropertyName ascending

            ParameterExpression prm = Expression.Parameter(typeof(T), "it");

            // create the 'it.PropertyName' call
            Expression property = Expression.Property(prm, propertyName);

            // get the return type of the property

            Type propertyType = property.Type;

            // use reflection to get and call your own generic method that composes

            // the orderby into the query.

            MethodInfo method = typeof(LinqExtensions).GetMethod("QuerableOrderByProperty", BindingFlags.Static | BindingFlags.NonPublic)

                .MakeGenericMethod(typeof(T), propertyType);

            return (IOrderedQueryable<T>)method.Invoke(null, new object[] { query, prm, property, ascending });
        }

        private static IOrderedQueryable<T> QuerableOrderByProperty<T, P>(this IQueryable<T> query, ParameterExpression prm, Expression property, bool ascending)
        {
            // create a func that merges the property call into the orderby
            Func<IQueryable<T>, Expression<Func<T, P>>, IOrderedQueryable<T>> orderBy = (q, p) => ascending ? q.OrderBy(p) : q.OrderByDescending(p);
            // execute

            return orderBy(query, Expression.Lambda<Func<T, P>>(property, prm));
        }
    }
}
