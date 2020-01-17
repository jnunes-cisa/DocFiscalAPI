using Common.Extensions;
using Repositories.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.Helpers
{

    public static class QueryHelper
    {

        public static QuerySet Parse(Dictionary<string, string> query)
        {

            var querySet = new QuerySet();

            if (query != null && query.Count > 0)
            {


                foreach (var pair in query)
                {

                    if (pair.Key.StartsWith("filter"))
                    {
                        querySet.Filters.AddRange(ParseFilterQuery(pair.Key, pair.Value));
                        continue;
                    }

                    if (pair.Key.StartsWith("sort"))
                    {
                        querySet.SortParameters = ParseSortParameters(pair.Value);
                        continue;
                    }

                    if (pair.Key.StartsWith("include"))
                    {
                        querySet.IncludedRelationships = ParseIncludedRelationships(pair.Value);
                        continue;
                    }

                    if (pair.Key.StartsWith("page"))
                    {
                        querySet.PageQuery = ParsePageQuery(querySet.PageQuery, pair.Key, pair.Value);
                        continue;
                    }

                    if (pair.Key.StartsWith("fields"))
                    {
                        querySet.Fields = ParseFieldsQuery(pair.Key, pair.Value);
                        continue;
                    }

                }

            }

            return querySet;

        }

        private static List<FilterQuery> ParseFilterQuery(string key, string value)
        {

            var queries = new List<FilterQuery>();

            var propertyName = key.Split('[', ']')[1].Trim().ToProperCase();

            var values = value.Split(',');
            foreach (var val in values)
            {

                (var operation, var filterValue) = ParseFilterOperation(val);

                var operationFilter = FilterOperations.eq;

                switch (operation)
                {
                    case "eq":
                        operationFilter = FilterOperations.eq;
                        break;
                    case "lt":
                        operationFilter = FilterOperations.lt;
                        break;
                    case "gt":
                        operationFilter = FilterOperations.gt;
                        break;
                    case "le":
                        operationFilter = FilterOperations.le;
                        break;
                    case "ge":
                        operationFilter = FilterOperations.ge;
                        break;
                    case "like":
                        operationFilter = FilterOperations.like;
                        break;
                    default:
                        operationFilter = FilterOperations.eq;
                        break;
                }

                queries.Add(new FilterQuery(propertyName, filterValue, operationFilter));

            }

            return queries;

        }

        private static (string operation, string value) ParseFilterOperation(string value)
        {

            if (value.Length < 3)
                return (string.Empty, value);

            var operation = value.Trim().Split(':');

            if (operation.Length == 1)
                return (string.Empty, value);

            if (!Enum.TryParse(operation[0].Trim(), out FilterOperations op))
                return (string.Empty, value);

            var prefix = operation[0].Trim();
            value = string.Join(":", operation.Skip(1));

            return (prefix.ToLower(), value.Trim());

        }

        private static PageQuery ParsePageQuery(PageQuery pageQuery, string key, string value)
        {

            pageQuery = pageQuery ?? new PageQuery();

            var propertyName = key.Split('[', ']')[1];

            if (propertyName.Trim() == "size")
                pageQuery.PageSize = Convert.ToInt32(value.Trim());
            else if (propertyName.Trim() == "number")
                pageQuery.PageOffset = Convert.ToInt32(value.Trim());

            return pageQuery;

        }

        private static List<SortQuery> ParseSortParameters(string value)
        {

            var sortParameters = new List<SortQuery>();

            value.Trim().Split(',').ToList().ForEach(p =>
            {
                var direction = SortDirection.Ascending;
                if (p.Length > 0 && p[0] == '-')
                {
                    direction = SortDirection.Descending;
                    p = p.Substring(1);
                }

                if (!string.IsNullOrWhiteSpace(p))
                    sortParameters.Add(new SortQuery(direction, p.Trim().ToProperCase()));

            });

            return sortParameters;

        }

        private static List<string> ParseIncludedRelationships(string value)
        {

            var retorno = new List<string>();

            foreach (var itemValue in value.Split(','))
            {

                var valor = itemValue.Trim();
                if (!string.IsNullOrWhiteSpace(valor))
                {

                    retorno.Add(valor);

                }

            }

            return retorno;

        }

        private static List<string> ParseFieldsQuery(string key, string value)
        {

            var typeName = key.Split('[', ']')[1];

            var includedFields = new List<string> { "Id" };

            var fields = value.Split(',');
            foreach (var field in fields)
            {
                includedFields.Add(field);
            }

            return includedFields;

        }

    }

}