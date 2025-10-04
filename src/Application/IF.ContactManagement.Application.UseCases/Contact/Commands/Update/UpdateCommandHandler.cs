using AutoMapper;
using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Transversal.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.Update
{
    public class UpdateCommandHandler : IRequestHandler<UpdateCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateCommandHandler> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var result = new Response<bool>();

            try
            {
                var contact = _mapper.Map<Domain.Entities.Contact>(request);
                contact.UpdatedAt = DateTime.UtcNow;
                contact.UpdatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var updateContact = await _unitOfWork.ContactRepository.UpdateAsync(contact);

                if (updateContact != null)
                {
                    result.Data = true;
                    result.IsSuccess = true;
                    result.Message = "Contact was updated.";
                }
                else
                {
                    result.Data = false;
                    result.IsSuccess = false;
                    result.Message = "Contact could not be updated.";
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
