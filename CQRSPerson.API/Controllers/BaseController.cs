using CQRSPerson.Domain.Errors;
using CQRSPerson.Domain.Logging;
using CQRSPerson.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace CQRSPerson.API.Controllers
{
    public class BaseController<T> : ControllerBase
    {
        internal ObjectResult  HandleInternalServerError<U>(string informationalMessage,string code, string context, Exception ex, IApplicationLogger<T> logger)
        {
            logger.LogError(ex, informationalMessage);
            var response = new StandardContentResponse<U>()
            {
                InformationalMessage = informationalMessage,
                Errors = new List<ApiError> { new ApiError(code, context, ex.Message) },
                StatusCode = HttpStatusCode.InternalServerError
            };

            return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.InternalServerError };
        }
    }
}
