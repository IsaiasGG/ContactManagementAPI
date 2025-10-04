using IF.ContactManagement.Application.UseCases.Fund.Queries.GetAll;
using IF.ContactManagement.Presentation.WebAPI.Controllers.v1;
using IF.ContactManagement.Transversal.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace IF.ContactManagement.UnitTests.Controllers
{
    public class FundControllerUnitTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly FundController _controller;

        public FundControllerUnitTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new FundController(_mediatorMock.Object);
        }

        #region GetAllFunds
        [Fact]
        public async Task GetAllFunds_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var response = new Response<List<GetAllQueryResponse>>
            {
                IsSuccess = true,
                Data = new List<GetAllQueryResponse>
                {
                    new GetAllQueryResponse { Id = 1, Name = "Test Fund" }
                },
                Message = "Funds retrieved successfully"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllFunds();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<Response<List<GetAllQueryResponse>>>(okResult.Value);
            Assert.True(resultValue.IsSuccess);
            Assert.Single(resultValue.Data);
        }

        [Fact]
        public async Task GetAllFunds_ReturnsBadRequest_WhenFails()
        {
            // Arrange
            var response = new Response<List<GetAllQueryResponse>>
            {
                IsSuccess = false,
                Message = "No funds found"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllFunds();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response.Message, badRequestResult.Value);
        }
        #endregion
    }
}
