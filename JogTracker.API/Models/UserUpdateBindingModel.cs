using JogTracker.Api.Models.Validators;

namespace JogTracker.Api.Models
{
    public class UserUpdateBindingModel : BindingModelBase
    {
        public UserUpdateBindingModel()
        {
            _validator = new UserUpdateBindingValidator();
        }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}