using FluentValidation;
using InnoGotchi_backend.Models;

namespace InnoGotchi_frontend.Models
{
    public class ChangePasswordValidator: AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword).NotNull();
            RuleFor(x => x.NewPassword).NotNull();
            RuleFor(x => x.ConfirmPassword).NotNull();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.NewPassword);
        }
    }
}
