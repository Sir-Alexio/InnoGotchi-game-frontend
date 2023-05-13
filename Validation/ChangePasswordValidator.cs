using FluentValidation;
using InnoGotchi_backend.Models.Entity;

namespace InnoGotchi_frontend.Models.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel>
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
