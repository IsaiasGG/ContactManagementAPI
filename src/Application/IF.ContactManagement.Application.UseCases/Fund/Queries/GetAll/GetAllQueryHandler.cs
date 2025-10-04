using AutoMapper;
using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Transversal.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IF.ContactManagement.Application.UseCases.Fund.Queries.GetAll
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
                var fundList = await _unitOfWork.FundRepository.GetAllAsync();

                if (fundList.Any())
                {
                    result.Data = _mapper.Map<List<GetAllQueryResponse>>(fundList.OrderBy(f => f.Id));
                    result.IsSuccess = true;
                    result.Message = "Successful query.";
                }
                else
                {
                    result.Data = new List<GetAllQueryResponse>(); 
                    result.IsSuccess = true; 
                    result.Message = "No funds found.";
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
