using Xunit;
using Moq;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using IF.ContactManagement.Presentation.WebAPI.Controllers.v1;
using IF.ContactManagement.Application.UseCases.Authentication.Queries.ValidateUser;
using IF.ContactManagement.Application.DTO.User;
using IF.ContactManagement.Transversal.Common;

namespace IF.ContactManagement.UnitTests.Controllers
{
    public class AuthControllerUnitTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<ValidateUserQuery>> _validatorMock;
        private readonly AuthController _controller;

        public AuthControllerUnitTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _validatorMock = new Mock<IValidator<ValidateUserQuery>>();

            _controller = new AuthController(_mediatorMock.Object, _validatorMock.Object);

            // Configure ControllerContext to avoid NullReferenceException
            _controller.ControllerContext = new ControllerContext
            {
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor
                {
                    ActionName = "Auth"
                }
            };
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenValidCredentials()
        {
            // Arrange
            var request = new ValidateUserQuery("user", "password");

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidateUserQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ValidateUserQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new Response<AuthResponseDTO>
                         {
                             IsSuccess = true,
                             Data = new AuthResponseDTO
                             {
                                 AccessToken = "access",
                                 RefreshToken = "refresh"
                             }
                         });

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<AuthResponseDTO>>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal("access", response.Data.AccessToken);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var request = new ValidateUserQuery("", "");

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidateUserQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("UserName", "Required") }));

            // Act
            var result = await _controller.Login(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<BadRequestResponse>(badRequest.Value);
            Assert.Equal(400, errorResponse.Status);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsInvalid()
        {
            // Arrange
            var request = new ValidateUserQuery("user", "wrongpassword");

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidateUserQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ValidateUserQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new Response<AuthResponseDTO>
                         {
                             IsSuccess = false,
                             Message = "Invalid credentials"
                         });

            // Act
            var result = await _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = Assert.IsType<Response<AuthResponseDTO>>(unauthorizedResult.Value);
            Assert.False(response.IsSuccess);
            Assert.Equal("Invalid credentials", response.Message);
        }
    }
}
