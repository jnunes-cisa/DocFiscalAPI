using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebApi.Context
{

    public class JsonApiContext : IJsonApiContext
    {

        public Dictionary<string, string> Query { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JsonApiContext(IHttpContextAccessor httpContextAccessor)
        {
            
            _httpContextAccessor = httpContextAccessor;
            Query = ReturnQuery();

        }

        private Dictionary<string, string> ReturnQuery()
        {

            var lstQuery = new Dictionary<string, string>();

            foreach (var itemQuery in _httpContextAccessor.HttpContext.Request.Query)
            {
                lstQuery.Add(itemQuery.Key, itemQuery.Value);
            }

            return lstQuery;

        }
       
    }

}