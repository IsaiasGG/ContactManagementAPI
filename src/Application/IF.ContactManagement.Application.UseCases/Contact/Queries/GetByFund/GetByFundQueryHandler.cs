using AutoMapper;
using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Application.UseCases.Contact.Queries.GetById;
using IF.ContactManagement.Transversal.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IF.ContactManagement.Application.UseCases.Contact.Queries.GetByFund
{
    public class GetByFundQueryHandler : IRequestHandler<GetByFundQuery, Response<List<GetByFundQueryResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetByFundQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetByFundQueryHandler(IUnitOfWork unitOfWork, ILogger<GetByFundQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Response<List<GetByFundQueryResponse>>> Handle(GetByFundQuery request, CancellationToken cancellationToken)
        {
            var result = new Response<List<GetByFundQueryResponse>>();

            try
            {
                var contactList = await _unitOfWork.FundContactRepository.GetByFundIdAsync(request.FundId);

                if (contactList.Count > 0)
                {
                    result.Data = _mapper.Map<List<GetByFundQueryResponse>>(contactList);
                    result.IsSuccess = true;
                    result.Message = "Successful query.";
                }
                else
                {
                    result.Data = new List<GetByFundQueryResponse>();
                    result.IsSuccess = true;
                    result.Message = "No contacts were found for the specified fund.";
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
