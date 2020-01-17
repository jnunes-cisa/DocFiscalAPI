using Repositories.Entities;
using Repositories.Extensions;
using Repositories.Helpers;
using Repositories.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repositories.Extensions
{

    public static class IQueryableExtensions
    {

        public static IOrderedQueryable<TEntity> Sort<TEntity>(this IQueryable<TEntity> source, SortQuery sortQuery)
        {

            return sortQuery.Direction == SortDirection.Descending
                ? OrderByDescending(source, sortQuery.SortedAttribute)
                : OrderBy(source, sortQuery.SortedAttribute);

        }

        public static IOrderedQueryable<TEntity> Sort<TEntity>(this IOrderedQueryable<TEntity> source, SortQuery sortQuery)
        {

            return sortQuery.Direction == SortDirection.Descending
                ? ThenByDescending(source, sortQuery.SortedAttribute)
                : ThenBy(source, sortQuery.SortedAttribute);

        }

        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string propertyName)
        {

            return CallGenericOrderMethod(source, propertyName, "OrderBy");

        }

        public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(this IQueryable<TEntity> source, string propertyName)
        {

            return CallGenericOrderMethod(source, propertyName, "OrderByDescending");

        }

        public static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IOrderedQueryable<TEntity> source, string propertyName)
        {

            return CallGenericOrderMethod(source, propertyName, "ThenBy");

        }

        public static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(this IOrderedQueryable<TEntity> source, string propertyName)
        {

            return CallGenericOrderMethod(source, propertyName, "ThenByDescending");

        }

        private static IOrderedQueryable<TEntity> CallGenericOrderMethod<TEntity>(this IQueryable<TEntity> source, string propertyName, string method)
        {

            // {x}
            var parameter = Expression.Parameter(typeof(TEntity), "x");

            // {x.propertyName}
            var property = Expression.Property(parameter, propertyName);

            // {x=>x.propertyName}
            var lambda = Expression.Lambda(property, parameter);

            // REFLECTION: source.OrderBy(x => x.Property)
            var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == method && x.GetParameters().Length == 2);
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TEntity), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<TEntity>)result;

        }

        private static Expression GetFilterExpressionLambda(Expression left, Expression right, FilterOperations operation)
        {

            Expression body;

            switch (operation)
            {
                case FilterOperations.eq:
                    // {model.Id == 1}
                    body = Expression.Equal(left, right);
                    break;
                case FilterOperations.lt:
                    // {model.Id < 1}
                    body = Expression.LessThan(left, right);
                    break;
                case FilterOperations.gt:
                    // {model.Id > 1}
                    body = Expression.GreaterThan(left, right);
                    break;
                case FilterOperations.le:
                    // {model.Id <= 1}
                    body = Expression.LessThanOrEqual(left, right);
                    break;
                case FilterOperations.ge:
                    // {model.Id <= 1}
                    body = Expression.GreaterThanOrEqual(left, right);
                    break;
                case FilterOperations.like:
                    // {model.Id <= 1}
                    body = Expression.Call(left, "Contains", null, right);
                    break;
                default:
                    throw new Exception($"500 - Filtro desconhecido para esta operação {operation}");

            }

            return body;

        }

        public static IQueryable<TEntity> Select<TEntity>(this IQueryable<TEntity> source, List<string> columns)
        {

            if (columns == null || columns.Any() == false)
                return source;

            var sourceType = source.ElementType;

            var resultType = typeof(TEntity);

            var parameter = Expression.Parameter(sourceType, "model");

            var bindings = columns.Select(column => Expression.Bind(
                resultType.GetProperty(column), Expression.PropertyOrField(parameter, column)));

            var body = Expression.MemberInit(Expression.New(resultType), bindings);

            var selector = Expression.Lambda(body, parameter);

            return source.Provider.CreateQuery<TEntity>(
                Expression.Call(typeof(Queryable), "Select", new[] { sourceType, resultType },
                source.Expression, Expression.Quote(selector)));

        }

        public static IQueryable<TEntity> Filter<TEntity>(this IQueryable<TEntity> source, FilterQuery filterQuery)
        {
            if (filterQuery == null)
                return source;

            if (filterQuery.Key.Contains('.'))
            {
                var relationshipArray = filterQuery.Key.Split('.');
                var concreteType = typeof(TEntity);

                var relation = concreteType.GetProperty(relationshipArray[0]);
                if (relation == null)
                    throw new ArgumentException($"'{relationshipArray[0]}' não é um relacionamento válido de '{concreteType}'");

                var relatedType = relation.PropertyType;
                var relatedAttr = relatedType.GetProperty(relationshipArray[1]);
                if (relatedAttr == null)
                    throw new ArgumentException($"'{relationshipArray[1]}' não é um atributo válido de '{relationshipArray[0]}'");
                try
                {

                    var convertedValue = TypeHelper.ConvertType(filterQuery.Value, relatedAttr.PropertyType);

                    var parameter = Expression.Parameter(concreteType, "model");

                    var leftRelationship = Expression.PropertyOrField(parameter, relation.Name);
                    var left = Expression.PropertyOrField(leftRelationship, relatedAttr.Name);
                    var right = Expression.Constant(convertedValue, relatedAttr.PropertyType);

                    var body = GetFilterExpressionLambda(left, right, filterQuery.Operation);

                    var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

                    return source.Where(lambda);

                }
                catch (FormatException)
                {

                    throw new Exception($"400 - Não foi possível converter {filterQuery.Value} para {relatedAttr.PropertyType.Name}");

                }

            }
            else
            {

                var concreteType = typeof(TEntity);
                var property = concreteType.GetProperty(filterQuery.Key);

                if (property == null)
                    throw new ArgumentException($"'{filterQuery.Key}' não é propriedade válida de '{concreteType}'");

                try
                {
                    var convertedValue = TypeHelper.ConvertType(filterQuery.Value, property.PropertyType);
                    var parameter = Expression.Parameter(concreteType, "model");

                    var left = Expression.PropertyOrField(parameter, property.Name);
                    var right = Expression.Constant(convertedValue, property.PropertyType);
                    var body = GetFilterExpressionLambda(left, right, filterQuery.Operation);

                    var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

                    return source.Where(lambda);
                }
                catch (FormatException)
                {

                    throw new Exception($"400 - Não é possivel converter {filterQuery.Value} para {property.PropertyType.Name}");

                }

            }

        }

        public static IQueryable<TEntity> Page<TEntity>(this IQueryable<TEntity> source, int pageSize, int pageNumber)
        {
            if (pageSize > 0)
            {
                if (pageNumber == 0)
                    pageNumber = 1;

                if (pageNumber > 0)
                    return source
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize);
                else // pagina fim do conjunto                  
                    return (source                        
                        .Skip((Math.Abs(pageNumber) - 1) * pageSize)
                        .Take(pageSize));
            }

            return source;
        }

        public static IQueryable<TEntity> Sort<TEntity>(this IQueryable<TEntity> source, List<SortQuery> sortQueries)
        {

            if (sortQueries == null || sortQueries.Count == 0)
                return source;

            var orderedEntities = source.Sort(sortQueries[0]);

            if (sortQueries.Count <= 1) return orderedEntities;

            for (var i = 1; i < sortQueries.Count; i++)
                orderedEntities = orderedEntities.Sort(sortQueries[i]);

            return orderedEntities;

        }

    }

}