using FluentValidation;
using IF.ContactManagement.Application.Interfaces.Repositories;

namespace IF.ContactManagement.Application.UseCases.Contact.Queries.GetById
{
    public class GetByIdQueryValidator : AbstractValidator<GetByIdQuery>
    {
        private readonly IContactRepository _contactRepository;

        public GetByIdQueryValidator(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;

            // ContactId must be greater than 0
            RuleFor(x => x.ContactId)
                .GreaterThan(0)
                .WithMessage("ContactId must be greater than 0.");

            // Check if the contact exists in the database
            RuleFor(x => x.ContactId)
                .MustAsync(async (id, ct) => await _contactRepository.ExistsAsync(id))
                .WithMessage("Contact with the provided ContactId does not exist.");
        }
    }
}
