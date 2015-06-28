using FluentValidation;
using JogTracker.Common;

namespace JogTracker.Api.Models.Validators
{
    public class RegisterBindingModelValidator : AbstractValidator<RegisterBindingModel>
    {
        public RegisterBindingModelValidator()
        {
            
            RuleFor(r => r.Email).NotEmpty();
            RuleFor(r => r.Email).EmailAddress();

            RuleFor(r => r.Password).NotEmpty();
            RuleFor(r => r.Password).Must(MustBeValidPassword).WithMessage("Password did not meet minimum complexity requirements.");
        }

        private bool MustBeValidPassword(string password)
        {
            var result = GlobalConfig.PasswordValidator.ValidateAsync(password).Result;

            return result.Succeeded;
        }

    }
}