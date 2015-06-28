﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using JogTracker.Api.Extensions;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace JogTracker.Api.Models
{
    public class JogFilterBindingModel : IValidatableObject
    {
        private readonly IValidator _validator;

        public JogFilterBindingModel()
        {
            _validator = new JogFilterBindingValidator();
        }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return _validator.Validate(this).ToValidationResult();
        }
    }
}