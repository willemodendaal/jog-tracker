using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using JogTracker.Api.Extensions;
using JogTracker.Api.Models.Validators;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;


namespace JogTracker.Api.Models
{
    public class JogBindingModel : BindingModelBase
    {

        public JogBindingModel()
        {
            _validator = new JogBindingValidator();
        }

        public DateTime DateTime { get; set; }
        public float DistanceKM { get; set; }
        public TimeSpan Duration { get; set; }

    }
}