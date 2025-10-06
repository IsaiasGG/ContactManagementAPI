using AutoMapper;
using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Application.Interfaces.Services;
using IF.ContactManagement.Transversal.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IF.ContactManagement.Application.UseCases.Contact.Queries.GetById
{
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Response<GetByIdQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetByIdQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        public GetByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetByIdQueryHandler> logger, IMapper mapper, IIdentityService identityService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _identityService = identityService;
        }
        public async Task<Response<GetByIdQueryResponse>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var result = new Response<GetByIdQueryResponse>();

            try
            {
                var contactList = await _unitOfWork.ContactRepository.GetByIdAsync(request.ContactId);

                if (contactList != null)
                {
                    string? createdByName = contactList.CreatedBy != null
                       ? await _identityService.GetUserNameAsync(contactList.CreatedBy)
                       : null;

                    string? updatedByName = contactList.UpdatedBy != null
                        ? await _identityService.GetUserNameAsync(contactList.UpdatedBy)
                        : null;

                    string? deletedByName = contactList.DeletedBy != null
                        ? await _identityService.GetUserNameAsync(contactList.DeletedBy)
                        : null;

                    result.Data = _mapper.Map<GetByIdQueryResponse>(contactList);

                    result.Data.CreatedByName = createdByName;
                    result.Data.UpdatedByName = updatedByName;
                    result.Data.DeletedByName = deletedByName;

                    result.IsSuccess = true;
                    result.Message = "Successful query.";
                }
                else
                {
                    result.Data = new GetByIdQueryResponse();
                    result.IsSuccess = true;
                    result.Message = "No contacts found.";
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
