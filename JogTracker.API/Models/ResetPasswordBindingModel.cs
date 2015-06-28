using JogTracker.Api.Models.Validators;

namespace JogTracker.Api.Models
{
    public class ResetPasswordBindingModel : BindingModelBase
    {
        public ResetPasswordBindingModel()
        {
            _validator = new ResetPasswordModelValidator();
        }

        public string UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}