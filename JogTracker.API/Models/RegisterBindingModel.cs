using FluentValidation;
using JogTracker.Api.Models.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using JogTracker.Api.Extensions;

namespace JogTracker.Api.Models
{
    public class RegisterBindingModel : IValidatableObject
    {
        private readonly IValidator _validator;

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterBindingModel()
        {
            _validator = new RegisterBindingModelValidator();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return _validator.Validate(this).ToValidationResult();
        }

    }
}