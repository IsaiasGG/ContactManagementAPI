using FluentValidation;
using IF.ContactManagement.Application.Interfaces.Repositories;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.DeleteFund
{
    public class DeleteFundCommandValidator : AbstractValidator<DeleteFundCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteFundCommandValidator(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;

            // FundId must be greater than 0
            RuleFor(x => x.FundId)
                .GreaterThan(0)
                .WithMessage("FundId must be greater than 0.");

            // ContactId must be greater than 0
            RuleFor(x => x.ContactId)
                .GreaterThan(0)
                .WithMessage("ContactId must be greater than 0.");

            // Optional: check if the relationship exists
            RuleFor(x => x)
                .MustAsync(async (dto, ct) => await _unitOfWork.FundContactRepository.ExistsAsync(dto.FundId, dto.ContactId))
                .WithMessage("The FundContact relationship does not exist.");
        }
    }
}
