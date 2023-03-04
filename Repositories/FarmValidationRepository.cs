using FluentValidation;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_frontend.Services;

namespace InnoGotchi_frontend.Repositories
{
    public class FarmValidationRepository:ValidationRepositoryBase<FarmDto>,IFarmValidation
    {
        public FarmValidationRepository(IValidator<FarmDto> validator):base(validator)
        {
        }
    }
}
