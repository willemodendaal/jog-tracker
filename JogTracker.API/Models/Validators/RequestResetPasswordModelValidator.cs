using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogTracker.Api.Models.Validators
{
    public class RequestResetPasswordModelValidator : AbstractValidator<RequestResetPasswordBindingModel>
    {
        public RequestResetPasswordModelValidator()
        {
            RuleFor(r => r).Must(EitherEmailOrUserNameSpecified).WithMessage("Either UserName or Email must be specified.");
        }

        private bool EitherEmailOrUserNameSpecified(RequestResetPasswordBindingModel model)
        {
            return !string.IsNullOrWhiteSpace(model.Email) || !string.IsNullOrWhiteSpace(model.UserName);
        }
    }
}