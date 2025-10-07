using FluentValidation;
using FluentValidation.Results;
using IF.ContactManagement.Application.DTO.Contact;
using IF.ContactManagement.Application.UseCases.Contact.Commands.AssignToFund;
using IF.ContactManagement.Application.UseCases.Contact.Commands.Create;
using IF.ContactManagement.Application.UseCases.Contact.Commands.Delete;
using IF.ContactManagement.Application.UseCases.Contact.Commands.DeleteFund;
using IF.ContactManagement.Application.UseCases.Contact.Commands.Update;
using IF.ContactManagement.Application.UseCases.Contact.Queries.GetAll;
using IF.ContactManagement.Application.UseCases.Contact.Queries.GetByFund;
using IF.ContactManagement.Application.UseCases.Contact.Queries.GetById;
using IF.ContactManagement.Presentation.WebAPI.Controllers.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IF.ContactManagement.Presentation.WebAPI.Controllers.v1
{
    [Authorize]
    [Route("contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<GetByIdQuery> _getByIdValidator;
        private readonly IValidator<GetByFundQuery> _getByFundValidator;
        private readonly IValidator<CreateCommand> _createValidator;
        private readonly IValidator<UpdateCommand> _updateValidator;
        private readonly IValidator<AssignToFundCommand> _assignToFundValidator;
        private readonly IValidator<DeleteCommand> _deleteValidator;
        private readonly IValidator<DeleteFundCommand> _deleteFundValidator;
        private string ControllerName { get; }

        public ContactController(IMediator mediator, IValidator<GetByIdQuery> getByIdValidator, IValidator<GetByFundQuery> getByFundValidator, IValidator<CreateCommand> createValidator,
            IValidator<UpdateCommand> updateValidator, IValidator<AssignToFundCommand> assignToFundValidator, IValidator<DeleteCommand> deleteValidator, IValidator<DeleteFundCommand> deleteFundValidator)
        {
            _mediator = mediator;
            _getByIdValidator = getByIdValidator;
            _getByFundValidator = getByFundValidator;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _assignToFundValidator = assignToFundValidator;
            _deleteValidator = deleteValidator;
            _deleteFundValidator = deleteFundValidator;
            ControllerName = GetType().Name;
        }

        /// <summary>
        /// Retrieves all available contacts.
        /// </summary>
        /// <remarks>
        /// This endpoint sends a <see cref="GetAllQuery"/> request through the mediator to fetch a list of contacts.
        /// </remarks>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the operation:
        /// - 200 OK with <see cref="GetAllQueryResponse"/> if the request is successful.
        /// - 400 Bad Request with an error message if the request fails validation or cannot be processed.
        /// - 401 Unauthorized if the user is not authorized to access this endpoint.
        /// </returns>
        /// <response code="200">Returns the list of contacts wrapped in <see cref="GetAllQueryResponse"/>.</response>
        /// <response code="400">If the request is invalid or could not be processed.</response>
        /// <response code="401">If the user is unauthorized to access this endpoint.</response>
        [HttpGet("GetAllContacts")]
        [ProducesResponseType(typeof(GetAllQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllContacts()
        {
            var response = await _mediator.Send(new GetAllQuery());
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Retrieves a contact by its unique identifier.
        /// </summary>
        /// <remarks>
        /// This endpoint sends a <see cref="GetByIdQuery"/> through the mediator to fetch the details
        /// of the contact associated with the specified <paramref name="ContactId"/>.
        /// </remarks>
        /// <param name="ContactId">
        /// The unique identifier of the contact to retrieve, provided as part of the URL route.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the operation:
        /// - 200 OK with <see cref="GetByIdQueryResponse"/> if the contact is found.
        /// - 400 Bad Request with an error message if the validation fails or the request cannot be processed.
        /// - 401 Unauthorized if the user is not authorized to access this endpoint.
        /// </returns>
        /// <response code="200">Returns the contact details wrapped in <see cref="GetByIdQueryResponse"/>.</response>
        /// <response code="400">If the provided ContactId is invalid or the request could not be processed.</response>
        /// <response code="401">If the user is unauthorized to access this endpoint.</response>
        [HttpGet("GetByIdContacts/{ContactId}")]
        [ProducesResponseType(typeof(GetByIdQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByIdContacts(int ContactId)
        {
            var query = new GetByIdQuery(ContactId);
            ValidationResult validationResult = await _getByIdValidator.ValidateAsync(query);

            if (!validationResult.IsValid)
            {
                string methodName = ControllerContext.ActionDescriptor.ActionName;
                return ValidationFormatter.FormatErrors(validationResult, methodName, ControllerName);
            }

            var response = await _mediator.Send(query);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Retrieves all contacts associated with a specific fund.
        /// </summary>
        /// <remarks>
        /// This endpoint sends a <see cref="GetByFundQuery"/> through the mediator to fetch the details
        /// of all contacts linked to the fund identified by <paramref name="FundId"/>.
        /// </remarks>
        /// <param name="FundId">
        /// The unique identifier of the fund for which to retrieve contacts. This value is provided in the URL route.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the operation:
        /// - 200 OK with <see cref="GetByFundQueryResponse"/> if contacts are found.
        /// - 400 Bad Request with validation errors if the request is invalid.
        /// - 401 Unauthorized if the user is not authorized to access this endpoint.
        /// </returns>
        /// <response code="200">Returns the list of contacts linked to the specified fund.</response>
        /// <response code="400">If the provided FundId is invalid or the request cannot be processed.</response>
        /// <response code="401">If the user is unauthorized to access this endpoint.</response>
        [HttpGet("GetContactsByFund/{FundId}")]
        [ProducesResponseType(typeof(GetByFundQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetContactsByFund(int FundId)
        {
            var query = new GetByFundQuery(FundId);
            ValidationResult validationResult = await _getByFundValidator.ValidateAsync(query);

            if (!validationResult.IsValid)
            {
                string methodName = ControllerContext.ActionDescriptor.ActionName;
                return ValidationFormatter.FormatErrors(validationResult, methodName, ControllerName);
            }

            var response = await _mediator.Send(query);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Creates a new contact.
        /// </summary>
        /// <param name="request">The contact details to create (Name, Email, PhoneNumber).</param>
        /// <returns>
        /// 200 OK if the contact is successfully created.  
        /// 400 Bad Request if validation fails or the request contains errors.  
        /// 401 Unauthorized if the user is not authenticated.
        /// </returns>
        [HttpPost("CreateContact")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateContact([FromBody] CreateContactDTO request)
        {
            var command = new CreateCommand(request.Name, request.Email, request.PhoneNumber);
            ValidationResult validationResult = await _createValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                string methodName = ControllerContext.ActionDescriptor.ActionName;
                return ValidationFormatter.FormatErrors(validationResult, methodName, ControllerName);
            }

            var response = await _mediator.Send(command);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Assigns a contact to a specific fund.
        /// </summary>
        /// <param name="request">The DTO containing FundId and ContactId.</param>
        /// <returns>
        /// 200 OK if the contact is successfully assigned.  
        /// 400 Bad Request if validation fails or the request is invalid.  
        /// 401 Unauthorized if the user is not authenticated.
        /// </returns>
        [HttpPost("AssignToFund")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AssignToFund([FromBody] AssignToFundDTO request)
        {
            var command = new AssignToFundCommand(request.FundId, request.ContactId);
            ValidationResult validationResult = await _assignToFundValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                string methodName = ControllerContext.ActionDescriptor.ActionName;
                return ValidationFormatter.FormatErrors(validationResult, methodName, ControllerName);
            }

            var response = await _mediator.Send(command);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Updates an existing contact's information.
        /// </summary>
        /// <param name="request">The DTO containing Id, Name, Email, and PhoneNumber.</param>
        /// <returns>
        /// 200 OK if the contact is successfully updated.  
        /// 400 Bad Request if validation fails or the request is invalid.  
        /// 401 Unauthorized if the user is not authenticated.
        /// </returns>
        [HttpPut("UpdateContact")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateContact([FromBody] UpdateContactDTO request)
        {
            var command = new UpdateCommand(request.Id, request.Name, request.Email, request.PhoneNumber);
            ValidationResult validationResult = await _updateValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                string methodName = ControllerContext.ActionDescriptor.ActionName;
                return ValidationFormatter.FormatErrors(validationResult, methodName, ControllerName);
            }

            var response = await _mediator.Send(command);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Deletes a contact by its ID.
        /// </summary>
        /// <param name="ContactId">The ID of the contact to delete.</param>
        /// <returns>
        /// 200 OK if the contact is successfully deleted.  
        /// 400 Bad Request if validation fails or the request is invalid.  
        /// 401 Unauthorized if the user is not authenticated.
        /// </returns>
        [HttpDelete]
        [Route("DeleteContact/{ContactId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteContact(int ContactId)
        {
            var command = new DeleteCommand(ContactId);
            ValidationResult validationResult = await _deleteValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                string methodName = ControllerContext.ActionDescriptor.ActionName;
                return ValidationFormatter.FormatErrors(validationResult, methodName, ControllerName);
            }

            var response = await _mediator.Send(command);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Removes a contact from a fund.
        /// </summary>
        /// <param name="FundId">The ID of the fund.</param>
        /// <param name="ContactId">The ID of the contact to remove.</param>
        /// <returns>
        /// 200 OK if the contact is successfully removed from the fund.  
        /// 400 Bad Request if validation fails or the request is invalid.  
        /// 401 Unauthorized if the user is not authenticated.
        /// </returns>
        [HttpDelete]
        [Route("{FundId}/RemoveFromFund/{ContactId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteCompanyAreaAsync(int FundId, int ContactId)
        {
            var command = new DeleteFundCommand(FundId, ContactId);
            ValidationResult validationResult = await _deleteFundValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                string methodName = ControllerContext.ActionDescriptor.ActionName;
                return ValidationFormatter.FormatErrors(validationResult, methodName, ControllerName);
            }

            var response = await _mediator.Send(command);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }
    }
}
