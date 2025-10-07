using FluentValidation;
using IF.ContactManagement.Application.Interfaces.Repositories;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.Delete
{
    public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            // ContactId must be greater than 0
            RuleFor(x => x.ContactId)
                .GreaterThan(0)
                .WithMessage("ContactId must be greater than 0.");

            // Check if the contact exists in the database
            RuleFor(x => x.ContactId)
                .MustAsync(async (id, ct) => await _unitOfWork.ContactRepository.ExistsAsync(id, ct))
                .WithMessage("Contact with the provided ContactId does not exist.");

            // Check if the contact is assigned to any fund
            RuleFor(x => x.ContactId)
                .MustAsync(async (id, ct) => !await _unitOfWork.FundContactRepository.IsAssignedToAnyFundAsync(id, ct))
                .WithMessage("You cannot delete a contact which is currently assigned to a fund.");
        }
    }
}
