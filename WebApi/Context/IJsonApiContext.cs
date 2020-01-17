using System.Collections.Generic;

namespace WebApi.Context
{

    public interface IJsonApiContext
    {

        Dictionary<string, string> Query { get; set; }

    }

}