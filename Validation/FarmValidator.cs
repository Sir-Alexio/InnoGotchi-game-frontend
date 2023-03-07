using FluentValidation;
using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_frontend.Models.Validators
{
    public class FarmValidator:AbstractValidator<FarmDto>
    {
        public FarmValidator()
        {
            RuleFor(x=>x.FarmName).NotEmpty();
        }
    }
}
