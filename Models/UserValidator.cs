using FluentValidation;
using InnoGotchi_backend.Models;

namespace InnoGotchi_frontend.Models
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x=> x.UserName).NotEmpty();
            RuleFor(x=> x.FirstName).NotEmpty();
            RuleFor(x=> x.LastName).NotEmpty();
            RuleFor(x=> x.Email).NotEmpty();
            RuleFor(x=> x.Password).NotEmpty();
        }

    }
}
