using IF.ContactManagement.Transversal.Common;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using IF.ContactManagement.Domain.Enums;

namespace IF.ContactManagement.Presentation.WebAPI.Controllers.Helpers
{
    public static class ValidationFormatter
    {
        public static IActionResult FormatErrors(ValidationResult validationResult, string method, string controller)
        {
            var errors = validationResult.Errors
                .Select(e => new Error
                {
                    Code = e.PropertyName,
                    Message = e.ErrorMessage,
                    ErrorType = ErrorType.Validation,
                    DateError = DateTime.UtcNow,
                    Method = method,
                    Controller = controller
                }).ToList();


            var errorResponse = new BadRequestResponse
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "An error occurred",
                Message = "The request contains validation errors.",
                Errors = errors
            };

            return new BadRequestObjectResult(errorResponse);
        }
    }
}
