using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace JogTracker.Api.Models.Validators
{
    public class JogBindingValidator : AbstractValidator<JogBindingModel>
    {
        public JogBindingValidator()
        {
            RuleFor(r => r.DateTime).NotEmpty();
            RuleFor(r => r.DistanceKM).NotEmpty().GreaterThan(0);
            RuleFor(r => r.Duration).NotEmpty().GreaterThan(TimeSpan.Zero);
        }
    }
}