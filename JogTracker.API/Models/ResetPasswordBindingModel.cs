using System;
using System.Collections.Generic;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using JogTracker.Api.Extensions;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using JogTracker.Api.Models.Validators;

namespace JogTracker.Api.Models
{
    public class ResetPasswordBindingModel : IValidatableObject
    {
        private readonly IValidator _validator;

        public string UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }

        public ResetPasswordBindingModel()
        {
            _validator = new ResetPasswordModelValidator();
        }

        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
        {
            return _validator.Validate(this).ToValidationResult();
        }
    }
}