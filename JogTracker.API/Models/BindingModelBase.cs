using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FluentValidation;
using JogTracker.Api.Extensions;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace JogTracker.Api.Models
{
    public class BindingModelBase : IValidatableObject
    {
        protected IValidator _validator;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return _validator.Validate(this).ToValidationResult();
        }
    }
}