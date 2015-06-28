using FluentValidation;

namespace JogTracker.Api.Models.Validators
{
    public class RequestResetPasswordModelValidator : AbstractValidator<RequestResetPasswordBindingModel>
    {
        public RequestResetPasswordModelValidator()
        {
            RuleFor(r => r.Email).NotEmpty();
        }

    }
}