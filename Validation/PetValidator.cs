using FluentValidation;
using InnoGotchi_backend.Models.DTOs;

namespace InnoGotchi_frontend.Validation
{
    public class PetValidator:AbstractValidator<PetDto>
    {
        public PetValidator()
        {
            RuleFor(x=>x.PetName).NotEmpty();
        }
    }
}
