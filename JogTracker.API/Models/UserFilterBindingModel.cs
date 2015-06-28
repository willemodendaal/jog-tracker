using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using JogTracker.Api.Extensions;
using JogTracker.Api.Models.Validators;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace JogTracker.Api.Models
{
    public class UserFilterBindingModel : IValidatableObject
    {
        private readonly IValidator _validator;

        public UserFilterBindingModel()
        {
            _validator = new UserFilterBindingValidator();
        }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = _validator.Validate(this).ToValidationResult();

            return result;
        }
    }
}