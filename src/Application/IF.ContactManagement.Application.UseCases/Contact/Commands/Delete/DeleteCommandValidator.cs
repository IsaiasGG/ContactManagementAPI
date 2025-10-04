using FluentValidation;
using IF.ContactManagement.Application.Interfaces.Repositories;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.Delete
{
    public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IFundContactRepository _fundContactRepository;

        public DeleteCommandValidator(
            IContactRepository contactRepository,
            IFundContactRepository fundContactRepository)
        {
            _contactRepository = contactRepository;
            _fundContactRepository = fundContactRepository;

            // ContactId must be greater than 0
            RuleFor(x => x.ContactId)
                .GreaterThan(0)
                .WithMessage("ContactId must be greater than 0.");

            // Check if the contact exists in the database
            RuleFor(x => x.ContactId)
                .MustAsync(async (id, ct) => await _contactRepository.ExistsAsync(id, ct))
                .WithMessage("Contact with the provided ContactId does not exist.");

            // Check if the contact is assigned to any fund
            RuleFor(x => x.ContactId)
                .MustAsync(async (id, ct) => !await _fundContactRepository.IsAssignedToAnyFundAsync(id, ct))
                .WithMessage("You cannot delete a contact which is currently assigned to a fund.");
        }
    }
}
