using JogTracker.Api.Models.Validators;

namespace JogTracker.Api.Models
{
    public class UserFilterBindingModel : BindingModelBase
    {
        public UserFilterBindingModel()
        {
            _validator = new UserFilterBindingValidator();
        }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}