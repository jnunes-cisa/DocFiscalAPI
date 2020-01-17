using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using WebApi.Exceptions;

namespace WebApi.Filters
{

    public class ApiExceptionFilter : IExceptionFilter
    {

        public void OnException(ExceptionContext context)
        {

            var status = HttpStatusCode.InternalServerError;
            var message = string.Empty;

            //var modelError = new ModelStateDictionary();

            var exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Acesso não autorizado;";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = "Recurso solicitado não implementado.";
                status = HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(CustomResultException))
            {

                var exceptionCustom = (context.Exception as CustomResultException);

                status = exceptionCustom.StatusCode;

                message = string.IsNullOrWhiteSpace(context.Exception.Message) ? string.Empty : $"{context.Exception.Message} - {context.Exception.InnerException?.Message}";

                if (exceptionCustom.ModelState != null && exceptionCustom.ModelState?.Count > 0)
                {

                    message = string.Empty;
                    //modelError = exceptionCustom.ModelState;

                }

            }
            else
            {
                message = string.IsNullOrWhiteSpace(context.Exception.Message) ? string.Empty : $"{context.Exception.Message} - {context.Exception.InnerException?.Message} - {context.Exception.StackTrace}";
                status = HttpStatusCode.InternalServerError;
            }

            if (!string.IsNullOrWhiteSpace(message))
            {

                context.ModelState.AddModelError(status.ToString(), message);

            }

            //if (context.ModelState == null && modelError?.Count > 0)
            //{

            //    context.Result = new ObjectResult(new SerializableError(modelError)) { StatusCode = (int)status };

            //}
            //else if (context.ModelState != null && modelError?.Count > 0)
            //{

            context.Result = new ObjectResult(new SerializableError(context.ModelState)) { StatusCode = (int)status };

            //}

        }

    }

}