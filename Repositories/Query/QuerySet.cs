using System.Collections.Generic;

namespace Repositories.Query
{

    public class QuerySet
    {

        public List<FilterQuery> Filters { get; set; } = new List<FilterQuery>();

        public PageQuery PageQuery { get; set; } = new PageQuery();

        public List<SortQuery> SortParameters { get; set; } = new List<SortQuery>();

        public List<string> IncludedRelationships { get; set; } = new List<string>();

        public List<string> Fields { get; set; } = new List<string>();

        public ReturnResult ReturnResult { get; set; }

    }

    public class ReturnResult
    {

        public int TotalRegisters { get; set; }

        public int TotalRegistersPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

    }

}