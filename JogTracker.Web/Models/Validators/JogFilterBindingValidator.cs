using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JogTracker.Api.Models
{
    internal class JogFilterBindingValidator : AbstractValidator<JogFilterBindingModel>
    {
        public JogFilterBindingValidator()
        {
            RuleFor(jog => jog)
                .Must(FromMustBeLessThanTo)
                .WithMessage("FromDate must be smaller than ToDate.");

            RuleFor(jog => jog.PageSize)
                .NotEmpty()
                .InclusiveBetween(1,100); //To protect against misuse.
            
            RuleFor(jog => jog.PageIndex)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);

        }

        private bool FromMustBeLessThanTo(JogFilterBindingModel jog)
        {
            if (jog.FromDate == null || jog.ToDate == null)
            {
                return true;
            }

            if (jog.ToDate < jog.FromDate)
            {
                return false;
            }

            return true;
        }
    }
}