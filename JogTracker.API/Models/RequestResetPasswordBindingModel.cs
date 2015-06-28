﻿using System;
using System.Collections.Generic;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using JogTracker.Api.Extensions;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using JogTracker.Api.Models.Validators;

namespace JogTracker.Api.Models
{
    public class RequestResetPasswordBindingModel : IValidatableObject
    {
        private readonly IValidator _validator;

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