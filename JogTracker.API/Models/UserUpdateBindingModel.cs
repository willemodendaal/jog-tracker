using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;
using FluentValidation;
using JogTracker.Api.Extensions;
using JogTracker.Api.Models.Validators;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace JogTracker.Api.Models
{
    public class UserUpdateBindingModel : IValidatableObject
    {
        private readonly IValidator _validator;

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserUpdateBindingModel()
        {
            _validator = new UserUpdateBindingValidator();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return _validator.Validate(this).ToValidationResult();
        }

    }
}