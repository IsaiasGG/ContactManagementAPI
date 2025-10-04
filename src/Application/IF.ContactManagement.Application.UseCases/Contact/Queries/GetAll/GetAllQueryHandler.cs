using AutoMapper;
using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Transversal.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IF.ContactManagement.Application.UseCases.Contact.Queries.GetAll
{
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, Response<List<GetAllQueryResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetAllQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Response<List<GetAllQueryResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var result = new Response<List<GetAllQueryResponse>>();

            try
            {
                var contactList = await _unitOfWork.ContactRepository.GetAllAsync();

                if (contactList.Any())
                {
                    result.Data = _mapper.Map<List<GetAllQueryResponse>>(contactList.OrderBy(c => c.Id));
                    result.IsSuccess = true;
                    result.Message = "Successful query.";
                }
                else
                {
                    result.Data = new List<GetAllQueryResponse>();
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
