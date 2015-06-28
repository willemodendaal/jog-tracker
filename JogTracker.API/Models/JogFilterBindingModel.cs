using System;

namespace JogTracker.Api.Models
{
    public class JogFilterBindingModel : BindingModelBase
    {
        public JogFilterBindingModel()
        {
            _validator = new JogFilterBindingValidator();
        }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}