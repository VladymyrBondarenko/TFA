using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;

namespace TFA.Server.Middlewares
{
    public static class ProblemDetailsFactoryExtensions
    {
        public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, 
            HttpContext context, IntentionManagerException ex)
        {
            return factory.CreateProblemDetails(context, StatusCodes.Status403Forbidden,
                "Authorization Failed", detail: ex.Message);
        }

        public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory,
            HttpContext context, DomainException domainException)
        {
            var statusCode = domainException.ErrorCode switch
            {
                DomainErrorCode.Gone => StatusCodes.Status410Gone,
                _ => StatusCodes.Status500InternalServerError
            };

            return factory.CreateProblemDetails(context, statusCode, domainException.Message, 
                detail: domainException.Message);
        }

        public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory,
            HttpContext context, ValidationException validationException)
        {
            var state = new ModelStateDictionary();

            foreach (var error in validationException.Errors)
            {
                state.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return factory.CreateValidationProblemDetails(context, state,
                StatusCodes.Status400BadRequest, "Validation failed", 
                detail: validationException.Message);
        }

        public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory,
            HttpContext context, Exception ex)
        {
            return factory.CreateProblemDetails(context, StatusCodes.Status500InternalServerError, "Unknown Error");
        }
    }
}