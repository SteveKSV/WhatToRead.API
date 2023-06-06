using BlazorApp.EF.Models;
using FluentValidation;

namespace BlazorApp.EF.Helpers
{
    public class RegisterValidator : AbstractValidator<RegisterModel>
    {
        public RegisterValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("Your first name cannot be empty")
                     .MinimumLength(2).WithMessage("Your first name length must be at least 2.");
            RuleFor(p => p.LastName).NotEmpty().WithMessage("Your last name cannot be empty")
                    .MinimumLength(2).WithMessage("Your last name length must be at least 2.");
            RuleFor(p => p.Username).NotEmpty().WithMessage("Your user name cannot be empty")
                     .MinimumLength(3).WithMessage("Your first name length must be at least 3.");
            RuleFor(p => p.Email)
                .EmailAddress();
            RuleFor(p => p.Password).NotEmpty().WithMessage("Your password cannot be empty")
                   .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                   .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                   .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                   .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                   .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
        }
    }
}
