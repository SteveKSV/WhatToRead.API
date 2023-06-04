using FluentValidation;
using Identity.Data.Models;

namespace Identity.Validators
{
    /// <summary>
    /// Validator for SignIn
    /// </summary>
    public class LoginValidator : AbstractValidator<TokenRequestModel>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LoginValidator()
        {
            RuleFor(p => p.Email)
                .EmailAddress();
            RuleFor(p => p.Password).NotEmpty().WithMessage("Your password cannot be empty")
                   .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                   .MaximumLength(16).WithMessage("Your password length must not exceed 16.");
        }
    }
}
