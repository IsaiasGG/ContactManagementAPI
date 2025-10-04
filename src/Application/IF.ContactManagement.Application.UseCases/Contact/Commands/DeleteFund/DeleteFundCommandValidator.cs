using FluentValidation;
using IF.ContactManagement.Application.Interfaces.Repositories;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.DeleteFund
{
    public class DeleteFundCommandValidator : AbstractValidator<DeleteFundCommand>
    {
        private readonly IFundContactRepository _fundContactRepository;
        public DeleteFundCommandValidator(IFundContactRepository fundContactRepository) 
        {
            _fundContactRepository = fundContactRepository;

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
                .MustAsync(async (dto, ct) => await _fundContactRepository.ExistsAsync(dto.FundId, dto.ContactId))
                .WithMessage("The FundContact relationship does not exist.");
        }
    }
}
