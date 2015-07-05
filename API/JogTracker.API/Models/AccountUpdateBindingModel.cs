using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JogTracker.Api.Models.Validators;

namespace JogTracker.Api.Models
{
    public class AccountUpdateBindingModel : BindingModelBase
    {
        public AccountUpdateBindingModel()
        {
            _validator = new AccountUpdateBindingValidator();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}