
using AutoMapper;
using IF.ContactManagement.Application.Interfaces.Repositories;
using IF.ContactManagement.Transversal.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.DeleteFund
{
    public class DeleteFundCommandHandler : IRequestHandler<DeleteFundCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteFundCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeleteFundCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteFundCommandHandler> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<bool>> Handle(DeleteFundCommand request, CancellationToken cancellationToken)
        {
            var result = new Response<bool>();

            try
            {
                var fundContact = await _unitOfWork.FundContactRepository.GetByIdAsync(request.FundId, request.ContactId);

                if (fundContact == null)
                    return new Response<bool> { IsSuccess = false, Message = "The specified Fund Contact could not be found." };

                var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var deleteFundContact = await _unitOfWork.FundContactRepository.SoftDeleteAsync(fundContact, user);

                if (deleteFundContact != null)
                {
                    result.Data = true;
                    result.IsSuccess = true;
                    result.Message = "The contact was removed from the fund.";
                }
                else
                {
                    result.Data = false;
                    result.IsSuccess = false;
                    result.Message = "Contact could not be removed from the fund.";
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
