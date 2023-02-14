using FluentValidation;
using InnoGotchi_backend.Models;

namespace InnoGotchi_frontend.Models
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x=> x.UserName).NotNull();
            RuleFor(x=> x.FirstName).NotNull();
            RuleFor(x => x.LastName).NotNull();
            RuleFor(x=> x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }

    }
}
