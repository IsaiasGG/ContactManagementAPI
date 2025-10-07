using FluentValidation;
using IF.ContactManagement.Application.Interfaces.Repositories;

namespace IF.ContactManagement.Application.UseCases.Contact.Commands.Update
{
    public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            // Id must be greater than 0
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.");

            // Name is required and must have at least 2 characters
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MinimumLength(2)
                .WithMessage("Name must be at least 2 characters long.");

            // Email is optional but if provided must be a valid email format
            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Invalid email format.");

            // PhoneNumber is optional but if provided must match a simple pattern
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9\s\-]{7,15}$")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("Invalid phone number format.");

            RuleFor(x => x)
                .MustAsync(async (dto, ct) =>
                {
                    // Retrieve the contact by Id
                    var contact = await _unitOfWork.ContactRepository.GetByIdAsync(dto.Id);

                    // Validation fails if the contact does not exist or is marked as deleted
                    return contact != null && !contact.IsDeleted;
                })
                .WithMessage("The contact does not exist or has been deleted.");
        }

    }
}
