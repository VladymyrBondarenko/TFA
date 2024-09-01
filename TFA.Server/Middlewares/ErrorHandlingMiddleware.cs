using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;

namespace TFA.Server.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, 
            RequestDelegate next,
            ProblemDetailsFactory problemDetailsFactory) 
        {
            _logger = logger;
            _next = next;
            _problemDetailsFactory = problemDetailsFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _logger.LogInformation("Error handling started for request in path {RequestPath}", 
                    context.Request.Path.Value);

                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                ProblemDetails problemDetails;

                switch (exception)
                {
                    case IntentionManagerException:
                        problemDetails = _problemDetailsFactory.CreateFrom(context, exception as IntentionManagerException);
                        break;
                    case ValidationException:
                        problemDetails = _problemDetailsFactory.CreateFrom(context, exception as ValidationException);
                        break;
                    case DomainException:
                        problemDetails = _problemDetailsFactory.CreateFrom(context, exception as DomainException);
                        _logger.LogInformation(exception, "Domain exception error occured");
                        break;
                    default:
                        problemDetails = _problemDetailsFactory.CreateFrom(context, exception);
                        _logger.LogInformation(exception, "Unhandled exception error occured");
                        break;
                }

                context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError; 
                await context.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType());
            }
        }
    }
}
