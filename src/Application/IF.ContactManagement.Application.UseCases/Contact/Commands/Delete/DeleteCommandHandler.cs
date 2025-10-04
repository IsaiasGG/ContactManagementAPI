using AutoMapper;
using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Transversal.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.Delete
{
    public class DeleteCommandHandler : IRequestHandler<DeleteCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeleteCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteCommandHandler> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var result = new Response<bool>();

            try
            {
                var contact = await _unitOfWork.ContactRepository.GetByIdAsync(request.ContactId);

                if (contact == null)
                    return new Response<bool> { IsSuccess = false, Message = "Contact not found." };

               var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var deleteContact = await _unitOfWork.ContactRepository.SoftDeleteAsync(contact, user);

                if (deleteContact != null)
                {
                    result.Data = true;
                    result.IsSuccess = true;
                    result.Message = "The contact was deleted.";
                }
                else
                {
                    result.Data = false;
                    result.IsSuccess = false;
                    result.Message = "Contact could not be deleted.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                result.Message = ex.Message;

                BaseError error = new BaseError
                {
                    PropertyMessage = ex.Source,
                    ErrorMessage = ex.StackTrace
                };

                result.Errors = new List<BaseError> { error };
                result.IsSuccess = false;
            }

            return result;
        }
    }
}
