using JogTracker.Api.Models.Validators;

namespace JogTracker.Api.Models
{
    public class RequestResetPasswordBindingModel : BindingModelBase
    {
        public string Email { get; set; }

        public RequestResetPasswordBindingModel()
        {
            _validator = new RequestResetPasswordModelValidator();
        }
    }
}