using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JogTracker.Web.Extensions
{
    /// <summary>
    /// Extension to the Fluent Validation Types.
    /// From: https://brettedotnet.wordpress.com/2013/05/01/asp-net-web-api-validation-a-one-more-better-approach/
    /// </summary>
    public static class FluentValidationExtensions
    {
        /// <summary>
        /// Converts the Fluent Validation result to the type the both MVC and ef expect
        /// </summary>
        /// <param name="validationResult">The validation result.</param>
        /// <returns></returns>
        public static IEnumerable<ValidationResult> ToValidationResult(
            this FluentValidation.Results.ValidationResult validationResult)
        {
            var results = validationResult.Errors.Select(item => new ValidationResult(item.ErrorMessage, new List<string> { item.PropertyName }));
            return results;
        }
    }
}