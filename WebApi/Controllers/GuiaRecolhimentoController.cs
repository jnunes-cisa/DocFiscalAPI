using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repositories;
using Services;
using Services.Interfaces;
using WebApi.Context;

namespace WebApi.Controllers
{

    /// <summary>
    /// GuiaRecolhimentoController
    /// </summary>
    public class GuiaRecolhimentoController : BaseController
    {
        private IGuiaRecolhimentoService _guiaRecolhimentoService;

        /// <summary>
        /// ContentorDispositivoController
        /// </summary>
        /// <param name="baseContext"></param>
        /// <param name="jsonApiContext"></param>
        /// <param name="configuration"></param>
        public GuiaRecolhimentoController(BaseContext baseContext, IJsonApiContext jsonApiContext, IConfiguration configuration) : base(baseContext, jsonApiContext, configuration)
        {
            _guiaRecolhimentoService = new GuiaRecolhimentoService(baseContext, jsonApiContext?.Query);
        }

        /// <summary>
        /// Obtém uma lista de guias pendentes
        /// </summary>
        /// <returns>Lista de guias pendentes</returns>
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(StatusCodes.Status204NoContent);

            /*return Ok(new ResultApi
            {
                Data = _guiaRecolhimentoService.GetAll(),
                Meta = _guiaRecolhimentoService.GetQuerySet()?.ReturnResult
            });*/

        }
    }
}