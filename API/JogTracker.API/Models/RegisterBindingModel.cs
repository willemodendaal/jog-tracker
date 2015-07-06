using JogTracker.Api.Models.Validators;

namespace JogTracker.Api.Models
{
    public class RegisterBindingModel : BindingModelBase
    {
        public RegisterBindingModel()
        {
            _validator = new RegisterBindingModelValidator();
        }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
        public bool UserManager { get; set; }
    }
}