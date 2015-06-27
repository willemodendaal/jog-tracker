using FluentValidation;
using JogTracker.Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace JogTracker.Api.Models.Validators
{
    public class RegisterBindingModelValidator : AbstractValidator<RegisterBindingModel>
    {
        public RegisterBindingModelValidator()
        {
            RuleFor(r => r.UserName).Must(MustBeValidUserName).WithMessage("Username may only contain letters and numbers.");
            RuleFor(r => r.UserName).Length(3, 50).WithMessage("Username must be between 3 and 50 characters long.");
            
            RuleFor(r => r.Email).NotEmpty();
            RuleFor(r => r.Email).EmailAddress();

            RuleFor(r => r.Password).NotEmpty();
            RuleFor(r => r.Password).Must(MustBeValidPassword).WithMessage("Password did not meet minimum complexity requirements.");
        }

        private bool MustBeValidPassword(string password)
        {
            var result = Config.PasswordValidator.ValidateAsync(password).Result;

            return result.Succeeded;
        }

        private bool MustBeValidUserName(string userName)
        {
            Regex r = new Regex(@"^\w+$"); //Only letters and numbers.
            return r.IsMatch(userName);
        }
    }
}