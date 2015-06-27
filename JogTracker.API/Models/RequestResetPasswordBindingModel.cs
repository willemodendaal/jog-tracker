using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using JogTracker.Api.Extensions;
using FluentValidation.Results;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using JogTracker.Api.Models.Validators;

namespace JogTracker.Api.Models
{
    public class RequestResetPasswordBindingModel : IValidatableObject
    {
        private readonly IValidator _validator;

        public string UserName { get; set; }
        public string Email { get; set; }

        public RequestResetPasswordBindingModel()
        {
            _validator = new RequestResetPasswordModelValidator();
        }

        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
        {
            return _validator.Validate(this).ToValidationResult();
        }

    }
}