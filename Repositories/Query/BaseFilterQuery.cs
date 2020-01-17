using System;

namespace Repositories.Query
{

    public class BaseFilterQuery
    {

        protected FilterOperations GetFilterOperation(string prefix)
        {

            if (prefix.Length == 0) return FilterOperations.eq;

            if (!Enum.TryParse(prefix, out FilterOperations opertion))
                throw new Exception($"400 - Prefixo de filtro inv�lido '{prefix}'");

            return opertion;

        }

    }

}