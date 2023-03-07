using FluentValidation;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_frontend.Services.Abstract;

namespace InnoGotchi_frontend.Services
{
    public class FarmValidationRepository : ValidationRepositoryBase<FarmDto>, IFarmValidation
    {
        public FarmValidationRepository(IValidator<FarmDto> validator) : base(validator)
        {
        }
    }
}
