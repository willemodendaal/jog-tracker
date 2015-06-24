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
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public JogFilterBindingModel()
        {
            _validator = new JogFilterBindingValidator();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return _validator.Validate(this).ToValidationResult();
        }
    }

   
}