using Xunit;
using Moq;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using IF.ContactManagement.Presentation.WebAPI.Controllers.v1;
using IF.ContactManagement.Application.UseCases.Contact.Queries.GetAll;
using IF.ContactManagement.Application.UseCases.Contact.Queries.GetById;
using IF.ContactManagement.Application.UseCases.Contact.Queries.GetByFund;
using IF.ContactManagement.Application.UseCases.Contact.Commands.Create;
using IF.ContactManagement.Application.UseCases.Contact.Commands.Update;
using IF.ContactManagement.Application.UseCases.Contact.Commands.AssignToFund;
using IF.ContactManagement.Application.UseCases.Contact.Commands.Delete;
using IF.ContactManagement.Application.UseCases.Contact.Commands.DeleteFund;
using IF.ContactManagement.Application.DTO.Contact;
using IF.ContactManagement.Transversal.Common;

namespace IF.ContactManagement.UnitTests.Controllers
{
    public class ContactControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<GetByIdQuery>> _getByIdValidatorMock;
        private readonly Mock<IValidator<GetByFundQuery>> _getByFundValidatorMock;
        private readonly Mock<IValidator<CreateCommand>> _createValidatorMock;
        private readonly Mock<IValidator<UpdateCommand>> _updateValidatorMock;
        private readonly Mock<IValidator<AssignToFundCommand>> _assignValidatorMock;
        private readonly Mock<IValidator<DeleteCommand>> _deleteValidatorMock;
        private readonly Mock<IValidator<DeleteFundCommand>> _deleteFundValidatorMock;
        private readonly ContactController _controller;

        public ContactControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _getByIdValidatorMock = new Mock<IValidator<GetByIdQuery>>();
            _getByFundValidatorMock = new Mock<IValidator<GetByFundQuery>>();
            _createValidatorMock = new Mock<IValidator<CreateCommand>>();
            _updateValidatorMock = new Mock<IValidator<UpdateCommand>>();
            _assignValidatorMock = new Mock<IValidator<AssignToFundCommand>>();
            _deleteValidatorMock = new Mock<IValidator<DeleteCommand>>();
            _deleteFundValidatorMock = new Mock<IValidator<DeleteFundCommand>>();

            _controller = new ContactController(
                _mediatorMock.Object,
                _getByIdValidatorMock.Object,
                _getByFundValidatorMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                _assignValidatorMock.Object,
                _deleteValidatorMock.Object,
                _deleteFundValidatorMock.Object
            );
        }

        #region GetAllContacts
        [Fact]
        public async Task GetAllContacts_ReturnsOk_WhenSuccess()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response<List<GetAllQueryResponse>>
                {
                    IsSuccess = true,
                    Data = new List<GetAllQueryResponse> { new GetAllQueryResponse { Id = 1, Name = "Test" } }
                });

            var result = await _controller.GetAllContacts();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<List<GetAllQueryResponse>>>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetAllContacts_ReturnsBadRequest_WhenFails()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response<List<GetAllQueryResponse>>
                {
                    IsSuccess = false,
                    Message = "Error"
                });

            var result = await _controller.GetAllContacts();
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region GetByIdContacts
        [Fact]
        public async Task GetByIdContacts_ReturnsOk_WhenValidationPasses()
        {
            _getByIdValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<GetByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response<GetByIdQueryResponse>
                {
                    IsSuccess = true,
                    Data = new GetByIdQueryResponse { Id = 1, Name = "Test" }
                });

            var result = await _controller.GetByIdContacts(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<GetByIdQueryResponse>>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(1, response.Data.Id);
        }

        [Fact]
        public async Task GetByIdContacts_ReturnsBadRequest_WhenValidationFails()
        {
            _getByIdValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<GetByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("ContactId", "Invalid") }));

            var result = await _controller.GetByIdContacts(1);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region GetContactsByFund
        [Fact]
        public async Task GetContactsByFund_ReturnsOk_WhenValidationPasses()
        {
            _getByFundValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<GetByFundQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetByFundQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response<List<GetByFundQueryResponse>>
                {
                    IsSuccess = true,
                    Data = new List<GetByFundQueryResponse> { new GetByFundQueryResponse { Id = 1, Name = "Test" } }
                });

            var result = await _controller.GetContactsByFund(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<List<GetByFundQueryResponse>>>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetContactsByFund_ReturnsBadRequest_WhenValidationFails()
        {
            _getByFundValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<GetByFundQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("FundId", "Invalid") }));

            var result = await _controller.GetContactsByFund(1);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region CreateContact
        [Fact]
        public async Task CreateContact_ReturnsOk_WhenValid()
        {
            _createValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response<bool> { IsSuccess = true });

            var result = await _controller.CreateContact(new CreateContactDTO { Name = "Name", Email = "a@b.com", PhoneNumber = "123" });
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateContact_ReturnsBadRequest_WhenValidationFails()
        {
            _createValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Required") }));

            var result = await _controller.CreateContact(new CreateContactDTO { Name = "", Email = "", PhoneNumber = "" });
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region AssignToFund
        [Fact]
        public async Task AssignToFund_ReturnsOk_WhenValid()
        {
            _assignValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<AssignToFundCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(It.IsAny<AssignToFundCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response<bool> { IsSuccess = true });

            var result = await _controller.AssignToFund(new AssignToFundDTO { FundId = 1, ContactId = 2 });
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AssignToFund_ReturnsBadRequest_WhenValidationFails()
        {
            _assignValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<AssignToFundCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("FundId", "Invalid") }));

            var result = await _controller.AssignToFund(new AssignToFundDTO { FundId = 0, ContactId = 0 });
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region UpdateContact
        [Fact]
        public async Task UpdateContact_ReturnsOk_WhenValid()
        {
            _updateValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response<bool> { IsSuccess = true });

            var result = await _controller.UpdateContact(new UpdateContactDTO { Id = 1, Name = "Name", Email = "a@b.com", PhoneNumber = "123" });
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateContact_ReturnsBadRequest_WhenValidationFails()
        {
            _updateValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Required") }));

            var result = await _controller.UpdateContact(new UpdateContactDTO { Id = 1, Name = "", Email = "", PhoneNumber = "" });
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region DeleteContact
        [Fact]
        public async Task DeleteContact_ReturnsOk_WhenValid()
        {
            _deleteValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response<bool> { IsSuccess = true });

            var result = await _controller.DeleteContact(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteContact_ReturnsBadRequest_WhenValidationFails()
        {
            _deleteValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("ContactId", "Invalid") }));

            var result = await _controller.DeleteContact(0);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region DeleteFund
        [Fact]
        public async Task DeleteFund_ReturnsOk_WhenValid()
        {
            _deleteFundValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteFundCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteFundCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response<bool> { IsSuccess = true });

            var result = await _controller.DeleteCompanyAreaAsync(1, 2);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteFund_ReturnsBadRequest_WhenValidationFails()
        {
            _deleteFundValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteFundCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("FundId", "Invalid") }));

            var result = await _controller.DeleteCompanyAreaAsync(0, 0);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion
    }
}
