using FluentValidation;

namespace JogTracker.Api.Models.Validators
{
    public class UserFilterBindingValidator : AbstractValidator<UserFilterBindingModel>
    {
        public UserFilterBindingValidator()
        {
            RuleFor(jog => jog.PageSize)
                .NotEmpty()
                .InclusiveBetween(1, 100); //To protect against misuse.

            RuleFor(jog => jog.PageIndex)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);
        }
    }
}