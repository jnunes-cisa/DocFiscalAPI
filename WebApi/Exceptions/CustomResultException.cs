using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Net;

namespace WebApi.Exceptions
{

    public class CustomResultException : Exception
    {

        public HttpStatusCode StatusCode { get; set; }
        public ModelStateDictionary ModelState { get; set; }

        public CustomResultException()
        {

            
            
        }

        public CustomResultException(ModelStateDictionary modelState)
        {

            StatusCode = HttpStatusCode.InternalServerError;
            ModelState = modelState;

        }

        public CustomResultException(ModelStateDictionary modelState, HttpStatusCode httpStatusCode)
        {

            StatusCode = httpStatusCode;
            ModelState = modelState;

        }

        public CustomResultException(string message) : base(message)
        {

            StatusCode = HttpStatusCode.InternalServerError;

        }

        public CustomResultException(string message, HttpStatusCode httpStatusCode) : base(message)
        {

            StatusCode = httpStatusCode;

        }

        public CustomResultException(string message, Exception innerException) : base(message, innerException)
        {

            StatusCode = HttpStatusCode.InternalServerError;

        }

        public CustomResultException(string message, Exception innerException, HttpStatusCode httpStatusCode) : base(message, innerException)
        {

            StatusCode = httpStatusCode;

        }

    }

}