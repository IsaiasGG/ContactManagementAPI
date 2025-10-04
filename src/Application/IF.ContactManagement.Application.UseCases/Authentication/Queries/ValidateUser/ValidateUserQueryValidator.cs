using FluentValidation;

namespace IF.ContactManagement.Application.UseCases.Authentication.Queries.ValidateUser
{
    public class ValidateUserQueryValidator : AbstractValidator<ValidateUserQuery>
    {
        public ValidateUserQueryValidator() 
        {
            // UserName: required, 3–50 characters, only letters, numbers, and some allowed symbols
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.")
                .Matches(@"^[a-zA-Z0-9._\-@]+$").WithMessage("Username can only contain letters, numbers, dots, underscores, dashes, and '@'.");

            // Password: required, minimum 8 characters, maximum 100, must include uppercase, lowercase, digit, and special character
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(8, 100).WithMessage("Password must be between 8 and 100 characters.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one digit.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character (e.g., !@#$%).");

        }
    }
}
