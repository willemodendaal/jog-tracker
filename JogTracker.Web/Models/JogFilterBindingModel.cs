using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using JogTracker.Web.Extensions;


namespace JogTracker.Web.Models
{
    public class JogFilterBindingModel : IValidatableObject
    {
        private readonly IValidator _validator;

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public JogFilterBindingModel()
        {
            _validator = new JogFilterBindingValidator();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return _validator.Validate(this).ToValidationResult();
        }
    }

    internal class JogFilterBindingValidator : AbstractValidator<JogFilterBindingModel>
    {
        public JogFilterBindingValidator()
        {
            RuleFor(jog => jog.FromDate).NotEmpty();
            RuleFor(jog => jog.ToDate).NotEmpty();
        }
    }
}