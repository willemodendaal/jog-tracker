using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using JogTracker.Common;

namespace JogTracker.Api.Models.Validators
{
    public class UserUpdateBindingValidator : AbstractValidator<UserUpdateBindingModel>
    {
        public UserUpdateBindingValidator()
        {
            RuleFor(r => r.Email).NotEmpty();
            RuleFor(r => r.Email).EmailAddress();

            RuleFor(r => r.FirstName).NotEmpty().Length(1, 100);
            RuleFor(r => r.LastName).NotEmpty().Length(1, 100);
        }


    }
}