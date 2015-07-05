using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using JogTracker.Common;

namespace JogTracker.Api.Models.Validators
{
    public class AccountUpdateBindingValidator : AbstractValidator<AccountUpdateBindingModel>
    {
        public AccountUpdateBindingValidator()
        {
            RuleFor(r => r.FirstName).NotEmpty().Length(1, 100);
            RuleFor(r => r.LastName).NotEmpty().Length(1, 100);
        }


    }
}