using FluentValidation;
using IF.ContactManagement.Application.Interfaces.Repositories;

namespace IF.ContactManagement.Application.UseCases.Contact.Queries.GetByFund
{
    public class GetByFundQueryValidator : AbstractValidator<GetByFundQuery>
    {
        private readonly IFundRepository _fundRepository;

        public GetByFundQueryValidator(IFundRepository fundRepository)
        {
            _fundRepository = fundRepository;

            // FundId must be greater than 0
            RuleFor(x => x.FundId)
                .GreaterThan(0)
                .WithMessage("FundId must be greater than 0.");

            // Check if the fund exists in the database
            RuleFor(x => x.FundId)
                .MustAsync(async (id, ct) => await _fundRepository.ExistsAsync(id, ct))
                .WithMessage("Fund with the provided FundId does not exist.");
        }
    }
}
