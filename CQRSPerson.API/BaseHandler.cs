using CQRSPerson.Domain.Errors;
using CQRSPerson.Domain.Logging;
using CQRSPerson.Domain.Responses;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CQRSPerson.API
{
    public abstract class BaseHandler<TReponseContent, THandler>
    {
        internal List<ApiError> ConvertFluentErrorsToApiErrors(IList<ValidationFailure> errors, string context)
        {
            return errors?
                .Select(error => new ApiError(error.ErrorCode, context, error.ErrorMessage))
                .AsEnumerable()
                .ToList() ?? new List<ApiError>();
        }

        internal StandardContentResponse<TReponseContent> HandleInternalServerError(Exception exception, IApplicationLogger<THandler> logger, string informationalMessage, string errorCode, string context)
        {
            logger.LogError(exception, informationalMessage);
            var response = new StandardContentResponse<TReponseContent>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                InformationalMessage = informationalMessage,
                Errors = new List<ApiError>()
                {
                    new ApiError(errorCode, context, exception.Message)
                }
            };
            return response;
        }

        internal StandardContentResponse<TReponseContent> HandleValidationError(List<ValidationFailure> validationErrors, string informationalMessage, string context)
        {
            return HandleValidationError(ConvertFluentErrorsToApiErrors(validationErrors, context), informationalMessage);
        }

        internal StandardContentResponse<TReponseContent> HandleValidationError(List<ApiError> apiErrors, string informationalMessage)
        {
            var response = new StandardContentResponse<TReponseContent>
            {
                StatusCode = HttpStatusCode.BadRequest,
                InformationalMessage = informationalMessage,
                Errors = apiErrors
            };
            return response;
        }

        internal StandardContentResponse<TReponseContent> HandleGenericFailure(string informationalMessage, string errorCode, string context, string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
        {
            var response = new StandardContentResponse<TReponseContent>
            {
                StatusCode = httpStatusCode,
                InformationalMessage = informationalMessage,
                Errors = new List<ApiError>()
                {
                    new ApiError(errorCode, context, message)
                }
            };
            return response;
        }

    }
}
