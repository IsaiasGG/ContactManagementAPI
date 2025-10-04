using FluentValidation;
using FluentValidation.Results;
using IF.ContactManagement.Application.DTO.User;
using IF.ContactManagement.Application.UseCases.Authentication.Queries.ValidateUser;
using IF.ContactManagement.Presentation.WebAPI.Controllers.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IF.ContactManagement.Presentation.WebAPI.Controllers.v1
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IValidator<ValidateUserQuery> validateUserValidator;
        private string ControllerName { get; }

        public AuthController(IMediator mediator, IValidator<ValidateUserQuery> validateUserValidator)
        {
            this.mediator = mediator;
            this.validateUserValidator = validateUserValidator;
            ControllerName = GetType().Name;
        }

        /// <summary>
        /// Authenticates a user using the provided credentials.
        /// </summary>
        /// <remarks>
        /// Example request:
        /// 
        ///     POST /Login
        ///     {
        ///        "UserName": "exampleuser",
        ///        "Password": "P@ssw0rd"
        ///     }
        /// 
        /// Returns a JSON object with access and refresh tokens, user roles, and module permissions if authentication is successful.
        /// </remarks>
        /// <param name="request">The login credentials encapsulated in a <see cref="ValidateUserQuery"/> object.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the authentication attempt:
        /// - 200 OK with <see cref="AuthResponseDTO"/> if authentication is successful.
        /// - 400 Bad Request if the request validation fails.
        /// - 401 Unauthorized if the credentials are invalid.
        /// </returns>
        /// <response code="200">User authenticated successfully, returning tokens and permissions.</response>
        /// <response code="400">Validation failed for the request.</response>
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] ValidateUserQuery request)
        {
            var query = new ValidateUserQuery(request.UserName, request.Password);
            ValidationResult validationResult = await validateUserValidator.ValidateAsync(query);

            if (!validationResult.IsValid)
            {
                string methodName = ControllerContext.ActionDescriptor.ActionName;
                return ValidationFormatter.FormatErrors(validationResult, methodName, ControllerName);
            }

            var response = await mediator.Send(query);
            if (response.IsSuccess)
                return Ok(response);

            return Unauthorized(response);
        }
     
    }
}
