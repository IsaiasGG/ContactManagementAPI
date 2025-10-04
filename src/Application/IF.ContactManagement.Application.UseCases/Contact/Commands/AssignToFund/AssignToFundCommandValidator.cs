using FluentValidation;
using IF.ContactManagement.Application.Interfaces.Repositories;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.AssignToFund
{
    public class AssignToFundCommandValidator : AbstractValidator<AssignToFundCommand>
    {
        private readonly IFundContactRepository _fundContactRepository;

        public AssignToFundCommandValidator(IFundContactRepository fundContactRepository)
        {
            _fundContactRepository = fundContactRepository;

            // Basic validations
            RuleFor(x => x.FundId)
                .GreaterThan(0)
                .WithMessage("FundId must be greater than 0.");

            RuleFor(x => x.ContactId)
                .GreaterThan(0)
                .WithMessage("ContactId must be greater than 0.");

            // Business rule: the same Contact cannot be assigned to the same Fund more than once
            RuleFor(x => x)
                .MustAsync(async (dto, ct) =>
                    !await _fundContactRepository.ExistsAsync(dto.FundId, dto.ContactId))
                .WithMessage("The same Contact can only be assigned to a Fund once.");
        }
    }
}
