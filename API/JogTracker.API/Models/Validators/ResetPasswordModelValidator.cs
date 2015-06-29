using FluentValidation;
using JogTracker.Common;


namespace JogTracker.Api.Models.Validators
{
    public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordBindingModel>
    {
        public ResetPasswordModelValidator()
        {
            RuleFor(r => r.UserId).NotEmpty();
            RuleFor(r => r.NewPassword).NotEmpty();
            RuleFor(r => r.Token).NotEmpty();
            RuleFor(r => r.NewPassword).Must(MustBeValidPassword).WithMessage("Password did not meet minimum complexity requirements.");

        }

        private bool MustBeValidPassword(string password)
        {
            var result = GlobalConfig.PasswordValidator.ValidateAsync(password).Result;
            return result.Succeeded;
        }
    }
}