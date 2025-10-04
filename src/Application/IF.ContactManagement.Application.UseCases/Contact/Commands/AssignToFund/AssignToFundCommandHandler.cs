using AutoMapper;
using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Transversal.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.AssignToFund
{
    public class AssignToFundCommandHandler : IRequestHandler<AssignToFundCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssignToFundCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AssignToFundCommandHandler(IUnitOfWork unitOfWork, ILogger<AssignToFundCommandHandler> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<bool>> Handle(AssignToFundCommand request, CancellationToken cancellationToken)
        {
            var result = new Response<bool>();

            try
            {
                var fundContact = _mapper.Map<Domain.Entities.FundContact>(request);
                var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var newFundContact = await _unitOfWork.FundContactRepository.AddAsync(fundContact, user);

                if (newFundContact != null)
                {
                    result.Data = true;
                    result.IsSuccess = true;
                    result.Message = "Contact was assigned to specified fund.";
                }
                else
                {
                    result.Data = false;
                    result.IsSuccess = false;
                    result.Message = "Contact could not be assigned to specified fund.";
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
