using IF.ContactManagement.Application.UseCases.Fund.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IF.ContactManagement.Presentation.WebAPI.Controllers.v1
{
    [Authorize]
    [Route("fund")]
    [ApiController]
    public class FundController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FundController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all available funds.
        /// </summary>
        /// <remarks>
        /// This endpoint sends a <see cref="GetAllQuery"/> request through the mediator to fetch a list of funds.
        /// </remarks>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the operation:
        /// - 200 OK with <see cref="GetAllQueryResponse"/> if the request is successful.
        /// - 400 Bad Request with an error message if the request fails validation or cannot be processed.
        /// - 401 Unauthorized if the user is not authorized to access this endpoint.
        /// </returns>
        /// <response code="200">Returns the list of funds wrapped in <see cref="GetAllQueryResponse"/>.</response>
        /// <response code="400">If the request is invalid or could not be processed.</response>
        /// <response code="401">If the user is unauthorized to access this endpoint.</response>
        [HttpGet("GetAllFunds")]
        [ProducesResponseType(typeof(GetAllQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllFunds()
        {
            var response = await _mediator.Send(new GetAllQuery());
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }
    }
}
