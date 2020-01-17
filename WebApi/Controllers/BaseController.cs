using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repositories;
using WebApi.Context;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        protected BaseContext BaseContext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected IJsonApiContext JsonApiContext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected IConfiguration Configuration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseContext"></param>
        public BaseController(BaseContext baseContext)
        {
            this.BaseContext = baseContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseContext"></param>
        /// <param name="jsonApiContext"></param>
        /// <param name="configuration"></param>
        public BaseController(BaseContext baseContext, IJsonApiContext jsonApiContext, IConfiguration configuration)
        {
            this.BaseContext = baseContext;
            this.JsonApiContext = jsonApiContext;
            this.Configuration = configuration;
        }

    }

}